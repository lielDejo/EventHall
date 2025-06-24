using EventModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using WebApiClient;
using EventModels;
using System.Windows.Controls;
using System.Net;
using System.Windows.Interop;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using System.Net.Http;

namespace HallEventAdmin
{
    public partial class Window3 : Window
    {
        private string idUser;
        private Stream image;
        private FileInfo imageFile;
        private string imagePath = "";
        private string userName = "";
        private Window mainWindow;

        public Window3(Window mainW,  string iduser, string userName)
        {
            mainWindow = mainW;
            idUser = iduser;
            this.userName = userName;
            InitializeComponent();
            LoadCitiesAsync();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.Close();
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                imagePath = dlg.FileName;
                this.image =File.OpenRead(imagePath);   
                this.imageFile = new FileInfo(imagePath);
                ImagePathTextBlock.Text = imagePath;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                PreviewImage.Source = bitmap;
            }
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if ( string.IsNullOrWhiteSpace(HallNameTextBox.Text) ||
    string.IsNullOrWhiteSpace(CapacityTextBox.Text) ||
    string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
    string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
    string.IsNullOrWhiteSpace(this.imagePath))
            {
                MessageBox.Show("אנא מלא את כל השדות וצרף תמונה ראשית לאולמך .");
                return;
            }

            if (!int.TryParse(CapacityTextBox.Text, out _))
            {
                MessageBox.Show("תכולת האנשים צריכה להיות מספר.");
                return;
            }

            string city = CityComboBox.SelectedItem as string;

            //WebClient<City> webClient = new WebClient<City>();
            //webClient.Schema = "http";
            //webClient.Port = 5232;
            //webClient.Host = "localhost";
            //webClient.Path = "api/Manager/addCity";
            //City city1 = new City();
            //city1.CityName = city;
            //city1.Halls = new List<EventHall>();
            //bool iscityId = await webClient.Post(city1);
            //int cityId = 0;
            //if (iscityId)
            //{
            //    WebClient<int> webClient1 = new WebClient<int>();
            //    webClient1.Schema = "http";
            //    webClient1.Port = 5232;
            //    webClient1.Host = "localhost";
            //    webClient1.Path = "api/Manager/getIDbyCityName";
            //    webClient1.AddParam("CityName", city);
            //    cityId = await webClient1.Get();
            //}
            ComboBoxItem selectedItem = HallTypeComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null)
            {
                MessageBox.Show("אנא בחר סוג אולם");
                return;
            }

            string hallType = selectedItem.Tag.ToString();


            EventHall eventHall = new EventHall();
            eventHall.TypeHall = hallType;
            eventHall.DescriptionHall= DescriptionTextBox.Text;
            eventHall.cityName = city;
            eventHall.PeopleContent = CapacityTextBox.Text; 
            eventHall.GeographicalLocation = AddressTextBox.Text;
            eventHall.HallName = HallNameTextBox.Text;
            eventHall.OwnerId = idUser;
            eventHall.HallImage = new List<Image_>
{
    new Image_
    {
        ImageName = "1", // למשל: hall1.jpg
        ImageAddress = this.imageFile.Extension,
        HallId = "" // אם עדיין לא נוצר HallId – אפשר להשאיר ריק
    }
};

            WebClient<EventHall> webClient2 = new WebClient<EventHall>();
            webClient2.Schema = "http";
            webClient2.Port = 5232;
            webClient2.Host = "localhost";
            webClient2.Path = "api/Manager/CreateNewHall";
            string hall_Id = "";
            try
            {
                bool isWork = await webClient2.Post(eventHall, this.image);
                if (isWork)
                {
                    
                    MessageBox.Show("האולם נוסף בהצלחה!\n\n");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"שגיאה בטעינת הנתונים מהשרת: {ex.Message}");
            }
           
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // חזרה לחלון הראשי או הקודם
            Window2 back = new Window2(mainWindow, idUser, userName);
            back.Show();
            this.Close();
        }
        private async void LoadCitiesAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = "https://data.gov.il/api/3/action/datastore_search?resource_id=5c78e9fa-c2e2-4771-93ff-7f400a12f7ba&limit=5000";

                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string content = await response.Content.ReadAsStringAsync();

                    // הדפסת התוכן כדי לבדוק מה התקבל
                    Console.WriteLine(content);

                    // נניח שהתשובה בפורמט JSON ואתה משתמש ב-Newtonsoft.Json
                    dynamic json = JsonConvert.DeserializeObject(content);

                    // נניח שהמפתח "records" מכיל את רשימות הערים
                    foreach (var record in json.result.records)
                    {
                        string cityName = record["שם_ישוב"];
                        CityComboBox.Items.Add(cityName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת רשימת הערים: " + ex.Message);
            }
        }

    }
}
