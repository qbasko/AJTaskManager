using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    public class ExternalUser
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "ExternalUserId")]
        public string ExternalUserId { get; set; }

        //[JsonProperty(PropertyName = "user")]
        ////[JsonConverter(typeof(UserJsonConverter))]
        //public User User { get; set; }

        //[JsonProperty(PropertyName = "userdomain")]
        //public UserDomain UserDomain { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "UserDomainId")]
        public string UserDomainId { get; set; }
    }
}
