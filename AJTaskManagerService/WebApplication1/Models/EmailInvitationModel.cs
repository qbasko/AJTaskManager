using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class EmailInvitationModel
    {
        [Required, Display(Name = "Recipient"), EmailAddress]
        public string ToEmail { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
