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
    
    public partial class UserNotifications
    {
        public string Id { get; set; }
        public System.DateTime Date { get; set; }
        public byte[] Version { get; set; }
        public System.DateTimeOffset CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public string TaskSubitem_Id { get; set; }
        public string User_Id { get; set; }
    
        public virtual TaskSubitems TaskSubitems { get; set; }
        public virtual Users Users { get; set; }
    }
}
