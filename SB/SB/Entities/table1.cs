namespace SB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Reflection;

    public partial class table1
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? value1 { get; set; }

        public decimal? value2 { get; set; }

        [StringLength(250)]
        public string range1 { get; set; }

        [StringLength(250)]
        public string range2 { get; set; }

        public DateTime? date1 { get; set; }
    }
    
    public class table1DTO 
    {
        public DateTime? DATE { get; set; }
        public string RANGE{ get; set; }
        public int? AMT { get; set; }
    }

    public static class table1Condition
    {
        public static List<string> propertiesList { get; set; }

        public static void addProperties(Type type_)
        {
            var a = type_.GetGenericArguments();
          
            var b = a.GetType().GetProperties();
            
        }
    }

}
