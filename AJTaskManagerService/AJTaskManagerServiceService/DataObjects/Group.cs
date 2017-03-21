using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Mobile.Service;

namespace AJTaskManagerServiceService.DataObjects
{
    public class Group : EntityData
    {
        public string GroupName { get; set; }
    }
}
