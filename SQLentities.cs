using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Model.SQLmodel
{

    class SQLentities
    {

    }   
    
    //SQL HR
    public partial class REGIONS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public REGIONS()
        {
            this.COUNTRIES = new HashSet<COUNTRIES>();
        }

        [Key]
        public decimal REGION_ID { get; set; }
        public string REGION_NAME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<COUNTRIES> COUNTRIES { get; set; }
    }

    public partial class LOCATIONS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOCATIONS()
        {
            this.DEPARTMENTS = new HashSet<DEPARTMENTS>();
        }

        [Key]
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

    public partial class JOBS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JOBS()
        {
            this.EMPLOYEES = new HashSet<EMPLOYEES>();
            this.JOB_HISTORY = new HashSet<JOB_HISTORY>();
        }

        [Key]
        public string JOB_ID { get; set; }
        public string JOB_TITLE { get; set; }
        public Nullable<int> MIN_SALARY { get; set; }
        public Nullable<int> MAX_SALARY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLOYEES> EMPLOYEES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JOB_HISTORY> JOB_HISTORY { get; set; }
    }

    public partial class JOB_HISTORY
    {
        [Key]
        public int EMPLOYEE_ID { get; set; }
        public System.DateTime START_DATE { get; set; }
        public System.DateTime END_DATE { get; set; }
        public string JOB_ID { get; set; }
        public Nullable<short> DEPARTMENT_ID { get; set; }
        public Nullable<int> EMPLOYEES_EMPLOYEE_ID { get; set; }

        public virtual DEPARTMENTS DEPARTMENTS { get; set; }
        public virtual EMPLOYEES EMPLOYEES { get; set; }
        public virtual JOBS JOBS { get; set; }
    }

    public partial class COUNTRIES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public COUNTRIES()
        {
            this.LOCATIONS = new HashSet<LOCATIONS>();
        }

        [Key]
        public string COUNTRY_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
        public Nullable<decimal> REGION_ID { get; set; }

        public virtual REGIONS REGIONS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LOCATIONS> LOCATIONS { get; set; }
    }

    public partial class DEPARTMENTS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEPARTMENTS()
        {
            this.EMPLOYEES2 = new HashSet<EMPLOYEES>();
            this.EMPLOYEES3 = new HashSet<EMPLOYEES>();
            this.JOB_HISTORY = new HashSet<JOB_HISTORY>();
        }

        [Key]
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

    public partial class EMPLOYEES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYEES()
        {
            this.DEPARTMENTS = new HashSet<DEPARTMENTS>();
            this.DEPARTMENTS1 = new HashSet<DEPARTMENTS>();
            this.EMPLOYEES1 = new HashSet<EMPLOYEES>();
            this.JOB_HISTORY = new HashSet<JOB_HISTORY>();
        }

        [Key]
        public int EMPLOYEE_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE_NUMBER { get; set; }
        public System.DateTime HIRE_DATE { get; set; }
        public string JOB_ID { get; set; }
        public Nullable<decimal> SALARY { get; set; }
        public Nullable<decimal> COMMISSION_PCT { get; set; }
        public Nullable<int> MANAGER_ID { get; set; }
        public Nullable<short> DEPARTMENT_ID { get; set; }
        public Nullable<short> DEPARTMENTS1_DEPARTMENT_ID { get; set; }
        public Nullable<int> EMPLOYEES2_EMPLOYEE_ID { get; set; }
        public Nullable<short> DEPARTMENTS_DEPARTMENT_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEPARTMENTS> DEPARTMENTS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEPARTMENTS> DEPARTMENTS1 { get; set; }
        public virtual DEPARTMENTS DEPARTMENTS2 { get; set; }
        public virtual DEPARTMENTS DEPARTMENTS3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMPLOYEES> EMPLOYEES1 { get; set; }
        public virtual EMPLOYEES EMPLOYEES2 { get; set; }
        public virtual JOBS JOBS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JOB_HISTORY> JOB_HISTORY { get; set; }
    }


    //SQL DWH
    [Table(@"REFMERCHANTS")]
    public class REFMERCHANTS_SQL : Repo_.IEntity, Repo_.IUser, Repo_.IMerchant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ITEM_ID { get; set; }
        public int USER_ID { get; set; }
        [Required]
        public long? MERCHANT { get; set; }
    }
    public partial class KEY_CLIENTS_SQL : Repo_.IEntity
    {
        //[Key, Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string INDUSTRY_FILE { get; set; }
        public string RESPONSIBILITY_GROUP { get; set; }
        public string RESPONSIBILITY_MANAGER { get; set; }
        public string GROUP_NAME { get; set; }
        public string INDUSTRY { get; set; }
        public string INDUSTRY_SECONDARY { get; set; }
        public string SE_NAME { get; set; }
        public string PHYSICAL_ADDRESS { get; set; }
        public string CITY { get; set; }
        [Required]
        public long? SE_NUMBER { get; set; }
        public string LEGAL_ENTITY { get; set; }
        public string PROVIDER_NAME { get; set; }
        public int? SECTOR_ID { get; set; }
        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime DATE { get; set; }
        //[ForeignKey("SECTOR_ID")]
        //public SECTOR_NAMES SECTOR_NAMES { get; set; }
    }

    [Table("Sectors")]
    public partial class SECTOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string SECTOR_NAME { get; set; }

        public virtual ICollection<SECTOR_MASK> SECTOR_MASKS { get; set; }
    }

    [Table("SectorMasks")]
    public partial class SECTOR_MASK
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MASK { get; set; }

        [ForeignKey("SECTOR")]
        public int SectorID { get; set; }

        //public int SECTOR_NAMEId { get; set; }
        public virtual SECTOR SECTOR { get; set; }
    }

    public partial class KEY_CLIENTS_SQL : Repo_.IEntity, Repo_.IMerchant, Repo_.ISector
    {
        [Required]
        public long? MERCHANT { get; set; }
    }


    public partial class T_ACQ_M_SQL : Repo_.IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }

    public partial class T_ACQ_M_SQL
    {
        public Nullable<System.DateTime> DATE { get; set; }
        public string PAY_SYS { get; set; }
        public string ISS_TYPE { get; set; }
        public string ACQ_BANK { get; set; }
        public string TRAN_TYPE { get; set; }
        public string RC { get; set; }
        public string MERCHANT { get; set; }
        public string MAIN_INDUSTRY { get; set; }
        public Nullable<decimal> FEE { get; set; }
        public Nullable<decimal> AMT { get; set; }
        public Nullable<decimal> CNT { get; set; }
    }

    [Table("T_ACQ_D")]
    public partial class T_ACQ_D_SQL : Repo_.IDate, Repo_.IEntity, Repo_.IMerchant, Repo_.IChainable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        [Required]
        public long? MERCHANT { get; set; }
        public string PAY_SYS { get; set; }
        public string ISS_TYPE { get; set; }
        public string ACQ_BANK { get; set; }
        public string TRAN_TYPE { get; set; }
        public string RC { get; set; }
        public string MAIN_INDUSTRY { get; set; }
        public Nullable<decimal> COR_FEE { get; set; }
        public Nullable<decimal> AMT { get; set; }
        public Nullable<decimal> FEE { get; set; }
        public Nullable<decimal> CNT { get; set; }
    }

    public partial class FakePOCO
    {

    }

}
