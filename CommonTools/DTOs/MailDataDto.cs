
using System.Collections.Generic;
using System.IO;

namespace CommonTools.DTOs
{
    public class MailDataDto
    {
        // Receiver
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public List<AttachmentFileDto> Attachments { get; set; } = null;
        public List<string> EmailsCC { get; set; }
    }
}
