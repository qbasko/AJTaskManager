using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class ToDoList
    {

        public string Id { get; set; }
        [JsonProperty(PropertyName = "ListName")]
        public string ListName { get; set; }

        [JsonProperty(PropertyName = "IsCompleted")]
        public bool IsCompleted { get; set; }

        [JsonProperty(PropertyName = "IsDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty(PropertyName = "GroupId")]
        public string GroupId { get; set; }
    }
}