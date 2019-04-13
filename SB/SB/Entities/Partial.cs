using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Entities
{
    class Partial
    { }

    public partial class COUNTRIES : IEntity, ICountry
    { }

    public partial class REGIONS : IEntity
    { }

    public class IEntity
    {
        //public Nullable<decimal> REGION_ID { get; set; }
    }

    public interface ICountry
    {
        string COUNTRY_NAME { get; set; }
    }
}
