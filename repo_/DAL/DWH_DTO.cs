using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Repo.DAL
{
    public partial class T_ACQ_D : IChainable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }    
    public partial class T_CTL_D 
    {
        [Key]
        public int ID { get; set; }
    }
    public partial class T_ECOMM_D : IEntity
    {
        [Key]
        public int ID { get; set; }
    }
    public partial class T_ECOMM_M 
    {
        [Key]
        public int ID { get; set; }
    }   

    public partial class TEMP_ACQ_D : DenormalEntity
    {
        [Key]
        public int ID { get; set; }
    }
    public partial class TEMP_ACQ_M : IEntity
    {
        [Key]
        public int ID { get; set; }
    }
    public partial class TEMP_CTL_D : IEntity
    {
        [Key]
        public int ID { get; set; }
    }
    public partial class TEMP_ECOMM_D : IEntity
    {
        [Key]
        public int ID { get; set; }
    }    
    public partial class TEMP_ACQ : IEntity
    {
        [Key]
        public int ID { get; set; }
    }

    public partial class REFMERCHANTS : IEntity, IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ITEM_ID { get; set; }
        public int USER_ID { get; set; }
    }

    public partial class KEY_CLIENTS : IEntity
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

    public partial class T_ACQ_D : IEntity, IChainable
    {
        public Nullable<System.DateTime> DATE { get; set; }
        [Required]
        public long? MERCHANT { get; set; }
    }
    public partial class T_ACQ_M : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
    public partial class KEY_CLIENTS : IEntity, IMerchant, ISector
    {
        [Required]
        public long? MERCHANT { get; set; }
    }
    public partial class TEMP_ACQ_D : IDate, IChainable
    {
        public new long? MERCHANT { get; set; }
    }
    public partial class REFMERCHANTS : IMerchant
    {
        [Required]
        public long? MERCHANT { get; set; }
    }
    public partial class TEMP_ACQ_M : IDate
    {
        public Nullable<System.DateTime> DATE { get; set; }
        public string MERCHANT { get; set; }
        public Nullable<decimal> CNT { get; set; }
        public Nullable<decimal> RN { get; set; }
    }

    // for running stored procedures disconnected from current POCO
    public class FakePOCO
    {

    }
}
