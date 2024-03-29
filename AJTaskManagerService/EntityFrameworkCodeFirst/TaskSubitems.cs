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
    
    public partial class TaskSubitems
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskSubitems()
        {
            this.TaskSubitemWorks = new HashSet<TaskSubitemWorks>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TaskItemId { get; set; }
        public string TaskStatusId { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public double EstimationInHours { get; set; }
        public bool IsDeleted { get; set; }
        public string PredecessorId { get; set; }
        public string SuccessorId { get; set; }
        public byte[] Version { get; set; }
        public System.DateTimeOffset CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedAt { get; set; }
        public bool Deleted { get; set; }
        public bool IsCompleted { get; set; }
        public string ExecutorId { get; set; }
    
        public virtual TaskItems TaskItems { get; set; }
        public virtual TaskStatus TaskStatus { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskSubitemWorks> TaskSubitemWorks { get; set; }
    }
}
