using System.Net;
using System.Net.Mail;

namespace SiiTaxi.Email
{
    public class Emailer
    {
        public Emailer(string from, string to, string cc, string body)
        {
            From = new MailAddress(from);
            To = new MailAddress(to);
            CC = new MailAddress(cc);
            Body = body;
        }

        public MailAddress From { get; set; }
        public MailAddress To { get; set; }
        public MailAddress CC { get; set; }

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
            mail.IsBodyHtml = true;
            mail.CC.Add(CC);
            mail.Body = Body;
            client.Send(mail);
        }
    }

    public partial class ConfirmTemplate
    {
        public string ConfirmationString { get; set; }
        public int TaxiId { get; internal set; }
    }
}