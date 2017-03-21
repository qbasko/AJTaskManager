using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class User : EntityData
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
