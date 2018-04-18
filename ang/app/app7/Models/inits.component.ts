import {ServiceCl} from '../Services/services.component';

//TS Collections
//npm install typescript-collections [-g] --save
import * as Collections from 'typescript-collections';

import {IItem,ICollection,IItemCollection
,IFactory,IFactoryItemCollection
,INode,ICollection_,INodeCollection} from './POCO.component';


export class Item implements IItem {
  key:number=0;
  name:string="";
  value:string="";
  constructor(key_?:number,name_?:string, value_?:string){
    this.key=key_;
    this.name=name_;
    this.value=value_;
  }
}

export class ItemG implements IItem{
  key:number;
  name:string;
  value:string;
  constructor(options:{key_:number,name_:string, value_:string}={key_:0,name_ : "",value_:""})
  {
    this.key=options.key_;
    this.name=options.name_;
    this.value=options.value_;
  }
}

//collections lib realization
export class ItemCollectionC<T extends IItem>{
  collection=new Collections.Set<T>();
}

export class Collection<T extends IItem> implements ICollection<T>{
  array:Array<T>;

  constructor(options:{array_?:Array<T>}={array_:new Array<T>()}){
      this.array=options.array_;
  }

  add(item:T){
    var max=0;
    var toPsuh:boolean=false;

    if(typeof(this.array)=='undefined'){
      ServiceCl.log("PrimitiveCollection array Undefined")
      this.array=Array<T>();
      toPsuh=true;
    }else{

      max=this.getMaxKey();
      ServiceCl.log(["PrimitiveCollection array defined. max = ",max])

      if(item.key==null){
          max+=1;
          toPsuh=true;
          ServiceCl.log(["item has no key. Max=  ",max])
      }else{
        ServiceCl.log(["item has key: ",item.key])

        if((this.getByItem(item)==null)){
          max+=1;
          toPsuh=true;
          ServiceCl.log(["item not exists. max= ",max])
        }

      }

    }

    if(toPsuh===true){
      item.key=max;
      this.array.push(item);
    }
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
    if((typeof(this.array)!=null)){
      var index_=this.array.findIndex(s=>s.key===item.key);
      if(index_!=-1){
          this.array[-1]=item;
          return this.array[-1];
      }
    }
    return null;
  }

  addUpdate(item:T){
    if((typeof(this.array)!=null)){
      var index_=this.array.findIndex(s=>s.key===item.key);
      ServiceCl.log(index_);
        if(index_!=null){
          ServiceCl.log("Add");
          this.add(item);
        }else{
          ServiceCl.log("Update");
          this.update(item);

        }
    }
  }

  addUpdateArr(items:Array<T>){
    for(var item of items){
      this.addUpdate(item);
    }
  }

  getMaxKey(){
    if(typeof(this.array)!=null){
      var max=Math.max.apply(Math,this.array.map(function(o){return o.key;}))
      if(max!=null){
        return max;
      }
    }
    return null;
  }
  getByItem(item:T){
    if(typeof(this.array)!=null){
      var index_=this.array.findIndex(s=>s.key===item.key);
      if(index_!=-1){
        return this.array[index_];
      }
    }
    return null;
  }
  getByKey(key:number){
    if(typeof(this.array)!=null){
      var index_=this.array.findIndex(s=>s.key===key);
      if(index_!=-1){
        return this.array[index_];
      }
    }
    return null;
  }
  getIndexByItem(item:T){
    if(typeof(this.array)!=null){
      return this.array.findIndex(s=>s.key===item.key);
    }
    return -1;
  }
  getIndexBykey(key:number){
      if(typeof(this.array)!=null){
        return this.array.findIndex(s=>s.key===key);
      }
      return -1;
    }
}

export class ItemCollection implements IItemCollection{
  item:IItem=new ItemG();
  collection:ICollection<IItem>=new Collection<ItemG>();
  constructor(options:{item_?:IItem,collection_?:ICollection<IItem>}={item_:new ItemG(),collection_:new Collection<ItemG>()}){
    this.item=options.item_;
    this.collection=options.collection_;
  }
}

export class Quiz extends ItemCollection{}
export class Question extends ItemCollection{}
export class Answer extends ItemCollection{}

export class QuizC extends ItemG{}
export class QuestionC extends ItemG{}
export class AnswerC extends ItemG{}

export class Factory implements IFactory{

  createItem(): Item{
    let itm:Item= new Item(0,"item1","item2");
    return itm;
  }

  createItemG(): ItemG{
      let itm:ItemG= new ItemG();
      return itm;
    }

}
export class FactoryCollection extends Factory{

