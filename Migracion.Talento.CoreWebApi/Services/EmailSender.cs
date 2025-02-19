using Microsoft.Extensions.Options;
using Migracion.Talento.CoreWebApi.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using RazorEngineCore;
using System.Text;
using CommonTools.DTOs;
using System.IO;

namespace Migracion.Talento.CoreWebApi.Services
{
    public class EmailSender: IEmailSender
    {
        //private readonly SmtpClient _client;
        private readonly EmailSenderOptions _options;
        private readonly SmtpClient _client;

        public EmailSender(IOptions<EmailSenderOptions> options)
        {
            _options = options.Value;
             _client = new SmtpClient();
            Task Connect=Task.Run(()=> _client.ConnectAsync(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls));
            Connect.Wait();
           
           Task Autheticate= Task.Run( ()=> _client.AuthenticateAsync(_options.Email, _options.Password));
            Autheticate.Wait();
        }

        public async Task SendEmailAsync (MailDataDto data)
        {


            #region sender
            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));
            mail.Sender = new MailboxAddress("Invite", _options.Email);

            // Receiver
            //foreach (string mailAddress in data.To)
            mail.To.Add(MailboxAddress.Parse(data.To));
            if (data.EmailsCC.Count > 0)
            {
                foreach (string emailCC in data.EmailsCC)
                {
                    mail.Cc.Add(MailboxAddress.Parse(emailCC));
                }
            }
            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = data.Subject;
            body.HtmlBody = data.Body;

            //if(data.attachments != null)
                //body.Attachments.Add("PDF Test.pdf", data.FileStreamBytes);

            //// Check if we got any attachments and add the to the builder for our message
            if (data.Attachments != null)
            {
                foreach (var attachment in data.Attachments)
                {
                    // Check if length of the file in bytes is larger than 0
                    if (attachment.File.Length > 0)
                    {
                        // Add the attachment from the byte array
                        body.Attachments.Add(attachment.FileName, attachment.File, ContentType.Parse("Application/pdf"));
                    }
                }
            }
            mail.Body = body.ToMessageBody();

            #endregion

            #region SendMail

            await _client.SendAsync(mail);
            
            #endregion 
        }

        public async Task DisconnectSmtpClient()
        {
            await _client.DisconnectAsync(true);
        }

        public String GetWelcomeTemplateEmail<T>(string emailTemplate, T emailTemplateModel)
        {
            string mailTemplate = LoadTemplate(emailTemplate);

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedMailTemplate = razorEngine.Compile(mailTemplate);

            return modifiedMailTemplate.Run(emailTemplateModel);
        }
        public string LoadTemplate(string emailTemplate)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string templateDir = Path.Combine(baseDir, "MailTemplates");
            string templatePath = Path.Combine(templateDir, $"{emailTemplate}.cshtml");

            using FileStream fileStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader streamReader = new StreamReader(fileStream, Encoding.Default);

            string mailTemplate = streamReader.ReadToEnd();
            streamReader.Close();

            return mailTemplate;
        }


    }
   
}
