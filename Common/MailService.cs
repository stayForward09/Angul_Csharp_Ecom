using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace StackApi.Common;

public class MailService : IMailService
{
    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment hostEnvironment;
    public MailService(IConfiguration _con, IWebHostEnvironment environment)
    {
        configuration = _con;
        hostEnvironment = environment;
    }
    public async Task<string> CreateContent(string name,string OTP)
    {
        hostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        string filename = hostEnvironment.WebRootPath + "/Design/OTP.html";
        var _content = await System.IO.File.ReadAllTextAsync(filename);
        _content = _content.Replace("[name]", name);
        _content = _content.Replace("[OTP]", OTP);
        return _content;
    }

    public string GenerateOTP()
    {
        Random generator = new Random();
        string r = generator.Next(0, 1000000).ToString("D6");
        return r;
    }

    public async Task<bool> SendEmail(string tomailid, string subject, string content)
    {
        try
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(configuration["SmtpMail"]); // Added to secrets
            email.To.Add(MailboxAddress.Parse(tomailid));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = content;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(configuration["SmtpHost"], 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration["SmtpMail"], configuration["SmtpPass"]); // Added to secrets
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return true;
        }
        catch
        {
            return false;
        }
    }
}