  createCollection() :Collection<ItemG> {
    return new Collection<ItemG>();
  }

}
export class FactoryItemColection implements IFactoryItemCollection{
  createItemCollection():IItemCollection{
    return new ItemCollection();
  };
  createQuizCollection():IItemCollection{
    return new Quiz();
  };
  createQuestionCollection():IItemCollection{
    return new Question();
  };
  createAnswerCollection():IItemCollection{
    return new Answer();
  };
}


//option constructors

export class NodeG implements INode{
  key:number;
  name:string;
  value:string;
  constructor(options:{key_:number,name_:string, value_:string}={key_:0,name_ : "",value_:""})
  {
    this.key=options.key_;
    this.name=options.name_;
    this.value=options.value_;
  }
}
export class CollectionG_<T extends NodeG> implements ICollection_<T>{
  array:Array<T>;
  tolog:boolean;
  constructor(options:{array_?:Array<T>}={array_:new Array<T>()}){
      this.array=options.array_;
  }

  add(item:T){
    var max=0;
    var toPsuh:boolean=false;

    if(typeof(this.array)=='undefined'){
      ServiceCl.log("PrimitiveCollection array Undefined")
      this.array=Array<T>();
      toPsuh=true;
    }else{

      max=this.getMaxKey();
      ServiceCl.log(["PrimitiveCollection array defined. max = ",max])

      if(item.key==null){
          max+=1;
          toPsuh=true;
          ServiceCl.log(["item has no key. Max=  ",max])
      }else{
        ServiceCl.log(["item has key: ",item.key])

        if((this.getByItem(item)==null)){
          max+=1;
          toPsuh=true;
          ServiceCl.log(["item not exists. max= ",max])
        }

      }

    }

    if(toPsuh===true){
      item.key=max;
      this.array.push(item);
    }
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
    if((typeof(this.array)!=null)){
      var index_=this.array.findIndex(s=>s.key===item.key);
      if(index_!=-1){
          this.array[-1]=item;
          return this.array[-1];
      }
    }
    return null;
  }

  addUpdate(item:T){
    if((typeof(this.array)!=null)){
      var index_=this.array.findIndex(s=>s.key===item.key);
      ServiceCl.log(index_);
        if(index_!=null){
          ServiceCl.log("Add");
          this.add(item);
        }else{
          ServiceCl.log("Update");
          this.update(item);

        }
    }
  }

  addUpdateArr(items:Array<T>){
    for(var item of items){
      this.addUpdate(item);
    }
  }

  getMaxKey(){
    if(typeof(this.array)!=null){
      var max=Math.max.apply(Math,this.array.map(function(o){return o.key;}))
      if(max!=null){
        return max;
      }
    }
    return null;
  }
  getByItem(item:T){
    if(typeof(this.array)!=null){
      var index_=this.array.findIndex(s=>s.key===item.key);
      if(index_!=-1){
        return this.array[index_];
      }
    }
    return null;
  }
  getByKey(key:number){
    if(typeof(this.array)!=null){
      var index_=this.array.findIndex(s=>s.key===key);
      if(index_!=-1){
        return this.array[index_];
      }
    }
    return null;
  }
  getIndexByItem(item:T){
    if(typeof(this.array)!=null){
      return this.array.findIndex(s=>s.key===item.key);
    }
    return -1;
  }
  getIndexBykey(key:number){
      if(typeof(this.array)!=null){
        return this.array.findIndex(s=>s.key===key);
      }
      return -1;
    }
}

//parameter constructions

export class Node implements INode{
  key:number=0;
  name:string="";
  value:string="";
  constructor(key_?:number,name_?:string, value_?:string)
  {
    if(key_!=null){this.key=key_;}
    if(name_!=null){this.name=name_;}
    if(value_!=null){this.value=value_;}
  }
}
export class Collection_<T extends Node> implements ICollection_<T>{
  array:Array<T>=new Array<T>();
  tolog:boolean=false;

  constructor(array_?:Array<T>){
    if(array_!=null){this.array=array_;}
  }

  add(item:T){
    var max=0;
    var toPsuh:boolean=false;

    if(typeof(this.array)=='undefined'){
      if(this.tolog){
        ServiceCl.log("PrimitiveCollection array Undefined")
      }
      this.array=Array<T>();
      toPsuh=true;
    }else{

      max=this.getMaxKey();
      if(this.tolog){
      ServiceCl.log(["PrimitiveCollection array defined. max = ",max])
      }
      if(item.key==null){
        max+=1;
        toPsuh=true;
        if(this.tolog){
        ServiceCl.log(["item has no key. Max=  ",max])
        }
      }else{
        if(this.tolog){
        ServiceCl.log(["item has key: ",item.key])
        }

        if((this.getByItem(item)==null)){
          max+=1;
          toPsuh=true;
          if(this.tolog){
          ServiceCl.log(["item not exists. max= ",max])
          }
        }

      }

    }

    if(toPsuh===true){
      if(this.tolog){
      ServiceCl.log(["pushing item with key: ",item,max])
      }
      item.key=max;
      this.array.push(item);
    }
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
    if((typeof(this.array)!=null)){
      var index_=this.array.findIndex(s=>s.key===item.key);
      if(index_!=-1){
          this.array[-1]=item;
          return this.array[-1];
      }
    }
    return null;
  }

