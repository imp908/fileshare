using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System;
using System.Collections.Generic;

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
        public string type {get;set;}

        public string name {get;set;}
       
    }
    public class OrientProperty : IOrientProperty
    {
        public string type {get;set;}

        public string name {get;set;}

    }
    public class V : IOrientVertex
    {
        public string type {get;set;}
        [JsonProperty("@rid")]
        public string id {get;set;}
        public string version {get;set;}
        public string class_ {get; set;}
    }
    public class Object_SC : V
    {
        [JsonProperty("GUID", Order = 2)]
        public string GUID { get; set; } = null;
        [JsonProperty("Created", Order = 3)]
        public DateTime? created { get; set; } = null;
        [JsonProperty("Changed", Order = 4)]
        public DateTime? changed { get; set; } = null;
        [JsonProperty("Disabled", Order = 5)]
        public DateTime? disabled { get; set; } = null;
        [JsonProperty("Content_", Order = 2)]
        public string content { get; set; } = null;
    }
    public class E : IOrientEdge
    {
        public string type {get; set;}

        public string id {get; set;}

        public string version {get; set;}

        public string class_ {get; set;}

        public string In {get; set;}
        public string Out {get; set;}
    }

    /// <summary>
    /// Orient Objects
    /// </summary>

    //Vertexes      
    public class Person : V
    {

        public long? Seed { get; set; } = 0;
        [JsonProperty("Created", Order=3)]
        public DateTime? Created {get; set;} = DateTime.Now;
        [JsonProperty("GUID", Order = 2)]
        public string GUID { get; set; } = string.Empty;
        [JsonProperty("Changed", Order = 4)]
        public DateTime? Changed { get; set; } = DateTime.Now;
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string MiddleName {get; set;}
        public DateTime? Birthday {get; set;}
        public string mail {get; set;}
        public int? telephoneNumber {get; set;}
        public int? userAccountControl {get; set;}
        public string objectGUID {get; set;}
        public string sAMAccountName {get; set;}
        [JsonProperty("Name", Order=1)]
        public string Name {get; set;}
        public string OneSHash {get; set;}
        public string Hash {get; set;}

        /*
        [JsonProperty("id")]
        public new string id {get; set;}

        
        public List<string> in_MainAssignment {get; set;}
        public List<string> out_MainAssignment {get; set;}
        */

        [JsonProperty("fieldTypes")]
        public string @fieldTypes {get; set;}

    }
    public class Unit : V
    {
        public long? Seed {get; set;}
        public DateTime? Created {get; set;}
        public string GUID {get; set;}
        public DateTime? Changed {get; set;}
        public string PGUID {get; set;}
        public string DepartmentColorRGB {get; set;}
        public string DepartmentColorClass {get; set;}
        public DateTime? Disabled {get; set;}
        public string Hash {get; set;}
        public string Name {get; set;}

    }
    public class UserSettings : V
    {
        public bool showBirthday {get; set;}
    }

    //Edges
    public class MainAssignment : E
    {
        [JsonProperty("Name", Order=1)]
        public string Name {get; set;}
        [JsonProperty("GUID", Order=2)]
        public string GUID {get; set;}
        [JsonProperty("Created", Order=3)]
        public DateTime? Created {get; set;}
        [JsonProperty("Changed", Order=4)]
        public DateTime? Changed {get; set;}
    }
    public class OldMainAssignment : E
    {

    }
    public class OutExtAssignment : E
    {
    }
    public class SubUnit : E
    {

    }
    public class CommonSettings : E
    {

    }


    public class TrackBirthdays : E
    {

    }

    /*
    CREATE class UserSettings extends V;
    CREATE PROPERTY UserSettings.showBirthday BOOLEAN;
    CREATE CLASS CommonSettings EXTENDS E;
    */

    //Note
    public class Note : V
    {
        string somethingNew { get; set; }
        public string Name { get; set; }

        public string pic { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string guid { get; set; } = null;
        public string content { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;

        public DateTime? pinned { get; set; } = null;
        public DateTime? published { get; set; } = null;

        int likes { get; set; } = 0;
        bool liked { get; set; } = false;
    }
    public class Comment : E
    {
        DateTime? When { get; set; }
    }
    public class Authorship : E
    {
        string strField { get; set; }
    }
    public class Like : E
    {
        int cnt { get; set; }
    }
    public class Tagged : E
    {
        string tagText { get; set; }
    }
    public class Tag : V
    {
        string tagText{ get; set; }
    }

    //for spagetty check
    public class MigrateCollection
    {
        public string @rid {get; set;}
        public string @class {get; set;}
        public string GUID {get; set;}
    }

    #endregion

    #region BreweryPOCOs
    public class Brewery : V
    {
        string Name {get; set;}
        DateTime Created {get; set;}
        string Changed {get; set;}
    }
    public class Beer : V
    {       
        string Sort {get; set;}
        DateTime Created {get; set;}
        string Changed {get; set;}
    }
    #endregion

    #region Quiz

    public class QuizGet : V
    {
        [JsonProperty("Author")]
        public string Author {get; set;}=null;
        [JsonProperty("QuizName")]
        public string Name {get; set;}=null;
        [JsonProperty("State")]
        public string State {get; set;}=null;
        [JsonProperty("StartDate"), JsonConverter(typeof(YDMminus))]
        public DateTime StartDate {get; set;}=DateTime.Now;
        [JsonProperty("EndDate"), JsonConverter(typeof(YDMminus))]
        public DateTime EndDate {get; set;}=DateTime.Now;
        
    }
    public class QuizSend
    {      
        public string title {get; set;}=null;       
        public QuizHrefNode href {get; set;}= new QuizHrefNode();
        [JsonProperty("id")]
        public int? id {get; set;}=500;
        [JsonProperty("parentid")]
        public int? parentid {get; set;}=50;
    }
    public class QuizHrefNode
    {
        public string link {get; set;}=null;
        public string target {get; set;}="_self";
    }
    
    #endregion

    #region JsonConverters
    //Mooved to JSON manager

    class MonthDayYearDateConverter : IsoDateTimeConverter
    {
        public MonthDayYearDateConverter()
        {
            DateTimeFormat="dd.MM.yyyy";
       }
    }

    class MonthDayYearDateNoDotsConverter : IsoDateTimeConverter
    {
        public MonthDayYearDateNoDotsConverter()
        {
            DateTimeFormat="yyyyMMdd";
       }
    }

    class YDMminus : IsoDateTimeConverter
    {
        public YDMminus()
        {
            DateTimeFormat="yyyy-MM-dd hh:mm:ss";
       }
    }
    #endregion

}