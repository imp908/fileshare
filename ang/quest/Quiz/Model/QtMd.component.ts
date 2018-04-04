import { Injectable } from '@angular/core';
import { FormGroup,FormControl }  from '@angular/forms';


import { HttpClient, HttpHeaders,HttpEvent } from '@angular/common/http';
import {Observable} from 'rxjs/Observable';

//Generic interface
//---------------------------------------------------------------
interface IPrimitiveCollection_ { <X>(arg:Array<X>):Array<X> }

//Generic PrimitiveCollection
//---------------------------------------------------------------
function PrimitiveCollection_<T>(arg:Array<T>):Array<T>{return arg;}
let myCol1: <U>(arg:Array<U>) => Array<U> = PrimitiveCollection_;
//call signature of and object literal type
let myCol2: { <X>(arg:Array<X>):Array<X> } =PrimitiveCollection_;
//call via interface
let myCol3: IPrimitiveCollection_ = PrimitiveCollection_;


//Generic classes
//---------------------------------------------------------------

export interface IprimitiveItem{
  key:number;
}
export interface IPrimitiveCollection<T extends IprimitiveItem>{
  array:Array<T>;
  delete(item:T);
  update(item:T);
  addUpdate(item:T)
}

class PrimitiveItem{
  key:number;
  name:string="";
  value?:string;
}
class PrimitiveCollection<T extends IprimitiveItem>{
  array:Array<T>;

