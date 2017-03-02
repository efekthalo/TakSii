using Newtonsoft.Json;
using System.Collections.Generic;

namespace SiiTaxi.Providers
{
    public class ReCaptcha
    {
        public static string Validate(string encodedResponse)
        {
            var client = new System.Net.WebClient();

            const string privateKey = "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe";

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