  addUpdate(item:T){
    if((typeof(this.array)!=null)){
      var index_=this.array.findIndex(s=>s.key===item.key);
      if(this.tolog){
      ServiceCl.log(index_);
        if(index_!=null){
          ServiceCl.log("Add");
          this.add(item);
        }else{
          ServiceCl.log("Update");
          this.update(item);

        }
      }
    }
  }

  addUpdateArr(items:Array<T>){
    for(var item of items){
      this.addUpdate(item);
    }
  }

  getMaxKey(){
    if(typeof(this.array)!=null){
      var max=Math.max.apply(Math,this.array.map(function(o){return o.key;}))
      if(!isFinite(max)){
        ServiceCl.log("Max infinite")
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
      var index_=this.array.findIndex(s=>s.key===item.key);
      if(index_!=-1){
        return this.array[index_];
      }
    }
    return null;
  }
  getByKey(key:number){
    if(typeof(this.array)!=null){
      var index_=this.array.findIndex(s=>s.key===key);
      if(index_!=-1){
        return this.array[index_];
      }
    }
    return null;
  }
  getIndexByItem(item:T){
    if(typeof(this.array)!=null){
      return this.array.findIndex(s=>s.key===item.key);
    }
    return -1;
  }
  getIndexBykey(key:number){
      if(typeof(this.array)!=null){
        return this.array.findIndex(s=>s.key===key);
      }
      return -1;
    }

}

export class NodeCollection extends Node{

  key:number=0;
  name:string="";
  value:string="";
  collection:ICollection_<INodeCollection>;
  constructor(key_?:number,name_?:string, value_?:string,collection_?:ICollection_<INodeCollection>)
  {
    super();
    if(collection_!=null){this.collection=collection_;}
  }

}

export class Factory_{
  node():INodeCollection{
    return new  NodeCollection();
  }

  answers(n:number){
    var answer:ICollection_<Node>=new Collection_<Node>();
    answer.tolog=false;
    for(var i=0;i<n;i++){
      answer.add(new Node(i,"Answer " +i,"Answer " +i));
    }
    return answer;
  }

}
export class Test{

    //obsolette

    public static GenerateItems(bol_:boolean){

        //Check ItemColection

        if(bol_==true){
          ServiceCl.log(["Factory ItemCollection: ",new FactoryItemColection().createItemCollection()]);
          ServiceCl.log(["Factory Quiz: ",new FactoryItemColection().createQuizCollection()]);
          ServiceCl.log(["Factory Question: ",new FactoryItemColection().createQuestionCollection()]);
          ServiceCl.log(["Factory Answer: ",new FactoryItemColection().createAnswerCollection()]);
        }
    }

    public static GenerateAnswers(bol_:boolean){

      if(bol_==true){
        var a_:Collection<AnswerC>=new Collection<AnswerC>();
        var q_:Collection<QuestionC>=new Collection<QuestionC>();

      }

    }

    //NEW

    public static GenNewColl(bol_:boolean){
      ServiceCl.log(["New factory NodeCollection: ",new Factory_().node()])
      ServiceCl.log(["New factory AnswersCollection: ",new Factory_().answers(5)])
    }

    public static GO(){

      Test.GenerateItems(false);
      Test.GenerateAnswers(false);

      Test.GenNewColl(true);

      /*
      //item facory test
      let fct=new Factory();
      ServiceCl.log(["Factory Item: ",fct.createItem()]);
      ServiceCl.log(["Factory ItemG: ",fct.createItemG()]);

      //item anf itemg factory test
      let fctCol=new FactoryCollection();
      let collG:Collection<ItemG>=fctCol.createCollection();
      collG.add(fct.createItemG());
      ServiceCl.log(["Factory CollG: ",collG.array]);

      //itemG collection factory test
      let fctItmColl=new FactoryItemColection();
      ServiceCl.log(["Item: ",new Item()]);
      ServiceCl.log(["ItemG: ",new ItemG()]);
      ServiceCl.log(["ItemCollection: ",new ItemCollection()]);
      */

    }

}
