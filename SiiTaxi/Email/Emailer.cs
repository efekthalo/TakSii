using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using SiiTaxi.Models;

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

        public bool SendEmail()
        {
            try
            {
                var client = new SmtpClient
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("taksii.test@gmail.com", "testowehaslo"),
                    Host = "smtp.gmail.com"
                };

                var mail = new MailMessage(From, To)
                {
                    Subject = Subject,
                    IsBodyHtml = true,
                    Body = Body
                };

                if (CC != null)
                {
                    mail.CC.Add(CC);
                }

                client.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public partial class ConfirmTemplate
    {
        public Taxi Taxi { get; set; }
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

    public partial class SendNotificationTemplate
    {
        public string TaxiCodeString { get; set; }
        public string TaxiFrom { get; set; }
        public string TaxiTo { get; set; }
        public string TaxiTime { get; set; }
    }

    public partial class SendRemoveToOwnerTemplate
    {
        public string TaxiCodeString { get; set; }
        public string TaxiFrom { get; set; }
        public string TaxiTo { get; set; }
        public string TaxiTime { get; set; }
        public TaxiPeople Joiner { get; set; }
    }

    public partial class SendRemoveToJoinersTemplate
    {
        public string TaxiCodeString { get; set; }
        public string TaxiFrom { get; set; }
        public string TaxiTo { get; set; }
        public string TaxiTime { get; set; }
    }
}