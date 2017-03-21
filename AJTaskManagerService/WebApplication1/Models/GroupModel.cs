using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class GroupModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
    }
}
