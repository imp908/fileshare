using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;

using System;

using IOrientObjects;

using System.Collections.Generic;

namespace POCO
{

    //MODEL scope
    #region POCOs

    //@JsonInclude(Include.NON_NULL)


    //Orient object
  
   
    public class OrientEntity : IOrientEntity
    {   
        [JsonProperty("@rid", Order=1)]
        public virtual string id { get; set; }
      
        [JsonProperty("@type")]
        public string type {get; set; }           
        [JsonProperty("@class")]
        public string class_ {get; set;} 
        [JsonProperty("value")]
        public string value_ {get; set;} 
      
        [JsonProperty("@version")]
        public virtual string version {get;  set;}   

        public bool ShouldSerializeclass_()
        {
          return false;
        }
        public bool ShouldSerializetype()
        {
          return false;
        }
        public bool ShouldSerializevalue_()
        {
          return false;
        }
    }
    
    public class OrientDefaultObject:OrientEntity,IOrientDefaultObject
    {
      [Mandatory(true),Updatable(false)]
      [JsonProperty("GUID", Order = 2)]
      public string GUID { get; set; } = null;
      [JsonProperty("created", Order = 3),JsonConverter(typeof(OrientDateTime))]
      public virtual DateTime? created { get; set; } = DateTime.Now;
      [JsonProperty("changed", Order = 4),JsonConverter(typeof(OrientDateTime))]
      public virtual DateTime? changed { get; set; } = DateTime.Now;
      [JsonProperty("Disabled", Order = 5),JsonConverter(typeof(OrientDateTime))]
      public DateTime? disabled { get; set; }
      
      public bool ShouldSerializeGUID()
      {
        return true;
      }
    }
    public class OrientEdge :OrientDefaultObject, IOrientEdge
    {
      
        [JsonProperty("out"),NOTSynchronisable(true)]
        public string Out {get; set;}    
        [JsonProperty("in"),NOTSynchronisable(true)]
        public string In {get; set;}

        public bool ShouldSerializeOut()
        {
            return false;
        }
        public bool ShouldSerializeIn()
        {
            return false;
        }
    }

    
    public class V : OrientDefaultObject, IOrientVertex
    {
              
      [JsonProperty("@rid", Order = 1)]
      public override string id { get; set; }       
      [JsonProperty("@version")]
      public override string @version {get; set;}         
      
      //Excluding fields from serializing to string
      public bool ShouldSerializeid()
      {
        return false;
      }
      public bool ShouldSerializeversion()
        {
          return false;
        }        
       
    }
    public class Object_SC : V
    {

    }
    public class E : OrientEdge
    {     
                    
      [JsonProperty("@rid", Order = 1)]
      public override string id { get; set; }
      [JsonProperty("@version")]
      public override string @version {get; set;}
        
      public bool ShouldSerializeid()
      {
        return false;
      }
      public bool ShouldSerializeversion()
      {
        return false;
      }

      public bool ShouldSerializeout()
      {
        return false;
      }
      public bool ShouldSerializein()
      {
        return false;
      }

    }

    /// <summary>
    /// Orient Objects
    /// </summary>

    //Vertexes      
    public class Person : V, IEquatable<Person>
    {
      [JsonProperty("Seed")]
      public long? Seed { get; set; }      
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string MiddleName { get; set; }
      [JsonConverter(typeof(OrientDateTime))]
      public DateTime? Birthday { get; set; }
      public string mail { get; set; }
      public int? telephoneNumber { get; set; }
      public int? userAccountControl { get; set; }
      public string objectGUID { get; set; }
      public string sAMAccountName { get; set; }
      [JsonProperty("Name", Order = 1)]
      public string Name { get; set; }
      [IsComparable(false)]
      public string OneSHash { get; set; }
      [JsonProperty("Hash"),IsComparable(false)]
      public string Hash { get; set; }

      [JsonConverter(typeof(OrientDateTime))]
      public override DateTime? changed{get;set;}
      [JsonConverter(typeof(OrientDateTime))]
      public override DateTime? created{get;set;}

      /*
      [JsonProperty("id")]
      public new string id {get; set;}

        
      public List<string> in_MainAssignment {get; set;}
      public List<string> out_MainAssignment {get; set;}
      

      [JsonProperty("fieldTypes")]
      public string @fieldTypes { get; set; }
      */

