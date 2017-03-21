using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AddTodoItemModel
    {
        [Required]
        [Display(Name = "Subtask Name")]
        public string Text { get; set; }

        public string Id { get; set; }

        [Display(Name="Is completed")]
        public bool IsCompleted { get; set; }

        public string TodoListId { get; set; }

        //public string TodoListName { get; set; }

        
    }
}