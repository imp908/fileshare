import { Injectable } from '@angular/core';
import { FormGroup,FormControl }  from '@angular/forms';

export class Itm{
    public key: number;
    public value: string;
    public label?: string;
    public options?: any;

    constructor(key_:number,value_: string, label_?: string,options_?: any){
      this.key=key_;
      this.value=value_;
      this.label=label_;
      this.options=options_;
    }

}

export class Qt extends Itm{
  toStore:boolean;
  type:string;
  constructor(key_:number,value_: string, type_:string, toStore_:boolean,label_?: string,aw_?: Aw[]){
    super(key_,value_,label_,aw_);
    this.toStore=toStore_;
    this.type=type_;
  };

  addAnswers(a:Aw[])
  {
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
  addAnswer(a:Aw)
  {
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
    for(var i=0; i<arr.length;i++)
    {
      if(arr[i][attr]===val){
        return i;
      }
    }
    return -1;
  }

}

export class Aw extends Itm{
  isChecked?: boolean;
  toStore:boolean;

  constructor( key_:number,value_: string,toStore_:boolean,label_?: string,options_?: any,isChecked_?:boolean){
    super(key_,value_,label_,options_);
    this.toStore=toStore_;
    this.isChecked=isChecked_;

  };
}


export class Quiz
{
  Qts: Qt[];

    constructor( qt_?:Qt[]){
      this.Qts=qt_;
    };

    addQuestions(a:Qt[])
    {
      var max=this.Qts[this.Qts.length-1].key;

      for(var i =0;i<a.length;i++)
      {
        a[i].key=max+i;
        this.Qts.push(a[i]);
      }
    }

    deeleteQuestions(a:Qt[]){
      for(var i =0;i<a.length;i++)
      {
        var b=this.Qts.findIndex(s=>s.key==a[i].key);
      }
    }
}



@Injectable()
export class serviceCl
{
  testQuizExplicit(){
    console.log('started testQuiz')

    let q: Quiz=new Quiz([
      new Qt(0,"Quest 1",'text',true,"Input QuestionText",
      [
        new Aw(1,"Answ 1 ",true,"Insert answer")
        ,new Aw(2,"Answ 2 ",true,"Insert answer")
      ]
    )
      ,new Qt(3,"Quest 2",'text',true,"Input QuestionText",
      [
        new Aw(4,"Answ 3 ",true,"Insert answer")
        ,new Aw(5,"Answ 4 ",true,"Insert answer")
      ])
    ]);

    console.log(q);

    return q;
  }
  testQuizImplicit()
  {
    console.log('testQuizImplicit')
    let q: Quiz = new Quiz();
    q.addQuestions([
      new Qt(0,"Question 1",'text',true,"")
    ]);
    return q;
  }
  questShema()
  {
    let q: Qt[]=[
      new Qt(0,"",'text',true,"Question text")
    ];

    return q;
  }

  answerShema()
  {
    let q: Aw[]=[
      new Aw(0,"",true,"Answer text")
    ];

    return q;
  }

  toControlGroup()
  {
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

  newAnswer():Aw
  {
    return new Aw(0,"answer text",true,"");
  }
  newQuestion():Qt
  {
    return new Qt(0,"question text",'text',true,"",[]);
  }
  questTypes()
  {
    let types:string[]=[
      'text','checkbox','radio button','drop box'
    ]

    return types;
  }
}

export class TestCl
{
  printAwCl(){
    let a=new Aw(0,"",true,"Quest text",);
    console.log(a);
  }
}
