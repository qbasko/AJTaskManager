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
    
    public partial class TaskItems
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskItems()
        {
            this.TaskSubitems = new HashSet<TaskSubitems>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskStatusId { get; set; }
        public string GroupId { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCompleted { get; set; }
        public byte[] Version { get; set; }
        public System.DateTimeOffset CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    
        public virtual Groups Groups { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskSubitems> TaskSubitems { get; set; }
    }
}
