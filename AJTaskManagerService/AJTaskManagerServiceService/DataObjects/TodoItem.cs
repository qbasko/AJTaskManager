using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool IsCompleted { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime Deadline { get; set; }

        public string TodoListId { get; set; }
        [ForeignKey("TodoListId")]
        public virtual TodoList TodoList { get; set; }
    }
}