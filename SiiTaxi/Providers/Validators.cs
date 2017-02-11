using Newtonsoft.Json;
using System.Collections.Generic;

namespace SiiTaxi.Providers
{
    public class Validators
    {
        public static bool IsEmailValid(string email, bool company = false)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && (company? email.EndsWith("@pl.sii.eu") : true);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsCaptchaValid(string response)
        {
            return (ReCaptcha.Validate(response) == "True" ? true : false);
        }
    }
}