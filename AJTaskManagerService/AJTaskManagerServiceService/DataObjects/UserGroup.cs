using Microsoft.WindowsAzure.Mobile.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJTaskManagerServiceService.DataObjects
{
    public class UserGroup : EntityData
    {
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public string RoleTypeId { get; set; }
        [ForeignKey("RoleTypeId")]
        public virtual RoleType Role { get; set; }

        public bool IsUserDefaultGroup { get; set; }

    }
}
