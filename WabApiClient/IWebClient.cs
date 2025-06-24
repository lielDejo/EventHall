﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    public interface IWebClient<T>
    {
        Task<T> Get();
        Task<bool> Post(T model, List<FileStream> files);
        Task<bool> Post(T model);
        Task<bool> Post(T model, Stream file);
        

    }
}