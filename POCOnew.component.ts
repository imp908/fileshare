
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
    if(o.typeName_==null){this._typeName="NodeNew"}

    }
    else{
      this._key=0;
      this._typeName="NodeNew";
    }
  }
}
export class CollectionNew<T extends NodeNew> extends NodeNew{

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:T[]}){
    if(o!=null){
      super(o)
      this.array=new Array<T>();
      if(o.array_!=null){
        this.addUpdateArr(o.array_);
      }
    }
  }
  tolog:boolean;
  ret:T;
  array:Array<T>;
  add(item:T){
    var max=0;
    var toPsuh:boolean=false;

    // console.log("Added");

    if(typeof(this.array)=='undefined'){
      if(this.tolog){
        console.log("PrimitiveCollection array Undefined")
      }
      this.array=Array<T>();
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
    this.setType(item._name);

    // console.log(["Array added",this.array]);
    return item;
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
      var index_=this.array.findIndex(s=>s._key===item._key);
        if(this.tolog){
          // console.log(index_);
        }

        if(index_!=null){
          // console.log("Add");
          this.add(item);
        }else{
          // console.log("Update");
          this.update(item);
        }
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
    if(typeof(this.array)!=null){
      var index_=this.array.findIndex(s=>s._key===item._key);
      if(index_!=-1){
        return this.array[index_];
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
    if(this.ret!=null){
      return this.ret._typeName;
    }
  }
  setType(type_:string){
    if(this.ret!=null){
      this.ret._typeName=type_;
    }
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

}

// export class NodeCollectionNew extends CollectionNew<NodeNew>{
//
//   constructor(o:{key_:number, name_:string, value_:string, typeName_:string,
//     collection_:CollectionNew<NodeNew>}){
//     super(o);
//     this.collection=o.collection_;
//
//     // this.collection.addUpdateArr(o.collection_.array);
//
//   }
//
//   a:number;
//   collection:CollectionNew<NodeCollectionNew>;
//
//
//   getType_():string {
//
//       //return this.collection.getType();
//     if(this.collection!=null){
//       return this.constructor.name;
//     }
//   }
//   sortHierarhy(asc:boolean){
//     if((this.collection!=null)){
//       // console.log("sort");
//       this.collection.sort(asc);
//       if((this.collection.array!=null) && (this.collection.array.length!=-1)){
//           for(let i =0;i<this.collection.array.length;i++){
//             // console.log("go deeper");
//             this.collection.array[i].sortHierarhy(asc);
//           }
//       }
//     }
//   }
//
//   //Recursive array collection search
//
//   scan(name_:string,col_:NodeCollectionNew){
//     let ret_:NodeCollectionNew=null;
//     ret_=this.findInParams(name_,col_,ret_);
//     return ret_;
//   }
//   findInParams(name_:string,col_:NodeCollectionNew,ret_:NodeCollectionNew){
//     // console.log(["findInParams: ",col_])
//
//     if(col_.collection!=null){
//       if(col_.collection.array!=null){
//         if(col_.collection.array.length>0){
//           for(let i=0;i<=col_.collection.array.length;i++){
//             let tCol_=col_.collection.array[i];
//             // console.log(["For: ",tCol_])
//             if(tCol_!=null){
//               if(tCol_._name==name_){
//                 // console.log(["Return: ",tCol_])
//                 ret_=tCol_;
//               }
//               if(tCol_.collection!=null){
//                 ret_=this.findInParams(name_,tCol_,ret_);
//               }
//             }
//           }
//         }
//       }
//     }
//
//       return ret_;
//   }
//
//   _hasArray(){
//     if(this._hasCollection()){
//       if(this.collection.array!=null){
//         if(this.collection.array.length>0){
//           return true;
//         }
//       }
//     }
//     return false;
//   }
//   _hasCollection(){
//     if((this.collection!=null)
//     ){
//       return true;
//     }else{return false;}
//   }
// }


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
      this._typeName="HtmlItemNew"
    }
  }
}

//html iytems for form createion and quiz elements controls display

export class TextControlNew extends HtmlItemNew{

  pattern:string;
  maxLength:number;
  minLength:number;

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:TextControlNew[]
  ,cssClass_:string
  ,show_:boolean
  ,HtmlTypeAttr_:string
  ,HtmlSubmittedValue_:any
  ,pattern_?:string
  ,maxLength_?:number
  ,minLength_?:number}){
    if(o!=null){
      super(o);
      this.pattern=o.pattern_;
      this.maxLength=o.maxLength_;
      this.minLength=o.minLength_;
    }
    else{
      this._typeName="TextControlNew"
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
  typeName_?:string,array_:CheckBoxControlNew[]
  ,cssClass_:string
  ,show_:boolean
  ,HtmlTypeAttr_:string
  ,HtmlSubmittedValue_:any}){
    if(o!=null){
      super(o);

    }
    else{
      this._typeName="CheckBoxControlNew"
    }
  }
}


//Quiz objects
//-----------


//base quiz object

export class QuizItemNew extends CollectionNew<QuizItemNew>{
  show:boolean;
  itemControlls:HtmlItemNew[];

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:QuizItemNew[],
  itemControlls_:HtmlItemNew[]
  ,cssClass_:string,show_:boolean}){
    if(o!=null){
      super(o);
      this.itemControlls=o.itemControlls_;
    }

  }
}

//child quiz objects

export class AnswerNew extends QuizItemNew{

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:QuizItemNew[]
  ,itemControlls_:HtmlItemNew[]
  ,cssClass_:string,show_:boolean}){
    if(o!=null){
      super(o);
      this.itemControlls=o.itemControlls_;
    }

  }
}
export class QuestionNew extends QuizItemNew{

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:AnswerNew[]
  ,itemControlls_:HtmlItemNew[]
  ,cssClass_:string,show_:boolean}){
    if(o!=null){
      super(o);
      this.itemControlls=o.itemControlls_;
    }

  }
}
export class QuizNew extends QuizItemNew{

  constructor(o:{key_?:number,
  name_?:string,
  value_?:string,
  typeName_?:string,array_:QuestionNew[]
  ,itemControlls_:HtmlItemNew[]
  ,cssClass_:string,show_:boolean}){
    if(o!=null){
      super(o);
      this.itemControlls=o.itemControlls_;
    }
  }
}
