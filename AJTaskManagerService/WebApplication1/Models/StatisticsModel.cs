using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class StatisticsModel
    {
        [Display (Name="GroupName")]
        public string GroupName { get; set; }

        public string GroupId { get; set; }
    }
}