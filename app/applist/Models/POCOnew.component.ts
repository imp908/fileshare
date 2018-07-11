
//base item for object iterating

export class NodeNew{
  _uniqueKey:number;
  _key:number;
  _name:string;
  _value:string;
  _typeName:string;

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string}){

    if(o!=null){
      this._key=o.key_;
      this._name=o.name_;
      this._value=o.value_;
      this._typeName=o.typeName_;

      if(o.key_==null){this._key=0;}
      if(o.typeName_==null){this._typeName=this.constructor.name}
    }else{
      this._key=0;
      this._typeName=this.constructor.name;
    }
  }
}
export class CollectionNew<T extends NodeNew> extends NodeNew{
  tolog:boolean;
  ret:T;
  array:Array<T>;
  collectionType:string;

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:T[]}){
    if(o!=null){
      super(o)
      this.array=new Array<T>();
      if(o.array_!=null){
        // this.addUpdateArr(o.array_);
          this.array=o.array_;
      }
    }else{this._typeName=this.constructor.name}
  }


  add(item:T){
    console.log(["Add",this,item]);

    var max=0;
    var toPsuh:boolean=false;

    // console.log("Added");

    if(typeof(this.array)=='undefined'){
      if(this.tolog){
        console.log("PrimitiveCollection array Undefined")
      }
      this.array=new Array<T>();
    }

    max=this.getMaxKey();

    if(this.tolog){
      console.log(["PrimitiveCollection array max = ",max])
    }

    if(item._key==null){

      //tolog
      if(this.tolog){console.log(["Item key is null"])}

      if(max!=-1){

        //tolog
        if(this.tolog){console.log(["array contains some elements"])}

        max+=1;
        item._key=max;
        toPsuh=true;
      }else{
        //tolog
        if(this.tolog){console.log(["array contains no elements"])}
        item._key=0;
        toPsuh=true;
      }

    }else{

      //tolog
      if(this.tolog){console.log(["Item key is: ",item._key])}

      if(max!=-1){
        //tolog
        if(this.tolog){console.log(["Array not empty"])}

          if((this.getByItem(item)!=null)){
            //tolog
            if(this.tolog){console.log(["Array contains item: ",item])}

            max+=1;
            item._key=max;
            toPsuh=true;

          }else
          {
            //tolog
            if(this.tolog){console.log(["Array not contains item"])}
            toPsuh=true;
          }
      }else{
        //tolog
        if(this.tolog){console.log(["Array is empty"])}
        toPsuh=true;
      }

    }

    if(toPsuh===true){
      if(this.tolog){console.log(["pushing item with key: ",item,max])}
      this.array.push(item);
    }
    this.collectionType=item._typeName;

    // console.log(["Array added",this.array]);
    return item;
  }
  addArr(items:T[]){
    for(let i of items){
      this.add(i);
    }
  }
  delete(item:T){
    if(typeof(this.array)!=null){
      var index_:number=this.getIndexByItem(item)
      if(index_!=-1){
        this.array.splice(index_,1);
        return this.array
      }
    }
    return null;
  }
  update(item:T){
    console.log(["Update",this,item]);
    var max=0;
    var toPsuh:boolean=false;

    if(typeof(this.array)!=null){
      //log
      if(this.tolog){console.log("PrimitiveCollection array exists")}
      max=this.getMaxKey();
      if(max>-1){
        //log
        if(this.tolog){console.log("Array contains items")}
        var index_=this.array.findIndex(s=>s._key===item._key);
        if(index_!=-1){
          //log
          if(this.tolog){console.log(["Array contains item",item])}
            this.array[item._key]=item;
        }
      }
    }

    return this.array;
  }

  addUpdate(item:T){
    if((typeof(this.array)!=null)){
      if(item._key!=null){

      var index_=this.array.find(s=>s._key==item._key);
        if(this.tolog){
          // console.log(index_);
        }
        console.log(["addUpdate",this,item,index_]);
        if(index_!=null){
          // console.log("Update");
          this.update(item);
        }else{
          // console.log("Add");
          this.add(item);
        }

      }else{this.add(item);}
    }

  }

  addUpdateArr(items:Array<T>){

    for(var item of items){
          // console.log(["addUpdateArr",item])
      this.addUpdate(item);
    }
  }

  getMaxKey(){
    if(typeof(this.array)!=null){
      var max=Math.max.apply(Math,this.array.map(function(o){return o._key;}))
      if(!isFinite(max)){
        //console.log("Max infinite")
        max=-1
      }
      if(max!=null){
        return max;
      }
    }
    return null;
  }
  getByItem(item:T){
    if((typeof(this.array)!=null) && item!=null){
      try{
        var index_=this.array.findIndex(s=>s._key===item._key);

        if(index_!=-1){
          return this.array[index_];
        }
      }catch(e){
        if(this.tolog==true){console.log(e)}
      }
    }
    return null;
  }
  getByName(item:string){
    if(typeof(this.array)!=null){
      var ret=this.array.find(s=>s._name===item);
      if(ret!=null){
        return ret;
      }
    }
    return null;
  }
  getByKey(key:number){
    if(typeof(this.array)!=null){
      var index_=this.array.findIndex(s=>s._key===key);
      if(index_!=-1){
        return this.array[index_];
      }
    }
    return null;
  }
  getIndexByItem(item:T){
    if(typeof(this.array)!=null){
      return this.array.findIndex(s=>s._key===item._key);
    }
    return -1;
  }
  getIndexBykey(key:number){
      if(typeof(this.array)!=null){
        return this.array.findIndex(s=>s._key===key);
      }
      return -1;
    }

  isUndefined(arr_:Array<T>):boolean{
    if(typeof(arr_)=='undefined'){
      if(this.tolog){
        console.log("PrimitiveCollection array Undefined")
      }
      return true;
    }
    return false;
  }

  getType():string {
    return this._typeName;
  }
  setType(type_:string){
    this._typeName=type_;
  }

  sortAsc(a:T,b:T){
    if(a._key>b._key){return 1}
    if(a._key<b._key){return -1}
    return 0;
  }
  sortDesc(a:T,b:T){
    if(a._key>b._key){return -1}
    if(a._key<b._key){return 1}
    return 0;
  }
  sort(asc:boolean){
    let a:Array<T>;
    if(asc){
      a=this.array.sort(this.sortAsc);
    }else{a=this.array.sort(this.sortDesc);}
    return a;
  }

  getType_():string {

      //return this.collection.getType();
    if(this.array!=null){
      return this.constructor.name;
    }

  }

  hasCollection(){
    if(this.array!=null){if(this.array.length>0){return true;}}
    return false;
  }

}

