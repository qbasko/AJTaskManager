using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    public class TaskStatus
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }
    }
}
