using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;

namespace GeekWear.Models.Contact
{
    public class Email
    {
        public void Send(IEnumerable<string> to, string subject, string body, string[] files = null)
        {
            SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            MailMessage mailMessage = new MailMessage();
            foreach (var emailAddress in to)
            {
                mailMessage.To.Add(emailAddress);
            }
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            mailMessage.From = new MailAddress(settings.Network.UserName);
            mailMessage.Body = body;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = settings.Network.Host;
            smtp.Port = settings.Network.Port;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(settings.Network.UserName, settings.Network.Password);
            smtp.EnableSsl = smtp.Port != 25;
            smtp.Send(mailMessage);
        }
    }
}