      //Excluding fields from serializing to string
      public bool ShouldSerializeSeed()
      {
        return false;
      }
      public bool ShouldSerializeuserAccountControl()
      {
        return false;
      }
      public bool ShouldSerializeOneSHash()
      {
        return false;
      }
      public bool ShouldSerializeHash()
      {
        return false;
      }
      public bool ShouldSerializecreated()
      {
        return false;
      }
      public bool ShouldSerializechanged()
      {
        return false;
      }
      public bool ShouldSerializeBirthday()
      {
        return false;
      }

      public override bool Equals(object obj)
      {
        if(obj is Person){
        return Equals((Person)obj);
        }else{return false;}
      }    
      public bool Equals(Person p_)
      {
        return withGUIDcomparer(p_);
      }
      public bool NoGUIDcomparer(Person p_)
      {
        
        if(     
          this.Name==p_.Name
          &&this.mail==p_.mail
          &&this.Birthday==p_.Birthday
          &&this.FirstName==p_.FirstName
          &&this.LastName==p_.LastName
          &&this.MiddleName==p_.MiddleName
          &&this.mail==p_.mail
          &&this.sAMAccountName==p_.sAMAccountName
          &&this.telephoneNumber==p_.telephoneNumber     
        ){ return true; }
        return false;
      }
      public bool withGUIDcomparer(Person p_)
      {
        
        if(     
          NoGUIDcomparer(p_)==true
          &&this.GUID==p_.GUID      
        ){ return true; }
        return false;
      }
      public bool FullComparer(Person p_)
      {
        
        if(     
          withGUIDcomparer(p_)==true
          &&this.id==p_.id      
        ){ return true; }
        return false;
      }

      //default hash. Custom not returned.
      public override int GetHashCode()
      {        
        //accidental small prime number. just for fun
        int hashCode = 17;
        //"overflow is fine" they said
        unchecked{
          hashCode = (hashCode * 397) ^ DateTime.Now.ToString("yyyyMMddHHmmssf").GetHashCode();
        }
        return base.GetHashCode();
      }
    }
    public class Unit : V
    {
               
      public long? Seed { get; set; }      
      public string PGUID { get; set; }
      public string DepartmentColorRGB { get; set; }
      public string DepartmentColorClass { get; set; }
      [JsonConverter(typeof(OrientDateTime))]
      public DateTime? Disabled { get; set; }
      public string Hash { get; set; }
      public string Name { get; set; }

    }
    public class UserSettings : V
    {
      public bool? showBirthday { get; set; }
    }
    
    public class PersonWithMng : Person{   
      public string MangerAcc{ get; set; }
    }
  

    //Edges
    public class MainAssignment : E
    {
   
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
    public class PersonRelation : E
    {

    }

    public class TrackBirthdays : E
    {

    } 

    //Note to front
    public class Note : V
    {
      [JsonProperty("author_")]
      public virtual Person author_ { get; set; }

      public virtual string PGUID { get; set; }

      public virtual string authAcc { get; set; }
      public virtual string authGUID { get; set; }
      public virtual string authName { get; set; }

      public string pic {get;set;}
      public string name {get;set;}

      [JsonProperty("content_")]
      public virtual string content { get; set; }
           
      public virtual ToggledProperty pinned { get; set; } = new ToggledProperty();
      public virtual ToggledProperty published  { get; set; } = new ToggledProperty();
    
      public int? Likes {get;set;} 
      public Tag taggs {get;set;}

      [Updatable(true)]
      public int? commentDepth { get; set; }
      public bool? hasComments { get; set; }
    
    }
    public class News:Note{
      //[JsonProperty("content_")]
      //public override string content { get; set; }
      //[Updatable(false)]
      //public override Person author_ { get; set; }
      
      //[JsonIgnore]
      //[Updatable(false)]
      //public string contentBase64 {
      //  get{
      //  if(content!=null){
      //  System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(content));
      //  }return null;}

      //  set{content =
      //  System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
      //  }}
    }
    public class Commentary:Note{
      //[JsonProperty("content_")]
      //public override string content { get; set; }
      //[JsonConverter(typeof(OrientDateTime))]
      //public new DateTime? published { get; set; }=DateTime.Now;
      [JsonProperty("author_"),Updatable(false)]
      public override Person author_ { get; set; }
    }  

