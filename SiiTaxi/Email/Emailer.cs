using System.Configuration;
using System.Net;
using System.Net.Mail;
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
                Cc = new MailAddress(cc);
            Body = body;
            Subject = subject;
        }

        private MailAddress From { get; }
        private MailAddress To { get; }
        private MailAddress Cc { get; }

        private string Subject { get; }
        private string Body { get; }

        public bool SendEmail()
        {
            try
            {
                var client = new SmtpClient
                {
                    Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]), //587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials =
                        new NetworkCredential(ConfigurationManager.AppSettings["smtpLogin"],
                            ConfigurationManager.AppSettings["smtpPassword"]),
                    Host = ConfigurationManager.AppSettings["smtpHost"]
                };

                var mail = new MailMessage(From, To)
                {
                    Subject = Subject,
                    IsBodyHtml = true,
                    Body = Body
                };

                if (Cc != null)
                    mail.CC.Add(Cc);

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
        public Taxi Taxi { private get; set; }
    }

    public partial class ConfirmJoinTemplate
    {
        public TaxiPeople TaxiPeople { private get; set; }
    }

    public partial class ResourceOnlyTemplate
    {
        public Taxi Taxi { private get; set; }
    }

    public partial class SendCodeTemplate
    {
        public Taxi Taxi { private get; set; }
    }

    public partial class SendCodeAndOrderedTemplate
    {
        public Taxi Taxi { private get; set; }
    }

    public partial class SendNotificationTemplate
    {
        public Taxi Taxi { get; set; }
    }

    public partial class SendRemoveToOwnerTemplate
    {
        public Taxi Taxi { get; set; }
        public TaxiPeople Joiner { get; set; }
    }

    public partial class SendRemoveToJoinersTemplate
    {
        public Taxi Taxi { get; set; }
    }

    public partial class OwnerContactTemplate
    {
        public Taxi Taxi { get; set; }
    }
}