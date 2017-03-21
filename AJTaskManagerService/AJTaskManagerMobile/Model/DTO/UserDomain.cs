using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    [JsonObject(Title = "userdomain")]
    public class UserDomain
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "UserDomainName")]
        public string UserDomainName { get; set; }
        [JsonProperty(PropertyName = "DomainKey")]
        public int DomainKey { get; set; }
    }
}