  add(item:T){
    var max=0;
    var toPsuh:boolean=false;

    if(typeof(this.array)=='undefined'){
      serviceCl.log("PrimitiveCollection array Undefined")
      this.array=Array<T>();
      toPsuh=true;
    }else{

      max=this.getMaxKey();
      serviceCl.log(["PrimitiveCollection array defined. max = ",max])

      if(item.key==null){
          max+=1;
          toPsuh=true;
          serviceCl.log(["item has no key. Max=  ",max])
      }else{
        serviceCl.log(["item has key: ",item.key])

        if((this.getByItem(item)==null)){
          max+=1;
          toPsuh=true;
          serviceCl.log(["item not exists. max= ",max])
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
      serviceCl.log(index_);
        if(index_!=null){
          serviceCl.log("Add");
          this.add(item);
        }else{
          serviceCl.log("Update");
          this.update(item);

        }
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


//Quiz non generic model weak inheritance
//---------------------------------------------------------------

export class Itm{
    public key: number=-1;
    public value: string="";
    public label?: string;
    public options?: any=[];

    constructor(key_:number,value_: string, label_?: string,options_?: any){
      this.key=key_;
      this.value=value_;
      this.label=label_;
      this.options=options_;
    }

}

export class Qt extends Itm{
  toStore:boolean=true;
  type:string="text";

  constructor(key_:number,value_: string, type_:string, toStore_:boolean,label_?: string,aw_?: Aw[]){
    super(key_,value_,label_,aw_);
    this.toStore=toStore_;
    this.type=type_;
  };

  addAnswers(a:Aw[]){
    if(typeof(this.options)=='undefined'){
      this.options=[];
    }
    var max=this.options[this.options.length-1].key;

    for(var i =0;i<a.length;i++)
    {
      a[i].key=max+i;
      this.options.push(a[i]);
    }
  }
  addAnswer(a:Aw){
    var max=-1;
    if(
      (typeof(this.options)=='undefined') ||
      (this.options.length==0)
     ){
      serviceCl.log("Undef")
      this.options=[];
    }
    else{
      serviceCl.log("def")
      serviceCl.log(this.options)
      max=this.options[this.options.length-1].key;
      a.key=max+1;
    }

    this.options.push(a);
  }
  deleteAnswers(a:Aw[]){
    for(var i =0;i<a.length;i++)
    {
      this.options.slice(this.options.findIndex(s=>s.key==a[i].key),1);
    }
  }
  deleteAnswer(a:Aw){
    //var max=this.options.findIndex(s=>s.key==1).key;
    var max=this.findIndex_(this.options,"key",a.key);
    serviceCl.log('delete');
    serviceCl.log(a);
    serviceCl.log('from');
    serviceCl.log(max);
    serviceCl.log('item');
    serviceCl.log(this.options);
    serviceCl.log('equals');
    serviceCl.log(this.options.splice(max,1));
    //this.options.slice(this.options.findIndex(s=>s.key==a.key),1);
    //this.options=this.options.splice(max,1);
  }

  findIndex_(arr:any[],attr:string,val:any){
    for(var i=0; i<arr.length;i++) {
      if(arr[i][attr]===val){
        return i;
      }
    }
    return -1;
  }

}

export class Aw extends Itm{
  isChecked?: boolean=true;
  toStore:boolean=true;

  constructor( key_:number,value_: string,toStore_:boolean,label_?: string,options_?: any,isChecked_?:boolean){
    super(key_,value_,label_,options_);
    this.toStore=toStore_;
    this.isChecked=isChecked_;

  };

}

export class answerType{
  type:string="text";
  name:string="text";
  description:string;

  public constructor(type_:string,name_:string,description_?:string){
    this.type=type_;
    this.name=name_;
    this.description=description_;
  }
}
export class answerTypes{
  selected:answerType=null;

  types_:answerType[]=[
    new answerType("text","Text field","Required text reponse")
    ,new answerType("radiobutton","One answer","select only one answer from one or various")
    ,new answerType("checkbox","Multianswer","select any answers from from one or various")
    ,new answerType("dropdown","List rate","rate every answer from one to various")
  ];

  public constructor(type?:answerType[]){
    this.selected=this.types_[0]
    this.arrCheckInit();
    if(type!=null){
      this.types_=type;
    }
  }

  arrCheckInit(){
    if(this.types_==null){
      this.types_=[];
    }
  }

  add(type:answerType){
    this.arrCheckInit();
    this.types_.push(type);
  }

  remove(type:answerType){
    if(this.types_!=null){
      this.types_.slice(this.findIndex_(this.types_,'type',type),1);
    }
  }

  findByItem(type:answerType)
  {
      return this.types_[this.findIndex_(this.types_,'type',type.type)];
  }
  findByType(type:string)
  {
      return this.types_[this.findIndex_(this.types_,'type',type)];
  }

  bindSelected(type:string)
  {
    this.selected=this.types_[this.findIndex_(this.types_,'type',type)];
  }

  findIndex_(arr:any[],attr:string,val:any){
    for(var i=0; i<arr.length;i++){
      if(arr[i][attr]===val){
        return i;
      }
    }
    return -1;
  }

}

export class Quiz extends PrimitiveItem{

  dateFrom:Date;
  dateTo:Date;
  questions_: Qt[]=[];
  selectedQuestion:Qt=null;
  types: answerTypes=new answerTypes();

  constructor(key_?:number,name_?:string,dateFrom_?:Date,dateTo_?:Date,qt_?:Qt[]){
    super();
    if(name_!=null){
      this.name=name_
    }
    if(key_!=null){
      this.key=key_;
    }
    if(qt_!=null){
      this.questions_=qt_;
    }
  };

  newQuestionInit(){
    return new Qt(-1,"","",true);
  }

  addQuestion(a:Qt){
    var arr:Qt[]=[a];
    this.addQuestions(arr);
  }
  addQuestions(a:Qt[]){
    serviceCl.log("addQuestions")
    var max:number=0;

    for(var i=0;i<a.length;i++){

      //IF exists
      if((this.questions_!=null) && (this.questions_.length>0)){
        var q=getItemFromObjArr(this.questions_,"key",a[i].key);
        serviceCl.log(q);

        if(q!=null){
          //this.deleteQuestion(q);
          this.updateQuestion(q,a[i])
          serviceCl.log(["exists for",a[i]])
        }else{
          max=getMaxID(this.questions_,'key');
          serviceCl.log(["not exists with max: ",max]);
          a[i].key=max+1;
          serviceCl.log("max key: ");serviceCl.log(a[i].key);
          this.questions_.push(a[i]);
        }

      }//if NOT exists PUSH
      else{
        a[i].key=max+1;
        serviceCl.log("max key: ");serviceCl.log(a[i].key);
        this.questions_.push(a[i]);
      }
    }
  }

  deleteQuestion(a:Qt){
    var arr:Qt[]=[a];
    this.deleteQuestions(arr);
  }
  deleteQuestions(a:Qt[]){
    serviceCl.log("deleteQuestions")
    for(var i =0;i<a.length;i++){
      this.questions_.splice(findIndex_(this.questions_,'key',a[i].key),1);
    }
  }

  updateQuestion(a:Qt,b:Qt){
    serviceCl.log(["updateQuestion item with ",a," ",b])
    a=b;
  }

}

export class Quizes{
  key:number=-1;
  quiz_:Quiz;
}




// Service functions for array indexes find and get
//---------------------------------------------------------------

function findIndex_(arr:any[],attr:string,val:any){
  serviceCl.log("findIndex_")
  for(var i=0; i<arr.length;i++){
    if(arr[i][attr]===val){
      return i;
    }
  }
  return -1;
}

//serach array by field name value

function getIDByFieldVal(arr:any[],field:string,val:any){
  serviceCl.log("getMaxID from array by field value")
  serviceCl.log([arr,field,val])
  return arr.findIndex(s=>s[field]===val);
}

//get item from array of objects

function getItemFromObjArr(arr:any[],field:string,val:any){
  serviceCl.log("getItemFromObjArr from array by field value")
  serviceCl.log([arr,field,val])
  return arr[arr.findIndex(s=>s[field]===val)];
}

//

function getMaxID(arr:any[],field:string){
  serviceCl.log("getMaxID from array by field value")
  var max=Math.max.apply(Math,arr.map(function(o){return o[field];}))
  serviceCl.log([arr,field,max]);
  return max;
}


//Service class with toLog booolesan console log and generators
//---------------------------------------------------------------
@Injectable()
export class serviceCl{

  public static toLog:boolean=true;
  public static test:boolean=true;

  public static log(n:any){
    if(serviceCl.toLog===true){
      console.log(n);
    }
  }
  public static run(callback:()=>void){
    var toLogStore=serviceCl.toLog;
    serviceCl.toLog=false;
      callback();
    serviceCl.toLog=toLogStore;
  }

  generateAnswers(n:number){
    var toLogStore=serviceCl.toLog;
    serviceCl.toLog=false;

    serviceCl.log(["generateAnswers for ",n," abs ",Math.abs(n)]);
    var abs=Math.abs(n);
    var a:Aw[]=[];
    for(var i=0;i<abs;i++){
        a.push(new Aw(i,"Answer "+i,true))
    }
    serviceCl.log(["Generated: ",a]);
    serviceCl.toLog=toLogStore;
    return a;
  }
  generateQuestions(n:number,min?:number,max?:number){
    var toLogStore=serviceCl.toLog;
    serviceCl.toLog=false;

    serviceCl.log(["generateAnswers for ",n," abs ",Math.abs(n)]);
    var abs=Math.abs(n);
    var a:Qt[]=[];
    var aw:Aw[]=[];
    for(var i=0;i<abs;i++){
      a.push(new Qt(i,"Question "+i,"text",true,"",this.generateAnswers(2)))
    }
    serviceCl.log(["Generated: ",a]);

    serviceCl.toLog=toLogStore;
    return a;
  }
  genearateQuizes(n:number){
    var toLogStore=serviceCl.toLog;
    serviceCl.toLog=false;

    serviceCl.log(["generateAnswers for ",n," abs ",Math.abs(n)]);
    var qz:Quiz[]=[];
    for(var i=0;i<n;i++){
      qz.push(new Quiz(i,"Quiz "+i,new Date(),new Date(),this.generateQuestions(2)))
    }
    serviceCl.log(["Generated quizes: ",qz]);

    serviceCl.toLog=toLogStore;
    return qz;
  }

  testQuizExplicit(){

    serviceCl.log('started testQuiz')

    let q: Quiz=new Quiz();

    // new Quiz([
    //   new Qt(0,"Quest 1",'dropbox',true,"Input QuestionText",
    //   [
    //     new Aw(1,"Answ 1 ",true,"Insert answer")
    //     ,new Aw(2,"Answ 2 ",true,"Insert answer")
    //   ]
    // )
    //   ,new Qt(3,"Quest 2",'checkbox',true,"Input QuestionText",
    //   [
    //     new Aw(4,"Answ 3 ",true,"Insert answer")
    //     ,new Aw(5,"Answ 4 ",true,"Insert answer")
    //   ])
    //   ,new Qt(6,"Quest 3",'checkbox',true,"Input QuestionText")
    // ]  );

    serviceCl.log(q);

    return q;
  }
  testQuizImplicit(){
    serviceCl.log('testQuizImplicit')
    let q: Quiz = new Quiz();

    q.addQuestions([
      new Qt(0,"Question 1",'text',true,"",[
        new Aw(0,"Aw 1",true)
        ,new Aw(1,"Aw 2",true)
      ])
    ]);

    return q;
  }
  testQuizesExplicit(){
    serviceCl.log('started testQuiz')

    let q: Quiz[]=[
      new Quiz()
    ];

    serviceCl.log(q);

    return q;
  }

  questShema(){
    let q: Qt[]=[
      new Qt(0,"",'text',true,"Question text")
    ];

    return q;
  }
  answerShema(){
    let q: Aw[]=[
      new Aw(0,"",true,"Answer text")
    ];

    return q;
  }

  toControlGroup(){
    const group=new FormGroup({first: new FormControl()});
    return group;
  }
  toControlGroupFM(q_: Qt[]){
    let g:{}={}
    for(let _q of q_){
      g[_q.key]=new FormControl(_q.value);
    }
    return new FormGroup(g);
  }
   toControlGroupOb(q_: Qt[]){
    serviceCl.log('toControlGroupOb');
    let g:{}={}
    for(let _q of q_){
      g[_q.key]= new FormControl({value:_q.value,toStore:_q.toStore});
    }
    serviceCl.log(g);
    return new FormGroup(g);
  }

  static newAnswer():Aw{
    return new Aw(0,"",true,"");
  }
  newQuestion():Qt{
    return new Qt(0,"question text",'text',true,"",[]);
  }
  questTypes(){
    let types:string[]=[
      'text','checkbox','radio button','drop box'
    ]

    return types;
  }
  questTypes_(){
    return new answerTypes();
  }

  quizesArr(){
    var a:Quiz[]=[
      new Quiz(null,"",null,null,[
        new Qt(0,"","text",true,"",[] )
      ])
    ]

    return a;
  }

  //serach array by field name value

  getIDByFieldVal(arr:any[],field:string,val:any){
    return getIDByFieldVal(arr,field,val);
  }

  equalitiesCheck(){

    serviceCl.log(["NaN==NaN",NaN==NaN]);
    serviceCl.log(["NaN===NaN",NaN===NaN]);
    serviceCl.log(["undefined==undefined",undefined==undefined]);
    serviceCl.log(["undefined===undefined",undefined===undefined]);
    serviceCl.log(["undefined==NaN",undefined==NaN]);
    serviceCl.log(["undefined===NaN",undefined===NaN]);
    serviceCl.log(["0==NaN",0==NaN]);
    serviceCl.log(["0==undefined",0==undefined]);

    var x=[];var y=[];
    serviceCl.log(["var x=[];var y=[];var a=x==y;",x==y]);
    serviceCl.log(["var x=[];var y=[];var a=x===y;",x===y]);

    var num = 0;
    var obj = new String("0");
    var str = "0";
    var b = false;

    serviceCl.log(["",num == num]); // true
    serviceCl.log(obj == obj); // true
    serviceCl.log(str == str); // true

    //NOT in TYPESCRIPT
    // serviceCl.log(num == obj); // true
    // serviceCl.log(num == str); // true

    serviceCl.log(obj == str); // true
    serviceCl.log(null == undefined); // true

    // оба false, кроме очень редких случаев
    serviceCl.log(obj == null);
    serviceCl.log(obj == undefined);

  }

  genericFucnrionsCall(){
    var toLogStore=serviceCl.toLog;
    serviceCl.toLog=false;

    serviceCl.log(PrimitiveCollection_([1,2,3]));
    serviceCl.log(PrimitiveCollection_(["a","1",2]));

    serviceCl.log(myCol1([1,2,3]));
    serviceCl.log(myCol2(["a","1",2]));

    serviceCl.log(myCol3([1,2,3]));
    serviceCl.log(myCol3(["a","1",2]));

    serviceCl.toLog=toLogStore;
  }
  generciClassesCall(){
    var toLogStore=serviceCl.toLog;
    serviceCl.toLog=false;

    var col:PrimitiveCollection<PrimitiveItem>=new PrimitiveCollection<PrimitiveItem>();
    col.add({key:0,name:"item1"});
    col.add({key:1,name:"item2",value:"val1"});
    col.add({key:1,name:"item3",value:"val2"});
    serviceCl.log(col);

    serviceCl.toLog=toLogStore;
  }
  genericQuizCollection(){
    var toLogStore=serviceCl.toLog;

    var col:PrimitiveCollection<Quiz>=new PrimitiveCollection();
    var arr:Quiz[]=this.genearateQuizes(3);

    for(var q of arr){
      serviceCl.log(arr);
      col.add(q);
    }

    col.add(this.genearateQuizes(1)[0]);

    var qz:Quiz[]=[
      new Quiz(2,"Quiz 2 duplicate")
      ,new Quiz(4,"Quiz 4 duplicate")
      ,new Quiz(4,"Quiz 4 duplicate")
    ]

    for(let q_ of qz){
      col.add(q_);
    }

    serviceCl.log(["Quiz list:",col]);
    serviceCl.toLog=toLogStore;
    return col;
  }

  test(){
    var ln=
    "--------------------------------------------------------------------";
    serviceCl.log(ln)

    this.genericQuizCollection();

    serviceCl.log(ln)
  }

}
