﻿using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using WebApiDocumentationLibrary.Entities;

namespace WebApiDocumentationWebApplication.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Paths> Paths { get; set; }
        public string WebApiTitle { get; internal set; }
        public string WebApiUrl { get; internal set; }        
        public OpenApiComponents Components { get; internal set; }
    }
}