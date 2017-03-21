using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model.DTO
{
    public class Group
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "GroupName")]
        public string GroupName { get; set; }

        [JsonIgnore]
        public bool IsChecked { get; set; }

        public string GroupNameTruncated
        {
            get
            {
                int index = GroupName.IndexOf(":", StringComparison.Ordinal);
                if (index != -1)
                    return GroupName.Substring(0, index);
                return GroupName;
            }
        }
    }
}
