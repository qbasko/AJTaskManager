using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class TaskSubitem : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string ExecutorId { get; set; }
        [ForeignKey("ExecutorId")]
        public User Executor { get; set; }

        public string TaskItemId { get; set; }
        [ForeignKey("TaskItemId")]
        public TaskItem TaskItem { get; set; }

        public string TaskStatusId { get; set; }
        [ForeignKey("TaskStatusId")]
        public virtual TaskStatus TaskStatus { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public double EstimationInHours { get; set; }

        public bool IsDeleted { get; set; }

        public string PredecessorId { get; set; }
        //[ForeignKey("TaskPredecessorId")]
        //public virtual TaskSubitem Predecessor { get; set; }

        public string SuccessorId { get; set; }
        //[ForeignKey("SucessorId")]
        //public virtual TaskSubitem Successor { get; set; }

        public bool IsCompleted { get; set; }

    }
}
