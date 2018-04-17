import {ServiceCl} from '../Services/services.component';

import {IItem,IFactory,ICollection} from './POCO.component';


export class Item implements IItem {
  key:number;
  name:string;
  value:string;
  constructor(key_:number,name_:string, value_:string){
    this.key=key_;
    this.name=name_;
    this.value=value_;
  }
}
export class ItemG implements IItem{
  key:number;
  name:string;
  value:string;
  constructor(options:{key_:number,name_:string, value_:string}={key_:0,name_ : "default name",value_:"default value"})
  {
    this.key=options.key_;
    this.name=options.name_;
    this.value=options.value_;
  }
}
export class Collection<T extends IItem>{
    array:Array<T>;

    constructor(){
        this.array=new Array<T>();
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

export class Test{

    public static GO(){
        let fct=new Factory();
        ServiceCl.log(["Factory Item: ",fct.createItem()]);
        ServiceCl.log(["Factory ItemG: ",fct.createItemG()]);

        let fctCol=new FactoryCollection();
        let collG:Collection<ItemG>=fctCol.createCollection();
        collG.add(fct.createItemG());
        ServiceCl.log(["Factory CollG: ",collG.array]);

    }

}
