using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class RoleType
    {
        [JsonProperty (PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty (PropertyName = "RoleName")]
        public string RoleName { get; set; }

        [JsonProperty (PropertyName = "RoleKey")]
        public int RoleKey { get; set; }

       
    }
}