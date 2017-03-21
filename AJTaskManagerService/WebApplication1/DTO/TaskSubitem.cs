using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class TaskSubitem
    {
        public string Id { get; set; }
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "ExecutorId")]
        public string ExecutorId { get; set; }
        [JsonProperty(PropertyName = "TaskItemId")]
        public string TaskItemId { get; set; }
        [JsonProperty(PropertyName = "TaskStatusId")]
        public string TaskStatusId { get; set; }
        [JsonProperty(PropertyName = "StartDateTime")]
        public DateTime? StartDateTime { get; set; }
        [JsonProperty(PropertyName = "EndDateTime")]
        public DateTime? EndDateTime { get; set; }
        [JsonProperty(PropertyName = "EstimationInHours")]
        public double EstimationInHours { get; set; }
        [JsonProperty(PropertyName = "IsDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty(PropertyName = "PredecessorId")]
        public string PredecessorId { get; set; }
        [JsonProperty(PropertyName = "SuccessorId")]
        public string SuccessorId { get; set; }
        [JsonProperty(PropertyName = "IsCompleted")]
        public bool IsCompleted { get; set; }
    }
}
