using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class GroupTaskItem : EntityData
    {
        public string GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public string TaskItemId { get; set; }
        [ForeignKey("TaskItemId")]
        public virtual TaskItem TaskItem { get; set; }
    }
}
