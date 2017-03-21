using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class CalendarTaskSubitem : EntityData
    {
        public string GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public string CalendarId { get; set; }
        [ForeignKey("CalendarId")]
        public virtual Calendar Calendar { get; set; }

        public string TaskSubitemId { get; set; }
        [ForeignKey("TaskSubitemId")]
        public virtual TaskSubitem TaskSubitem { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
