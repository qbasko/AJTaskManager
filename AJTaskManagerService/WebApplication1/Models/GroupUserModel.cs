using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class GroupUserModel
    {
        public string UserGroupId { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        [Display(Name = "User's Email")]
        public string UserEmail { get; set; }
        public string RoleTypeId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
