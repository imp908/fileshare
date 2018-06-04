
//NEW

export interface INode{
  _key:number;
  _name:string;
  _value:string;
  typeName:string;
}
export interface ICollection_<T>{
  tolog:boolean;
  array:Array<T>;
  type_:string;

  add(item:T);
  delete(item:T);
  update(item:T);
  addUpdate(item:T);
  addUpdateArr(items:Array<T>);

  getMaxKey();
  getByItem(item:T);
  getByName(item:string);
  getByKey(key:number);
  getIndexByItem(item:T);
  getIndexBykey(key:number);

  isUndefined(arr_:Array<T>):boolean;
  getType();
  setType(type_:string);

  sortAsc(a:T,b:T);
  sortDesc(a:T,b:T);
  sort(asc:boolean);

}

export interface INodeCollection extends INode{

  _key:number;
  _name:string;
  _value:string;
  parentKey:number;
  collection:ICollection_<INodeCollection>;
  getType_();
  sortHierarhy(asc:boolean);
  scan(name_:string,col_:INodeCollection);
  findInParams(name_:string,col_:INodeCollection,ret_:INodeCollection);
  shallowCopy();
  _sliceArr(nc:INodeCollection);

}
