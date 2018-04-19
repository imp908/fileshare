
//obsolette

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
  collection:ICollection<IItem>;
}
export interface IItemCollectionG<T extends IItem>{
  item:T;
  collection:ICollection<T>;
}

export interface IFactory{
  createItem():IItem;
}
export interface IFactoryItemCollection{
  createItemCollection():IItemCollection;
}

//export interface IFactoryG<T extends IItem>{ createItem():T;}





//NEW

export interface INode{
  key:number;
  name:string;
  value:string;
}
export interface ICollection_<T>{
  tolog:boolean;
  array:Array<T>;
  add(item:T);
  delete(item:T);
  update(item:T);
  addUpdate(item:T);
  addUpdateArr(items:Array<T>);
}
export interface INodeCollection extends INode{
  key:number;
  name:string;
  value:string;
  collection:ICollection_<INodeCollection>;
}
