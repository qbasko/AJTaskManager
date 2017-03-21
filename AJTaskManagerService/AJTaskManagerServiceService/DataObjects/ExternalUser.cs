using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class ExternalUser : EntityData
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string UserDomainId { get; set; }
        [ForeignKey("UserDomainId")]
        public virtual UserDomain UserDomain { get; set; }

        public string ExternalUserId { get; set; }
    }
}
