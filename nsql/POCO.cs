using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IOrientObjects;

namespace POCO
{

    //MODEL scope
    #region POCOs

    //@JsonInclude(Include.NON_NULL)


    //Orient object
   public class OrientDatabase : IOrientDatabase
    {

    }
    public class OrientClass : IOrientClass
    {
        public string type { get; set; }

        public string name { get; set; }
       
    }
    public class OrientProperty : IOrientProperty
    {
        public string type { get; set; }

        public string name { get; set; }

    }
    public class OrientVertex : IOrientVertex
    {
        public string type { get; set; }
        [JsonProperty("@rid")]
        public string id { get; set; }

        public string version { get; set; }

        public string class_ { get; set; }
    }
    public class OrientEdge : IOrientEdge
    {
        public string type { get; set; }

        public string id { get; set; }

        public string version { get; set; }

        public string class_ { get; set; }

        public string In { get; set; }
        public string Out { get; set; }
    }

    /// <summary>
    /// Orient Objects
    /// </summary>

    //Vertexes
    public class Person : OrientVertex
    {

        public long? Seed { get; set; }
        [JsonProperty("Created", Order = 3)]
        public DateTime? Created { get; set; }
        [JsonProperty("GUID", Order = 2)]
        public string GUID { get; set; }
        [JsonProperty("Changed", Order = 4)]
        public DateTime? Changed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Birthday { get; set; }
        public string mail { get; set; }
        public int? telephoneNumber { get; set; }
        public int? userAccountControl { get; set; }
        public string objectGUID { get; set; }
        public string sAMAccountName { get; set; }
        [JsonProperty("Name", Order = 1)]
        public string Name { get; set; }
        public string OneSHash { get; set; }
        public string Hash { get; set; }

        /*
        [JsonProperty("id")]
        public new string id { get; set; }

        
        public List<string> in_MainAssignment { get; set; }
        public List<string> out_MainAssignment { get; set; }
        */

        [JsonProperty("fieldTypes")]
        public string @fieldTypes { get; set; }

    }
    public class Unit : OrientVertex
    {
        public long? Seed { get; set; }
        public DateTime? Created { get; set; }
        public string GUID { get; set; }
        public DateTime? Changed { get; set; }
        public string PGUID { get; set; }
        public string DepartmentColorRGB { get; set; }
        public string DepartmentColorClass { get; set; }
        public DateTime? Disabled { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }

    }
    public class UserSettings : OrientVertex
    {
        public bool showBirthday { get; set; }
    }

    //Edges
    public class MainAssignment : OrientEdge
    {
        [JsonProperty("Name", Order = 1)]
        public string Name { get; set; }
        [JsonProperty("GUID", Order = 2)]
        public string GUID { get; set; }
        [JsonProperty("Created", Order = 3)]
        public DateTime? Created { get; set; }
        [JsonProperty("Changed", Order = 4)]
        public DateTime? Changed { get; set; }
    }
    public class OldMainAssignment : OrientEdge
    {

    }
    public class OutExtAssignment : OrientEdge
    {
    }
    public class SubUnit : OrientEdge
    {

    }
    public class CommonSettings : OrientEdge
    {

    }


    public class TrackBirthdays : OrientEdge
    {

    }

    /*
    CREATE class UserSettings extends V;
    CREATE PROPERTY UserSettings.showBirthday BOOLEAN;
    CREATE CLASS CommonSettings EXTENDS E;
    */

    //for spagetty check
    public class MigrateCollection
    {
        public string @rid { get; set; }
        public string @class { get; set; }
        public string GUID { get; set; }
    }

    #endregion

    #region BreweryPOCOs
    public class Brewery : OrientVertex
    {
        string Name { get; set; }
        DateTime Created { get; set; }
        string Changed { get; set; }
    }
    public class Beer : OrientVertex
    {        
        string Sort { get; set; }
        DateTime Created { get; set; }
        string Changed { get; set; }
    }
    #endregion

}