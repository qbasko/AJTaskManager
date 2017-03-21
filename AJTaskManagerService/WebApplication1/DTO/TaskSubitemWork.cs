using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApplication1.DTO
{
    public class TaskSubitemWork
    {
        public string Id { get; set; }
        [JsonProperty("Comment")]
        public string Comment { get; set; }
        [JsonProperty("TaskSubitemId")]
        public string TaskSubitemId { get; set; }
        [JsonProperty("UserId")]
        public string UserId { get; set; }
        [JsonProperty("StartDateTime")]
        public DateTime StartDateTime { get; set; }
        [JsonProperty("EndDateTime")]
        public DateTime? EndDateTime { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public string Entry
        {
            get
            {
                return String.Format("{0} - {1} | {2}", StartDateTime,
                    EndDateTime.HasValue ? EndDateTime.Value.ToString() : "",
                    User != null ? User.FullName : "");
            }
        }
    }
}
