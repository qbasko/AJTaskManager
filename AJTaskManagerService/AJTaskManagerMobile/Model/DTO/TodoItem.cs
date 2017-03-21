using System;
using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    public class TodoItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "IsCompleted")]
        public bool IsCompleted { get; set; }

        [JsonProperty(PropertyName = "IsDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty(PropertyName = "CreatedDateTime")]
        public DateTime CreatedDateTime { get; set; }

        [JsonProperty(PropertyName = "Deadline")]
        public DateTime Deadline { get; set; }

        [JsonProperty(PropertyName = "TodoListId")]
        public string TodoListId { get; set; }
    }
}
