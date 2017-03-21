using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class UserGroup
    {
        [JsonProperty (PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "GroupId")]
        public string  GroupId { get; set; }

        [JsonProperty(PropertyName = "RoleTypeId")]
        public string RoleTypeId { get; set; }

        [JsonProperty(PropertyName = "IsUserDefaultGroup")]
        public bool IsUserDefaultGroup { get; set; }
    }
}