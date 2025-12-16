using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebHomework.Data // veya namespace WebHomework.Services
{
    // Bu sınıf sisteme "Ben e-posta göndericisiyim" der ama aslında hiçbir şey yapmaz.
    // Böylece hata almadan kayıt işlemini geçeriz.
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Gerçekten mail atma, sadece görevi tamamlandı olarak işaretle.
            return Task.CompletedTask;
        }
    }
}
