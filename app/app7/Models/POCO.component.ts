
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

  getMaxKey();
  getByItem(item:T);
  getByKey(key:number);
  getIndexByItem(item:T);
  getIndexBykey(key:number);

  isUndefined(arr_:Array<T>):boolean;

}

export interface INodeCollection extends INode{
  key:number;
  name:string;
  value:string;
  parentKey:number;
  collection:ICollection_<INodeCollection>;
}
