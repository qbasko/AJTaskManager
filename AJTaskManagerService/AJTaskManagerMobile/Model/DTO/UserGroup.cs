using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    public class UserGroup
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
         [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }
         [JsonProperty(PropertyName = "GroupId")]
        public string GroupId { get; set; }
         [JsonProperty(PropertyName = "RoleTypeId")]
        public string RoleTypeId { get; set; }

         [JsonProperty(PropertyName = "IsUserDefaultGroup")]        
         public bool IsUserDefaultGroup { get; set; }

    }
}
