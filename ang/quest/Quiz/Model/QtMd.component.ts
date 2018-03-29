import { Injectable } from '@angular/core';
import { FormGroup,FormControl }  from '@angular/forms';

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
      console.log("Undef")
      this.options=[];
    }
    else{
      console.log("def")
      console.log(this.options)
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
    console.log('delete');
    console.log(a);
    console.log('from');
    console.log(max);
    console.log('item');
    console.log(this.options);
    console.log('equals');
    console.log(this.options.splice(max,1));
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

export class Quiz{
  questions_: Qt[]=[];
  selectedQuestion:Qt=null;
  types: answerTypes=new answerTypes();

  constructor( qt_?:Qt[],types_?:answerTypes){
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
    console.log("addQuestions")
    var max:number=0;

    for(var i=0;i<a.length;i++){

      //IF exists
      if((this.questions_!=null) && (this.questions_.length>0)){
        var q=getItemFromObjArr(this.questions_,"key",a[i].key);
        console.log(q);

        if(q!=null){
          //this.deleteQuestion(q);
          this.updateQuestion(q,a[i])
          console.log("exists for",a[i])
        }else{
          max=getMaxID(this.questions_,'key');
          console.log("not exists with max: ",max);
          a[i].key=max+1;
          console.log("max key: ");console.log(a[i].key);
          this.questions_.push(a[i]);
        }

      }//if NOT exists PUSH
      else{
        a[i].key=max+1;
        console.log("max key: ");console.log(a[i].key);
        this.questions_.push(a[i]);
      }
    }
  }

  deleteQuestion(a:Qt){
    var arr:Qt[]=[a];
    this.deleteQuestions(arr);
  }
  deleteQuestions(a:Qt[]){
    console.log("deleteQuestions")
    for(var i =0;i<a.length;i++){
      this.questions_.splice(findIndex_(this.questions_,'key',a[i].key),1);
    }
  }

  updateQuestion(a:Qt,b:Qt){
    console.log("updateQuestion item with ",a," ",b)
    a=b;
  }

}


function findIndex_(arr:any[],attr:string,val:any){
  console.log("findIndex_")
  for(var i=0; i<arr.length;i++){
    if(arr[i][attr]===val){
      return i;
    }
  }
  return -1;
}

//serach array by field name value

function getIDByFieldVal(arr:any[],field:string,val:any){
  console.log("getMaxID from array by field value")
  console.log(arr,field,val)
  return arr.findIndex(s=>s[field]===val);
}

//get item from array of objects

function getItemFromObjArr(arr:any[],field:string,val:any){
  console.log("getItemFromObjArr from array by field value")
  console.log(arr,field,val)
  return arr[arr.findIndex(s=>s[field]===val)];
}

//

function getMaxID(arr:any[],field:string){
  console.log("getMaxID from array by field value")
  var max=Math.max.apply(Math,arr.map(function(o){return o[field];}))
  console.log(arr,field,max);
  return max;
}

@Injectable()
export class serviceCl{

  testQuizExplicit(){

    console.log('started testQuiz')

    let q: Quiz=new Quiz();
/*
    new Quiz([
      new Qt(0,"Quest 1",'dropbox',true,"Input QuestionText",
      [
        new Aw(1,"Answ 1 ",true,"Insert answer")
        ,new Aw(2,"Answ 2 ",true,"Insert answer")
      ]
    )
      ,new Qt(3,"Quest 2",'checkbox',true,"Input QuestionText",
      [
        new Aw(4,"Answ 3 ",true,"Insert answer")
        ,new Aw(5,"Answ 4 ",true,"Insert answer")
      ])
      ,new Qt(6,"Quest 3",'checkbox',true,"Input QuestionText")
    ]  );
*/
    console.log(q);

    return q;
  }
  testQuizImplicit(){
    console.log('testQuizImplicit')
    let q: Quiz = new Quiz();
    q.addQuestions([
      new Qt(0,"Question 1",'text',true,"")
    ]);
    return q;
  }
  testQuizesExplicit(){
    console.log('started testQuiz')

    let q: Quiz[]=[new Quiz()];

    console.log(q);

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
    console.log('toControlGroupOb');
    let g:{}={}
    for(let _q of q_){
      g[_q.key]= new FormControl({value:_q.value,toStore:_q.toStore});
    }
    console.log(g);
    return new FormGroup(g);
  }

  newAnswer():Aw{
    return new Aw(0,"answer text",true,"");
  }
  newQuestion():Qt{
    return new Qt(0,"question text",'text',true,"",new []);
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
      new Quiz([
        new Qt(0,"","text",true,[
          new
        ])
      ])
    ]

    return a;
  }

  //serach array by field name value

  getIDByFieldVal(arr:any[],field:string,val:any){
    return getIDByFieldVal(arr,field,val);
  }

}

export class TestCl{
  printAwCl(){
    let a=new Aw(0,"",true,"Quest text",);
    console.log(a);
  }
}
