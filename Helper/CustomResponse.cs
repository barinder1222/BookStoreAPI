using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStoreAPI.Helper
{
    public class CustomResponse
    {
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }


    }
}