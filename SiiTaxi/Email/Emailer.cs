using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace SiiTaxi.Email
{
    public class Emailer
    {
        public Emailer(string from, string to, string body)
        {
            From = new MailAddress(from);
            To = new MailAddress(to);
            Body = body;
        }

        public MailAddress From { get; set; }
        public MailAddress To { get; set; }
        
        public string Body { get; set; }

        public void SendEmail()
        {
            var client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("adam.guja@gmail.com", "ftflmurmguwfvpas");
            client.Host = "smtp.gmail.com";

            var mail = new MailMessage(From, To);
            mail.IsBodyHtml = true;
            mail.Body = Body;
            client.Send(mail);
        }        
    }

    public partial class ConfirmTemplate
    {
        public string ConfirmationString { get; set; }
    }
}