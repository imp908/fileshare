using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TransactionSite_.Models
{
    public class REFMERCHANT
    {               
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MERCHANT { get; set; }
        public string parent { get; set; }
        public string ABRV_NAME { get; set; }
        public string FULL_NAME { get; set; }
        public int MCC { get; set; }
        public string RC { get; set; }
        public string STREET { get; set; }
        public string city { get; set; }
        public long reg_NR { get; set; }
        public int region { get; set; }
    }
    public class FD_ACQ_D
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }       
        public DateTime DT_REG { get; set; }
        public string PAY_SYS { get; set; }
        public string ISSUER_TYPE { get; set; }
        public string ACQUIRE_BANK { get; set; }
        public string TYPE_TRANSACTION { get; set; }
        public string MERCHANT { get; set; }
       
        public double? FEE { get; set; }
        public double? AMT { get; set; }
        public int? CNT { get; set; }
    }

    public class T_ACQ_D
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; } 
        public DateTime DT_REG { get; set; }
        public string PAY_SYS { get; set; }
        public string ISSUER_TYPE { get; set; }
        public string ACQUIRE_BANK { get; set; }
        public string TYPE_TRANSACTION { get; set; }
        public string MERCHANT { get; set; }
        public double FEE { get; set; }
        public double AMT { get; set; }       
        public int CNT { get; set; }
    }

}