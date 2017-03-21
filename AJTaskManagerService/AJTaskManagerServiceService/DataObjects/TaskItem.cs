using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class TaskItem : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskStatusId { get; set; }
        [ForeignKey("TaskStatusId")]
        public virtual TaskStatus TaskStatus { get; set; }

        public string GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsCompleted { get; set; }
    }
}