//html objects from button to form items
//------

//base html item

export class HtmlItemNew extends CollectionNew<HtmlItemNew>{

  cssClass:string;
  HtmlTypeAttr:string;
  HtmlSubmittedValue:any;
  show:boolean;

  constructor(o:{key_?:number,
    name_?:string,
    value_?:string,
    typeName_?:string
    ,array_:HtmlItemNew[]
    ,cssClass_:string
    ,show_:boolean
    ,HtmlTypeAttr_:string
    ,HtmlSubmittedValue_:any}){
    if(o!=null){
      super(o);
      this.show=o.show_;
      this.cssClass=o.cssClass_;
      this.HtmlTypeAttr=o.HtmlTypeAttr_;
      this.HtmlSubmittedValue=o.HtmlSubmittedValue_;
    }
    else{
      this._typeName=this.constructor.name
    }
  }

  scan(name_:string,f_:HtmlItemNew[]){
    let r_:HtmlItemNew=null;
      r_=HtmlItemNew.recArr(name_,f_,r_)
      // console.log("Return ed ext ",r_)
    return r_;
  }
  static recArr(name_:string,itm_:Array<HtmlItemNew>,ret_:HtmlItemNew){
    // console.log("started ",itm_,ret_)
    if(itm_!=null){
    if(itm_.length>0){
      for(let i=0;i<itm_.length;i++){
        // console.log("inited for i: ",i,itm_,ret_)
        if(itm_[i]._name==name_){
          ret_= itm_[i];
          // console.log(["Itm name: ",itm_[i]._name,name_,ret_])
        }else{
          if(itm_[i].array!=null){
          if(itm_[i].array.length>0){
            // console.log("recurced:  ",i,itm_[i].array,ret_)
            ret_=HtmlItemNew.recArr(name_,itm_[i].array,ret_);
          }
          }
        }
      }
    }
    }

    // console.log("returned ",ret_)
    return ret_;
  }

