using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class UserModel
    {
        [Required]
        //[DataType(DataType.Password)]
        [Display(Name = "Name")]
        public string UserName { get; set; }

        [Required]
        //[DataType(DataType.Password)]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}