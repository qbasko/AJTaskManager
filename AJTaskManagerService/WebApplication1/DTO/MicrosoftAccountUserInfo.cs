using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class MicrosoftAccountUserInfoJson
    {
        [JsonProperty("microsoft")]
        public MicrosoftAccountUserInfo MicrosoftAccountUserInfo { get; set; }
    }

    public class MicrosoftAccountUserInfo
    {
        [JsonProperty("emails")]
        public MicrosoftAccountUserEmails Emails { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("updated_time")]
        public string UpdatedTime { get; set; }
    }
}