    public class Comment : E
    {
      [JsonConverter(typeof(OrientDateTime))]
      DateTime? When { get; set; } = DateTime.Now;
    }
    public class Authorship : E
    {
      string strField { get; set; }
    }
    public class Liked : E
    {
      int cnt { get; set; }
    }
    public class Tagged : E
    {
      
    }
    public class Tag : V
    {
      public string tagText { get; set; }
    }

    //Property for conditional values 
    public class ToggledProperty
    {
      public bool isTrue { get; set; } = false;
      [JsonConverter(typeof(OrientDateTime)), Updatable(false)]
      public DateTime? dateChanged { get; set; } = null;
    }
    
    //for database migrations 
    public class NodeReferenceConditional
    {
        public OrientDefaultObject orientItem { get; set; }
        public bool processed { get; set;} = false;
    }
    #endregion

    //classes returned by REST HTTP
    #region OrientMaintenanceClasses
    //returned by Oreint Http while db check
    //used for migration, classes and db shema contained
    public class OrientDatabase : IOrientDatabase
    {
      public List<OrientClass> classes {get;set;}
      public OrientConfigValues config {get;set;}
    }
    
    //from orient Http class shema
    public class OrientClass
    {
      public string name {get;set;}
      public string superClass {get;set;}
      public List<string> superClasses {get;set;}
      public string alias {get;set;}
      [JsonProperty("abstract")]
      public bool? abstract_ {get;set;}
      public bool? strictmode {get;set;}
      public List<int> clusters {get;set;}
      public int? defaultCluster {get;set;}
      public string clusterSelection {get;set;}
      public int? records {get;set;}
      [JsonProperty("properties")]
      public List<OrientProperty> properties_ {get;set;}
    }
    public class OrientProperty{
      public string name {get;set;}
      public string type {get;set;}
      public bool? mandatory {get;set;}
      [JsonProperty("readonly")]
      public bool? readonly_ {get;set;}
      public bool? notNull {get;set;}
      public int? min {get;set;}
      public int? max {get;set;}
      public string regexp {get;set;}
      public string collate {get;set;}
      public string defaultValue {get;set;}
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class OrientConfigValues
    {
      public List<OrientValue> values {get;set;}
    }
    public class OrientValue
    {
      public string name {get;set;}
      public string value {get;set;}
    }
    #endregion

    #region ControllerParams
    public class GETparameters
    {
      public int? offest {get;set;}
      public bool? published {get;set;}
      public bool? pinned {get;set;}
      public bool? asc {get;set;}
      public bool? liked {get;set;}
      public Tag tagg {get;set;}
      public Person author {get;set;}
    }
    public class PostTags
    {
      public News news_ {get;set;}
      public List<Tag> tags_ {get;set;}
    }
    public class StringResult
    {
      List<string> results { get; set; }
    }
    #endregion

    #region Attributes

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ToggleAttribute:System.Attribute
    {
      public bool toggledValue=true;
      public ToggleAttribute(bool val_)
      {
        this.toggledValue = val_;        
      }
    }    

    //Custom attributes
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Updatable:System.Attribute
    {
      public bool isUpdatable=true;
      public Updatable(bool isUpdatable_)
      {
        this.isUpdatable = isUpdatable_;        
      }
    }
     [System.AttributeUsage(System.AttributeTargets.Property)]
    public class NOTSynchronisable:System.Attribute
    {
      public bool isUpdatable=true;
      public NOTSynchronisable(bool isUpdatable_)
      {
        this.isUpdatable = isUpdatable_;        
      }
    }    
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Mandatory:System.Attribute
    {
      public bool isMandatory;
      public Mandatory(bool item_)
      {
        this.isMandatory = item_;
      }
    }
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class IsComparable:ToggleAttribute
    {
      public IsComparable(bool val_) : base(val_){}
    }
    #endregion

    #region BreweryPOCOs
    public class Brewery : V
    {
        string Name { get; set; }
        DateTime Created { get; set; }
        string Changed { get; set; }
    }
    public class Beer : V
    {
        string Sort { get; set; }
        DateTime Created { get; set; }
        string Changed { get; set; }
    }
    #endregion

    #region Quiz

