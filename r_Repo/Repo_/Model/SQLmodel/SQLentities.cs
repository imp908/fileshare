using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.SQLmodel
{

    #region DWH
    [Table(@"REFMERCHANTS_SQL")]
    public class REFMERCHANTS_SQL : Repo_.IEntityInt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ITEM_ID { get; set; }
        public int USER_ID { get; set; }
        [Required]
        public long MERCHANT { get; set; }
    }
    [DataContract]
    public partial class KEY_CLIENTS_SQL : Repo_.IEntityInt
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
        [DataMember]
        public long MERCHANT { get; set; }
        public string LEGAL_ENTITY { get; set; }
        public string PROVIDER_NAME { get; set; }
        [Required]        
        public int? SECTOR_ID { get; set; }
        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime DATE { get; set; }
        //[ForeignKey("SECTOR_ID")]
        //public SECTOR_NAMES SECTOR_NAMES { get; set; }
    }

    [Table("MERCHANT_LIST")]
    public partial class MERCHANT_LIST_SQL : Repo_.IEntityInt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public long MERCHANT { get; set; }
        [Required]
        public int USER_ID { get; set; }
        [Required]
        public DateTime UPDATE_DATE { get; set; }
    }

    [Table("USERS")]
    public partial class USERS_SQL : Repo_.IEntityInt
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Sername { get; set; }
        [Required]
        public string mail { get; set; }

    }


    [Table("SECTORS")]
    public partial class SECTOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string SECTOR_NAME { get; set; }

        public virtual ICollection<SECTOR_MASK> SECTOR_MASKS { get; set; }
    }

    [Table("SECTORMASKS")]
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

    [Table("T_ACQ_M_SQL")]
    public partial class T_ACQ_M_SQL : Repo_.IEntityInt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }    
        public Nullable<System.DateTime> DATE { get; set; }
        public string PAY_SYS { get; set; }
        public string ISS_TYPE { get; set; }
        public string ACQ_BANK { get; set; }
        public string TRAN_TYPE { get; set; }
        public string RC { get; set; }
        public long MERCHANT { get; set; }
        public string MAIN_INDUSTRY { get; set; }
        public Nullable<decimal> FEE { get; set; }
        public Nullable<decimal> AMT { get; set; }
        public Nullable<decimal> CNT { get; set; }
    }

    [Table("T_ACQ_D_SQL")]
    public partial class T_ACQ_D_SQL :  Repo_.IEntityInt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        [Required]
        public long MERCHANT { get; set; }
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
    #endregion
  
}

