using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SchoolApi.DAL;
using SchoolApi.Models;
using SchoolApi.Models.User;

namespace SchoolApi.Services;

public class EmailService
{
    // The email and password use to send email to the user
    private readonly string _host;
    private readonly int _port;
    private readonly string _email;
    private readonly string _password;
    
    private readonly SchoolApiContext _context;
    
    public EmailService(SchoolApiContext context, IConfiguration configuration)
    {
        _context = context;
        _host = configuration.GetSection("AppSettings:Email:Host").Value ?? throw new ArgumentNullException(nameof(_host));
        _port = int.Parse(configuration.GetSection("AppSettings:Email:Port").Value ?? throw new ArgumentNullException(nameof(_port)));
        _email = configuration.GetSection("AppSettings:Email:Address").Value ?? throw new ArgumentNullException(nameof(_email));
        _password = configuration.GetSection("AppSettings:Email:Password").Value ?? throw new ArgumentNullException(nameof(_password));
    }
    
    /// <summary>
    /// Send an email to the user
    /// </summary>
    /// <param name="addressFrom">The source address of the email</param>
    /// <param name="password">Password of the account of addressFrom</param>
    /// <param name="addressTo">The email to send the email</param>
    /// <param name="subject">The subject</param>
    /// <param name="body">The content</param>
    private void SendEmail(string addressFrom, string password, string addressTo, string subject, TextPart body)
    {
        MimeMessage email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(addressFrom));
        email.To.Add(MailboxAddress.Parse(addressTo));
        email.Subject = subject;
        email.Body = body;
        
        
        using SmtpClient smtp = new SmtpClient();
        
        // Need to check those info and change them
        smtp.Connect(_host, 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(addressFrom, password);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
    
    /// <summary>
    /// Send an email to the user
    /// </summary>
    /// <param name="addressTo">The email to send the email</param>
    /// <param name="subject">The subject</param>
    /// <param name="body">The content</param>
    public void SendEmail(string addressTo, string subject, TextPart body)
    {
        SendEmail(_email, _password, addressTo, subject, body);
    }
    
    /// <summary>
    /// Send an email to the user
    /// </summary>
    /// <param name="userDistId">The id of the user you want to send the email</param>
    /// <param name="subject">The subject</param>
    /// <param name="body">The content</param>
    public void SendEmail(int userDistId, string subject, TextPart body)
    {
        // Get the email of the user
        User? user = _context.Users.FirstOrDefault(u => u.Id == userDistId);
        if (user == null)
            return;
        
        SendEmail(_email, _password, user.Email, subject, body);
    }
}