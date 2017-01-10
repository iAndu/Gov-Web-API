using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIGovApp.Models
{
    public class ErrorMessage
    {
        public int code { get; set; }
        public string error { get; set; }
        public string type { get; set; }
    }
}