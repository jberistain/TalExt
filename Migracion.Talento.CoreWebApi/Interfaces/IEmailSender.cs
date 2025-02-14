using CommonTools.DTOs;
using Migracion.Talento.CoreWebApi.Services;

namespace Migracion.Talento.CoreWebApi.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailDataDto mailData);
        //Task SendEmailAttachmentAsycn(MailDataDto mailData);
        string GetWelcomeTemplateEmail<T>(string emailTamplate, T emailTemplateModel);
        Task DisconnectSmtpClient();
    }
}
