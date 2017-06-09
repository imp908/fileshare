using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Repo.DAL
{

    [Table("T_ACQ_D")]
    public partial class T_ACQ_D_SQL : IMerchant, IChainable
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

    [Table("T_ACQ_M")]
    public partial class T_ACQ_M_SQL : IDate, IEntity
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
        public long? MERCHANT { get; set; }
        public string MAIN_INDUSTRY { get; set; }
        public Nullable<decimal> FEE { get; set; }
        public Nullable<decimal> AMT { get; set; }
        public Nullable<decimal> CNT { get; set; }
    }

    [Table("T_CTL_D")]
    public partial class T_CTL_D_SQL
    {
        [Key]
        public int ID { get; set; }
    }

    [Table("T_ECOMM_D")]
    public partial class T_ECOMM_D_SQL : IEntity, IDate
    {
        [Key]
        public int ID { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
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

    [Table("T_ECOMM_M")]
    public partial class T_ECOMM_M_SQL
    {
        [Key]
        public int ID { get; set; }
    }

    [Table("TEMP_ACQ_D")]
    public partial class TEMP_ACQ_D_SQL :  IDate, IChainable
    {
        [Key]
        public int ID { get; set; }
        public new long? MERCHANT { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        public Nullable<decimal> CNT { get; set; }
        public Nullable<decimal> RN { get; set; }
    }

    [Table("TEMP_ACQ_M")]
    public partial class TEMP_ACQ_M_SQL : IEntity, IDate
    {
        [Key]
        public int ID { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        public string MERCHANT { get; set; }
        public Nullable<decimal> CNT { get; set; }
        public Nullable<decimal> RN { get; set; }
    }

    [Table("TEMP_CTL_D")]
    public partial class TEMP_CTL_D_SQL : IEntity
    {
        [Key]
        public int ID { get; set; }
    }

    [Table("TEMP_ECOMM_D")]
    public partial class TEMP_ECOMM_D_SQL : IEntity, IDate
    {
        [Key]
        public int ID { get; set; }
        public Nullable<System.DateTime> DATE { get; set; }
        public string PAY_SYS { get; set; }
        public string ISS_TYPE { get; set; }
        public string ACQ_BANK { get; set; }
        public string TRAN_TYPE { get; set; }
        public string RC { get; set; }
        public string MERCHANT { get; set; }
        public string FULL_NAME { get; set; }
        public string ABRV_NAME { get; set; }
        public string GROUP_NAME { get; set; }
        public string MAIN_INDUSTRY { get; set; }
        public Nullable<decimal> FEE { get; set; }
        public Nullable<decimal> AMT { get; set; }
        public Nullable<decimal> CNT { get; set; }
    }

    [Table("TEMP_ACQ_SQL")]
    public partial class TEMP_ACQ_SQL : IEntity
    {
        [Key]
        public int ID { get; set; }
    }

    [Table("REFMERCHANTS")]
    public partial class REFMERCHANTS_SQL : IEntity, IMerchant, IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ITEM_ID { get; set; }
        public int USER_ID { get; set; }
        [Required]
        public long? MERCHANT { get; set; }
    }

    [Table("KEY_CLIENTS")]
    public partial class KEY_CLIENTS_SQL : IEntity, ISector
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
        public long? MERCHANT { get; set; }
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
    public partial class SECTOR_SQL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string SECTOR_NAME { get; set; }
        
        public virtual ICollection<SECTOR_MASK_SQL> SECTOR_MASKS { get; set; }
    }

    [Table("SectorMasks")]
    public partial class SECTOR_MASK_SQL
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }        
        public string MASK { get; set; }

        [ForeignKey("SECTOR")]
        public int SectorID { get; set; }

        //public int SECTOR_NAMEId { get; set; }
        public virtual SECTOR_SQL SECTOR { get; set; }
    }
   

    // for running stored procedures disconnected from current POCO
    public class FakePOCO_SQL
    {

    }

}