    public class Quiz : V
    {
        [JsonProperty("Author")]
        public string Author { get; set; } = null;
        [JsonProperty("QuizName")]
        public string Name { get; set; } = null;
        [JsonProperty("State")]
        public string State { get; set; } = null;
        [JsonProperty("StartDate"), JsonConverter(typeof(YDMminus))]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [JsonProperty("EndDate"), JsonConverter(typeof(YDMminus))]
        public DateTime EndDate { get; set; } = DateTime.Now;
    }
    public class QuizGet : Quiz
    { 
        [JsonProperty("StartDate")]
        public new string StartDate { get; set; } = DateTime.Now.ToString();
        [JsonProperty("EndDate")]
        public new string EndDate { get; set; } = DateTime.Now.ToString();
    }
    public class QuizSend
    {
        public string title { get; set; } = null;
        public QuizHrefNode href { get; set; } = new QuizHrefNode();
        [JsonProperty("id")]
        public int? id { get; set; } = 500;
        [JsonProperty("parentid")]
        public int? parentid { get; set; } = 50;
    }
    public class QuizHrefNode
    {
        public string link { get; set; } = null;
        public string target { get; set; } = "_self";
    }
    
    #endregion

    #region QuizNew

    public class QuiznewType{
      
    }
    public class QuizItem :V
    {
      public int key {get;set;}
      public string name {get;set;}
      public string value {get;set;}

      public List<QuizItem> options {get;set;} 
    }
    public class Question : QuizItem
    {
        public bool toStore { get; set; } = true;
        public string type { get; set; } = "";
        
        public List<Answer> answers { get; set; }
    }
    public class Answer :QuizItem
    {
        public bool isChecked { get; set; } = true;
        public bool toStore { get; set; } = true;
    }

    public class QuizNewGet : QuizItem
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public List<Question> questions_ { get; set; }
    }

    #endregion

    #region JsonConverters
    //Mooved to JSON manager

    class MonthDayYearDateConverter : IsoDateTimeConverter
    {
        public MonthDayYearDateConverter()
        {
            DateTimeFormat = "dd.MM.yyyy";
        }
    }
    class MonthDayYearDateNoDotsConverter : IsoDateTimeConverter
    {
        public MonthDayYearDateNoDotsConverter()
        {
            DateTimeFormat = "yyyyMMdd";
        }
    }
    class YDMminus : IsoDateTimeConverter
    {
        public YDMminus()
        {
            DateTimeFormat = "yyyy-MM-dd hh:mm:ss";
        }
    }
    class OrientDateTime : IsoDateTimeConverter
    {
        public OrientDateTime()
        {        
            DateTimeFormat = ConfigurationManager.AppSettings["OrientDateTime"];
        }
    }   
    #endregion

    /// <summary>
    /// Test for checking serialization deserialization rules. 
    /// [JsonIgnore] - blocks property in both directions : serialization or deserialization.
    /// Conditional property, ShouldSerialize{PropertyName} : hides property while serializing to sting.    
    /// </summary>
    #region TestOrientFieldCleanPOCOS    
    public class TestOrientObjectPOCO : OrientDefaultObject
    {        
        [JsonIgnore]
        [JsonProperty("id",Order=1)]
        internal string @rid { get; set; }
        [JsonProperty("@version",Order=2)]
        internal new string @version { get; set; }
        public string @class { get; set; }
        public new string @type { get; set; }

        [Updatable(true)]
        public string UpdatableField  { get; set; }
        [Updatable(false)]
        public string nonUpdatableField  { get; set; }

        public bool ShouldSerializeversion()
        {
          return false;
        }
    }
    public class TestOrientNodeObject:TestOrientObjectPOCO
    {  

    }
    public class TestOrientEdgeObject:TestOrientNodeObject
    {     
       public virtual string @in{get;set;}
       public virtual string @out{get;set;}
       
    }        
    
    public class TestOreintDefaultObject:TestOrientNodeObject
    {
        public new string GUID{get;set;}
        public string ID{get;set;}
    } 
    public class TestPersonPOCO:TestOrientNodeObject
    {
      public string Name{get;set;}
      public string Acc{get;set;}
    }
    public class TestNews : Note
    {

      [JsonProperty("Created", Order = 3)]
      public new DateTime? created { get; set; } = DateTime.Now;
      [JsonProperty("Changed", Order = 4)]
      public new DateTime? changed { get; set; } = null;
      [JsonProperty("Disabled", Order = 5)]
      public new DateTime? disabled { get; set; } = null;

      [JsonProperty("content_")]
      public  new string content { get; set; }=string.Empty;
      public new string description { get; set; }=string.Empty;
    
      public new DateTime? pinned { get; set; }=null;

      [JsonProperty("published")]
      public new  DateTime? published { get; set; }=null;      
        
      
    }
    #endregion
    
}