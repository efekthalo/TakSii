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
        public Emailer(string from, string to, string body, string subject, string cc = null)
        {
            From = new MailAddress(from);
            To = new MailAddress(to);
            if (cc != null)
            {
                CC = new MailAddress(cc);
            }
            Body = body;
            Subject = subject;
        }

        public MailAddress From { get; set; }
        public MailAddress To { get; set; }
        public MailAddress CC { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }

        public void SendEmail()
        {
            var client = new SmtpClient();
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("taksii.test@gmail.com", "testowehaslo");
            client.Host = "smtp.gmail.com";

            var mail = new MailMessage(From, To);
            mail.Subject = Subject;
            mail.IsBodyHtml = true;
            if (CC != null)
            {
                mail.CC.Add(CC);
            }
            mail.Body = Body;
            client.Send(mail);
        }
    }

    public partial class ConfirmTemplate
    {
        public string ConfirmationString { get; set; }
        public int TaxiId { get; internal set; }
    }

    public partial class ConfirmJoinTemplate
    {
        public string ConfirmationString { get; set; }
        public int Id { get; internal set; }
    }

    public partial class SendCodeTemplate
    {
        public string TaxiCodeString { get; set; }
        public string TaxiFrom { get; set; }
        public string TaxiTo { get; set; }
        public string TaxiTime { get; set; }
    }

    public partial class SendCodeAndOrderedTemplate
    {
        public string TaxiCodeString { get; set; }
        public string TaxiFrom { get; set; }
        public string TaxiTo { get; set; }
        public string TaxiTime { get; set; }
    }
}