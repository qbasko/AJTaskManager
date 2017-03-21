using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class MicrosoftAccountUserEmails
    {
        [JsonProperty("preferred")]
        public string Preferred { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("personal")]
        public string Personal { get; set; }
        [JsonProperty("business")]
        public string Business { get; set; }
    }
}
