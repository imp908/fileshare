//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SB.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class JOBS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JOBS()
        {
            this.EMPLOYEES = new HashSet<EMPLOYEES>();
            this.JOB_HISTORY = new HashSet<JOB_HISTORY>();
        }
    
        public string JOB_ID { get; set; }
        public string JOB_TITLE { get; set; }
        public Nullable<int> MIN_SALARY { get; set; }
        public Nullable<int> MAX_SALARY { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLOYEES> EMPLOYEES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JOB_HISTORY> JOB_HISTORY { get; set; }
    }
}
