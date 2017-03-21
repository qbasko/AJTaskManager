using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AJTaskManagerMobile.Model
{
    public class MicrosoftAccountUserEmails
    {
        [JsonProperty("preferred")]
        public string Preferred { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("personal")]
        public string Personal { get; set; }
        [JsonProperty("business")]
        public string Business { get; set; }
    }
}
