using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.Models
{
    public class ResponseDto
    {
        public int code { get; set; }
        public string message { get; set; }
        public bool error { get; set; }
        public dynamic response { get; set; }
    }
}