  DeepClone(){
    let  r_ = Object.assign(
      Object.create(
        Object.getPrototypeOf(this)
      ),this
      );
      return r_;
  }
  recObj(){
    let ret_:HtmlItemNew;
      ret_=this.DeepClone();
      ret_.array=[];
        if(this.array!=null){
        if(this.array.length){
          let ln=this.array.length;
          for(let i=0;i<ln;i++){
            let ps=this.array[i].recObj();
            // console.log(ps)
            ret_.array.push(ps);
          }
        }
        }
    return ret_;
  }

}

//html iytems for Form Controlls createion and quiz elements controls display

export class TextControlNew extends HtmlItemNew{

  pattern:string;
  maxLength:number;
  minLength:number;
  displayValue:string;

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:HtmlItemNew[]
  ,cssClass_:string
  ,show_:boolean
  ,HtmlTypeAttr_:string
  ,HtmlSubmittedValue_:any
  ,DisplayValue_:any
  ,pattern_?:string
  ,maxLength_?:number
  ,minLength_?:number}){
    if(o!=null){
      super(o);
      this.pattern=o.pattern_;
      this.maxLength=o.maxLength_;
      this.minLength=o.minLength_;
      this.displayValue=o.DisplayValue_;
    }
    else{
      this._typeName=this.constructor.name
    }
  }
}
export class CheckBoxControlNew extends HtmlItemNew{

  pattern:string;
  maxLength:number;
  minLength:number;

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:HtmlItemNew[]
  ,cssClass_:string
  ,show_:boolean
  ,HtmlTypeAttr_:string
  ,HtmlSubmittedValue_:any}){
    if(o!=null){
      super(o);
    }
    else{
      this._typeName=this.constructor.name
    }
  }
}
export class RadioButtonControlNew extends HtmlItemNew{

}
export class DropDownControlNgNew extends HtmlItemNew{

}
export class DropDownControlMultiNgNew extends HtmlItemNew{

}
export class DropDownControlMultiNew extends HtmlItemNew{

}
export class DatePickerControlNew extends HtmlItemNew{
  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string
  ,array_:HtmlItemNew[]
  ,cssClass_:string
  ,show_:boolean
  ,HtmlTypeAttr_:string
  ,HtmlSubmittedValue_:Date}){
    if(o!=null){
      super(o);
    }
    else{
      this._typeName=this.constructor.name
    }
  }
}
export class NumberPickerControlNew extends HtmlItemNew{

  minN?:number;
  maxN?:number;
  DisplayValue:number;
  overflow:boolean;

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string
  ,array_:HtmlItemNew[]
  ,cssClass_:string
  ,show_:boolean
  ,HtmlTypeAttr_:string
  ,HtmlSubmittedValue_:number
  ,DisplayValue_:number
  ,minN?:number
  ,maxN?:number}){
    if(o!=null){
      super(o);
      this.minN=o.minN;
      this.maxN=o.maxN;
      this.DisplayValue=o.DisplayValue_;
    }
    else{
      this._typeName=this.constructor.name
    }
  }

  checkInput(res:number){
    let ok:boolean=null;

    if(this.maxCheck(res) &&
    this.minCheck(res) ) {
      this.DisplayValue=res;
      this.HtmlSubmittedValue=this.DisplayValue;
      this.overflow=false;
    }else{
      console.log("not ok", this.HtmlSubmittedValue,this.DisplayValue)
      this.HtmlSubmittedValue=null;
      this.overflow=true;
    }

  }
  maxCheck(res:number){
    if(this.minN!=null){
      if(res>=this.minN){
        return true;
      }else{return false;}
    }
    return true;
  }
  minCheck(res:number){
    if(this.maxN!=null){
      if(res<=this.maxN){
        return true;
      }else{return false;}
    }
    return true;
  }

}

