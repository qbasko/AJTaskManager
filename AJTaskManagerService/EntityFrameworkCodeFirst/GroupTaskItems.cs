//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityFrameworkCodeFirst
{
    using System;
    using System.Collections.Generic;
    
    public partial class GroupTaskItems
    {
        public string Id { get; set; }
        public byte[] Version { get; set; }
        public System.DateTimeOffset CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public string Group_Id { get; set; }
        public string TaskItem_Id { get; set; }
    
        public virtual Groups Groups { get; set; }
        public virtual TaskItems TaskItems { get; set; }
    }
}
