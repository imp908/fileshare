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
    
    public partial class LOCATIONS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOCATIONS()
        {
            this.DEPARTMENTS = new HashSet<DEPARTMENTS>();
        }
    
        public short LOCATION_ID { get; set; }
        public string STREET_ADDRESS { get; set; }
        public string POSTAL_CODE { get; set; }
        public string CITY { get; set; }
        public string STATE_PROVINCE { get; set; }
        public string COUNTRY_ID { get; set; }
    
        public virtual COUNTRIES COUNTRIES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEPARTMENTS> DEPARTMENTS { get; set; }
    }
}
