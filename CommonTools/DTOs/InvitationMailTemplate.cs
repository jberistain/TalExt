using iTextSharp.text;
using System.Collections.Generic;
using System.IO;

namespace CommonTools.DTOs
{
    public class InvitationMailTemplate
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string NumSolicitud { get; set; }
        public string Evento { get; set; }
        public string Subject { get; set; }
        public string UrlToConfirm { get; set; }
        public List<AttachmentFileDto> attachments { get; set; }
    }
}
