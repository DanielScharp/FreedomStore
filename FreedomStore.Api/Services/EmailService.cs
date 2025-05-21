using FreedomStore.Api.Email;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FreedomStore.Api.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task EnviarEmail(EmailRequest props)
        {
            // instanciar classe de mensagem 'mimemessage' 
            var message = new MimeMessage();

            //from address
            message.From.Add(new MailboxAddress(_emailSettings.Name, _emailSettings.Sender));

            // subject
            message.Subject = props.Subject;

            //to address
            message.To.Add(new MailboxAddress(props.ReplayTo, props.MailTo));

            //body
            message.Body = new TextPart("html")
            {
                Text = props.Body,
            };

            using(var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, true);

                await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }

    }
}
