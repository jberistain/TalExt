using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonTools.DTOs
{
    public class AttachmentFileDto
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
