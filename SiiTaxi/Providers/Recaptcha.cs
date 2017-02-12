using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace SiiTaxi.Providers
{
    public class ReCaptcha
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }

        public static string Validate(string EncodedResponse)
        {
            var client = new WebClient();

            var PrivateKey = "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe";

            var GoogleReply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey,
                        EncodedResponse));

            var captchaResponse = JsonConvert.DeserializeObject<ReCaptcha>(GoogleReply);

            return captchaResponse.Success;
        }
    }
}