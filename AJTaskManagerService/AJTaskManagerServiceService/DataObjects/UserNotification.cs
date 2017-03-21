using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class UserNotification : EntityData
    {
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string TaskSubitemId { get; set; }
        [ForeignKey("TaskSubitemId")]
        public TaskSubitem TaskSubitem { get; set; }
    }
}
