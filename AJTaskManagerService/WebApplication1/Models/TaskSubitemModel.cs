using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TaskSubitemModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name="Activity Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public string TaskItemId { get; set; }

        [Required]
        [Display(Name = "Activity Status")]
        public string Status { get; set; }

        public string StatusId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDateTime { get; set; }

        [Display(Name="Estimation in hours")]
        public double EstimationInHours { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Predecessor")]
        public string Predecessor { get; set; }

        public string PredecessorId { get; set; }

        [Display(Name = "Successor")]
        public string Successor { get; set; }

        public string SuccessorId { get; set; }

        [Display(Name = "Is completed")]
        public bool IsCompleted { get; set; }

        [Required]
        [Display(Name ="Executor")]
        public string Executor { get; set; }

        public string ExecutorId { get; set; }

    }
}