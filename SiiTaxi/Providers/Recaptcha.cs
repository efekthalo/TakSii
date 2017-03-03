using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;

namespace SiiTaxi.Providers
{
    public class ReCaptcha
    {
        public static string Validate(string encodedResponse)
        {
            var client = new System.Net.WebClient();

            var privateKey = ConfigurationManager.AppSettings["recaptchaServer"];

            var googleReply = client.DownloadString(
                $"https://www.google.com/recaptcha/api/siteverify?secret={privateKey}&response={encodedResponse}");

            var captchaResponse = JsonConvert.DeserializeObject<ReCaptcha>(googleReply);

            return captchaResponse.Success;
        }

        [JsonProperty("success")]
        private string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}