using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace NSQLmamager
{
  class ClassReflectionsManager
  {
    
      /// <summary>
      /// Gets values from property collection of From_object, sets to same name properties of To_object. 
      /// Object types the same, no prop type cheking.
      /// </summary>
      /// <typeparam name="T">Object type</typeparam>
      /// <param name="fromObject"></param>
      /// <param name="toObject"></param>
      /// <returns></returns>
      public T UpdateProperties<T,K>(T fromObject,T toObject, K updateAttr)
        where T:class where K: System.Attribute
      {       
        T result = null;
        if(fromObject!=null&&toObject!=null){
          result=toObject;

          Type tpFrom=fromObject.GetType();
          Type tpTo=toObject.GetType();

          PropertyInfo[] propertiesFrom=tpFrom.GetProperties();
          PropertyInfo[] propertiesTo=tpTo.GetProperties();

          for(int i=0;i<propertiesFrom.Count();i++)
          {
            bool updateProperty = true;
            int propInd=-1;  
            IEnumerable<Attribute> propAttr = propertiesFrom[i].GetCustomAttributes();
            if( (from s in propAttr where s.GetType().Equals(typeof(K)) select s).Any())
            {

        K atr = (K)
        ((from s in propAttr where s.GetType().Equals(typeof(K)) select s).FirstOrDefault());

        //if(atr.isUpdatable==false)
        if(true)
        {
          updateProperty = false;
        }
         
            }

    if(updateProperty==true){
    PropertyInfo proeprtyFrom = propertiesFrom[i];
    var a = proeprtyFrom.GetValue(fromObject, null);

    for(int i2=0;i2<propertiesTo.Count();i2++){
    if(propertiesFrom[i].Name==propertiesTo[i2].Name){
      propInd=i2;
      object val_ = propertiesFrom[i].GetValue(fromObject, null);
      bool toUpdate = true;

      if(val_==null){
    toUpdate = false;
      }else{
      if(val_.GetType().Equals(typeof(string))){
      if(val_.ToString()==string.Empty)
      {
    toUpdate = false;
      }
      }
    }

    if(toUpdate){
    propertiesTo[i2].SetValue(result, propertiesFrom[i].GetValue(fromObject, null), null);
    }

    }
    }

            }
          }
        }

        return result;
      }
    
      //<<<move to separate PropertyWorking class
      public T PropertyTryReturnAttribute<T>(PropertyInfo p_) 
        where T:System.Attribute
      {          
        try{
        if(p_.GetCustomAttributes().Where(s=>s.GetType().Equals(typeof(T))).Any()){ 
            return (T)p_.GetCustomAttributes().Where(s=>s.GetType().Equals(typeof(T))).FirstOrDefault();
        }
        }catch(Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}
        return null;
      }

  }


}
