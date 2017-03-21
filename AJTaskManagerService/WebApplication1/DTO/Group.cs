using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class Group
    {

        public string Id { get; set; }
        [JsonProperty(PropertyName = "GroupName")]
        public string GroupName { get; set; }

        public string GroupNameTruncated
        {
            get { return GroupName.Split(':')[0]; }
        }
    }
}