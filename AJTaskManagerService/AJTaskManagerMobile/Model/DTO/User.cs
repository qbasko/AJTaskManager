using System;
using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    [JsonObject(Title = "user")]
    public class User
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "LastName")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }
        [JsonIgnore]
        public bool IsChecked { get; set; }

        [JsonIgnore]
        public string FullName
        {
            get { return String.Format("{0} {1}", UserName, LastName); }
        }
    }
}
