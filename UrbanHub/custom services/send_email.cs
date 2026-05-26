using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace UrbanHub.custom_services;

public class  send_email
{
    private readonly string _from = "almubin9516@gmail.com";
    private readonly string _password = "lttx pttk qrva dwtg";

    public async Task SendEmail(string to, string subject, string body) { 

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("UrbanHub", _from));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_from, _password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}