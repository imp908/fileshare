
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
