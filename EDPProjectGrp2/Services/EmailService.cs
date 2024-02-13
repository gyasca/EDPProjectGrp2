using System;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    public void SendPasswordResetEmail(string email, string resetLink)
    {
        // Configure SMTP client
        using (var client = new SmtpClient("smtp.gmail.com"))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("gregoryuplaystaff@gmail.com", "njyk gclt bpfz nnmx");
            client.EnableSsl = true;
            client.Port = 587; // Port for Gmail SMTP

            // Create email message
            var message = new MailMessage();
            message.From = new MailAddress("gregoryuplaystaff@gmail.com");
            message.To.Add(email);
            message.Subject = "Password Reset";
            message.Body = $"Click the following link to reset your password: {resetLink}";

            // Send email
            client.Send(message);
        }
    }
}
