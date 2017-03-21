using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    public class TodoList
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
