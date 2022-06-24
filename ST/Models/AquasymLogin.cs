using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ST.Models
{
    public class AquasymLogin
    {
        public string targetUrl { get; set; }
        public bool success { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
        public error error { get; set; }
        public result result { get; set; }
    }


    public class error
    {
        public string code { get; set; }
        public string message { get; set; }
        public string details { get; set; }
        public bool? validationErrors { get; set; }
    }

    public class result
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}