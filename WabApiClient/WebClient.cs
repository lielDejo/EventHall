using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace WebApiClient
{
    public class WebClient<T> : IWebClient<T>
    {
        UriBuilder uriBuilder;
        HttpRequestMessage request;
        HttpResponseMessage response;

        public WebClient()
        {
            this.uriBuilder = new UriBuilder();
            this.request = new HttpRequestMessage();
        }

        public string Schema
        {
            set { this.uriBuilder.Scheme = value; }
        }
        public string Host
        {
            set { this.uriBuilder.Host = value; }
        }
        public int Port
        {
            set { this.uriBuilder.Port = value; }
        }
        public string Path
        {
            set { this.uriBuilder.Path = value; }
        }

        public void AddParam(string name, string value)
        {
            if (this.uriBuilder.Query == string.Empty)
                this.uriBuilder.Query = "?";
            else
                this.uriBuilder.Query += "&";

            this.uriBuilder.Query += $"{name}={value}";
        }

        public async Task<T> Get()
        {
            this.request.Method = HttpMethod.Get;
            this.request.RequestUri = new Uri(this.uriBuilder.ToString());

            using (HttpClient client = new HttpClient())
            {
                this.response = await client.SendAsync(this.request);

                if (this.response.IsSuccessStatusCode)
                {
                    string content = await this.response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response: " + content); // לבדוק מה מחזיר השרת

                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)content; // החזר כמחרוזת אם T הוא string
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<T>(content); // JSON המרה ל-
                    }
                }
            }

            return default(T);
        }

        public async Task<bool> Post( T model, List<FileStream> files)
        {
            this.request.Method = HttpMethod.Post;
            this.request.RequestUri = new Uri(this.uriBuilder.ToString());
            MultipartFormDataContent multipartFormData = new MultipartFormDataContent();

            ObjectContent<T> objectContent = new ObjectContent<T>(model, new JsonMediaTypeFormatter());
            multipartFormData.Add(objectContent, "model");
            foreach (Stream stream in files)
            {
                StreamContent streamContent = new StreamContent(stream);
                multipartFormData.Add(streamContent, "file","file");
            }
            this.request.Content = multipartFormData;
            using (HttpClient client = new HttpClient())
            {
                this.response = await client.SendAsync(this.request);
                if (this.response.IsSuccessStatusCode == true)
                {
                    return await this.response.Content.ReadAsAsync<bool>();
                }
            }
            return false;
        }


        public async Task<bool> Post(T model)
        {
            this.request.Method = HttpMethod.Post;
            this.request.RequestUri = new Uri(this.uriBuilder.ToString());
            ObjectContent<T> objectContent = new ObjectContent<T>(model, new JsonMediaTypeFormatter());
            this.request.Content = objectContent;
            using (HttpClient client = new HttpClient())
            {
                this.response = await client.SendAsync(this.request);
                if (this.response.IsSuccessStatusCode == true)
                {
                    return await this.response.Content.ReadAsAsync<bool>();
                }
            }
            return false;
        }

        public async Task<bool> Post(T model, Stream file)
        {
            this.request.Method = HttpMethod.Post;
            this.request.RequestUri = new Uri(this.uriBuilder.ToString());
            MultipartFormDataContent multipartFormData = new MultipartFormDataContent();
            ObjectContent<T> objectContent = new ObjectContent<T>(model, new JsonMediaTypeFormatter());
            StreamContent streamContent = new StreamContent(file);
            multipartFormData.Add(objectContent, "model");
            multipartFormData.Add(streamContent,"file","file");
            this.request.Content = multipartFormData;
            using (HttpClient client = new HttpClient())
            {
                this.response = await client.SendAsync(this.request);
                if (this.response.IsSuccessStatusCode == true)
                {
                    return await this.response.Content.ReadAsAsync<bool>();
                }
            }
            return false;
        }

       

    }
}