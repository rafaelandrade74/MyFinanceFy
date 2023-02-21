using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;
using MyFinanceFy.Libs.Ajuda;

namespace MyFinanceFy.Libs.Servicos
{
    public class EmailSender
    {
        readonly IConfiguration _configuration;
        MimeMessage message = new ();
        public EmailSender(IConfiguration configuration)
        {

            _configuration = configuration;

        }
        public void To(string email)
        {
            message.To.Add(new MailboxAddress(email, email));
        }


        public Task SendEmailAsync(string subject, string htmlMessage)
        {
            return Task.Run(() =>
            {
                string pastaBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string LogoUrl = Path.Combine(pastaBase, "img","logo.png");
                string templateEmailUrl = Path.Combine(pastaBase, "Email", "Template.html");

                string templateEmail = File.ReadAllText(templateEmailUrl);
                message.From.Add(new MailboxAddress(_configuration.GetValue<string>("Email:name"), _configuration.GetValue<string>("Email:username")));
                message.Subject = $"{Constantes.MyFinancefy} - {subject}";

                BodyBuilder? builder = new ();
                MimeEntity? logo = builder.LinkedResources.Add(LogoUrl);
                logo.ContentId = MimeUtils.GenerateMessageId();
                builder.HtmlBody = templateEmail.Replace("{{corpo}}", htmlMessage).Replace("{{logo_url}}", $"cid:{logo.ContentId}");
                message.Body = builder.ToMessageBody();

                using SmtpClient? client = new ();
                client.Connect(_configuration.GetValue<string>("Email:host"), _configuration.GetValue<int>("Email:port"), _configuration.GetValue<bool>("Email:ssl"));

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_configuration.GetValue<string>("Email:username"), _configuration.GetValue<string>("Email:password"));

                client.Send(message);
                client.Disconnect(true);
            });
        }
    }
}
