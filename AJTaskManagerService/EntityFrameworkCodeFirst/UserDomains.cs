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
    
    public partial class UserDomains
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserDomains()
        {
            this.ExternalUsers = new HashSet<ExternalUsers>();
        }
    
        public string Id { get; set; }
        public int DomainKey { get; set; }
        public string UserDomainName { get; set; }
        public byte[] Version { get; set; }
        public System.DateTimeOffset CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalUsers> ExternalUsers { get; set; }
    }
}
