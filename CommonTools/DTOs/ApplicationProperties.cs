using System;
using System.Collections.Generic;
using System.Text;

namespace CommonTools.DTOs
{
    public  class ApplicationProperties
    {
        public ApplicationProperties() { }
        public string UrlToConfirm { get; set; }
        public string UrlToConfirmReceiveDocumentation { get; set; }
        public string UrlToSearchCode { get; set; }
    }
}
