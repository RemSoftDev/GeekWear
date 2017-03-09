using GeekWear.Models;
using GeekWear.Models.Contact;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Http;

namespace GeekWear.Controllers
{
    public class ContactController : ApiController
    {
        // POST: api/Contact
        public void Post(ContactFormMessage message)
        {
            string body = File.ReadAllText(HostingEnvironment.MapPath("~/Content/EmailTemplates/MessageFromSite.html"));
            body = body.Replace("{{messageContent}}", message.Message)
                       .Replace("{{fromEmail}}", message.Email)
                       .Replace("{{fromName}}", message.Name);
            var to = new List<string> { WebConfigurationManager.AppSettings["contactFormToEmail"] };
            new Email().Send(to, "GeekWear.London contact form", body);
        }
    }
}
