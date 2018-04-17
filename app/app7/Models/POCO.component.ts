
export interface IItem{
  key:number;
  name:string;
  value:string;
}
export interface ICollection<T extends IItem>{
  array:Array<T>;
  add(item:T);
  delete(item:T);
  update(item:T);
  addUpdate(item:T);
  addUpdateArr(items:Array<T>);
}
export interface IItemCollection{
  item:IItem;
  coletcion:ICollection<IItem>;
}

export interface IFactory{
  createItem():IItem;
}

//export interface IFactoryG<T extends IItem>{ createItem():T;}
