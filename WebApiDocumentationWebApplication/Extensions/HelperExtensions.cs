using ConsoleApp.Entities;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebApiDocumentationWebApplication.Extensions
{
    public static class HelperExtensions
    {
        public static string GetBadge(this Operation operation)
        {
            return operation.Method switch
            {
                "post" => "badge-post",
                "get" => "badge-get",
                "put" => "badge-put",
                "delete" => "badge-delete",
                "head" => "badge-head",
                _ => string.Empty
            };
        }

        
    }
}
