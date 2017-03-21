using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    public class RoleType
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "RoleName")]
        public string RoleName { get; set; }
        [JsonProperty(PropertyName = "RoleKey")]
        public int RoleKey { get; set; }

        [JsonIgnore]
        public bool IsChecked { get; set; }
    }
}
