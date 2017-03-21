using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class AddTodoListModel
    {
        [Required]
        [Display(Name = "Task Name")]
        public string ListName { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        public string GroupId { get; set; }

        public string ListId { get; set; }

        [Display(Name="Is Completed")]
        public bool IsCompleted { get; set; }

   
    }
}