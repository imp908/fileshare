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
    
    public partial class DEPARTMENTS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEPARTMENTS()
        {
            this.EMPLOYEES2 = new HashSet<EMPLOYEES>();
            this.EMPLOYEES3 = new HashSet<EMPLOYEES>();
            this.JOB_HISTORY = new HashSet<JOB_HISTORY>();
        }
    
        public short DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public Nullable<int> MANAGER_ID { get; set; }
        public Nullable<short> LOCATION_ID { get; set; }
        public Nullable<int> EMPLOYEES_EMPLOYEE_ID { get; set; }
        public Nullable<int> EMPLOYEES_EMPLOYEE_ID1 { get; set; }
    
        public virtual EMPLOYEES EMPLOYEES { get; set; }
        public virtual EMPLOYEES EMPLOYEES1 { get; set; }
        public virtual LOCATIONS LOCATIONS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLOYEES> EMPLOYEES2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLOYEES> EMPLOYEES3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JOB_HISTORY> JOB_HISTORY { get; set; }
    }
}