export class LabelControlNew extends HtmlItemNew{}

//Quiz objects
//-----------


//base quiz object

export class QuizItemNew extends HtmlItemNew{
  show:boolean;
  itemControlls:HtmlItemNew[];

  constructor(o:{key_?:number,name_?:string,value_?:string,typeName_?:string
    ,array_:QuizItemNew[],itemControlls_:HtmlItemNew[]
    ,cssClass_:string,show_:boolean,HtmlTypeAttr_:string,HtmlSubmittedValue_:any}){
    if(o!=null){
      super(o);
      this.itemControlls=o.itemControlls_;
      this.show=o.show_;
      this.cssClass=o.cssClass_;
      this.HtmlTypeAttr=o.HtmlTypeAttr_;
      this.HtmlSubmittedValue=o.HtmlSubmittedValue_;
    }
    else{
      this._typeName=this.constructor.name
    }
  }

  recObj(){
    let ret_:QuizItemNew;
      ret_=this.DeepClone();
      ret_.array=[];
      ret_.itemControlls=[];

      if(this.itemControlls!=null){
        let ln=this.itemControlls.length;
        for(let i=0;i<ln;i++){
          let ps=this.itemControlls[i].recObj();
          // console.log(ps)
          ret_.itemControlls.push(ps);
        }
      }

      if(this.array!=null){
      if(this.array.length){
        let ln=this.array.length;
        for(let i=0;i<ln;i++){
          let ps=this.array[i].recObj();
          // console.log(ps)
          ret_.array.push(ps);
        }
      }
      }

    return ret_;
  }

  nameItem(){
    if(this.itemControlls!=null){
      if(this.itemControlls.length>0){
        let itm=this.scan("ItemName",this.itemControlls);
        // console.log("------------------------")
        // console.log(["nameExtractBind",this.itemControlls,itm])
        if(itm!=null){
          return itm;
        }
      }
    }
    return null;
  }
  nameObjectToItem(){
    let extrName=this.nameItem();
    if(extrName!=null){
      extrName.HtmlSubmittedValue=this._name;
    }
  }
  nameItemToObject(){
    let extrName=this.nameItem();
    if(extrName!=null){
      this._name=extrName.HtmlSubmittedValue;
    }
  }

}

//child quiz objects

export class AnswerNew extends QuizItemNew{}
export class QuestionNew extends QuizItemNew{}
export class QuizNew extends QuizItemNew{}


//Buttons

export class ButtonNew extends HtmlItemNew{
  _clicked:boolean;
  _toolTipText:string;
  _disabled:boolean;

  constructor(o:{key_?:number,name_?:string,value_?:string,typeName_?:string
    ,array_:QuizItemNew[],
    itemControlls_:HtmlItemNew[]
    ,cssClass_:string,show_:boolean,HtmlTypeAttr_:string
    ,HtmlSubmittedValue_:any
    ,clicked_:boolean,toolTipText_:string,disabled_:boolean}){
    if(o!=null){
      super(o);
      this._clicked=o.clicked_;
      this._toolTipText=o.toolTipText_
      this._disabled=o.disabled_;
    }else{
      this._typeName=this.constructor.name
    }
  }

}


//Buttons used in Quiz

export class NewAddNew extends ButtonNew{}

export class SaveNew extends ButtonNew{}
export class EditNew extends ButtonNew{}
export class CopyNew extends ButtonNew{}
export class DeleteNew extends ButtonNew{}
export class Cancel extends ButtonNew{}
