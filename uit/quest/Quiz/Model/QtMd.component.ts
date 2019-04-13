import { Injectable } from '@angular/core';
import { FormGroup,FormControl }  from '@angular/forms';

export class Itm{
    key: number;
    value: string;
    label?: string;
    options?: any;

    constructor( key_: number, value_: string, label_?: string,options_?: any){
      this.key=key_;
      this.value=value_;
      this.label=label_;
      this.options=options_;
    }
}


export class Qt extends Itm{
  toStore:boolean;
  constructor( key_: number, value_: string,toStore_:boolean, label_?: string,options_?: any){
    super(key_,value_,label_,options_);
      this.toStore=toStore_;
  };

}

export class Aw extends Qt{
  isChecked?: boolean;

  constructor( key_: number, value_: string,toStore_:boolean, label_?: string,options_?: any,isChecked_?:boolean){
    super(key_,value_,toStore_,label_,options_);
    this.isChecked=isChecked_;

  };
}


export class Quiz
{
  Qts: Qt[];

    constructor( qt_?:Qt[]){
      this.Qts=qt_;
    };
}


class QtTxt extends Qt{

  options: Itm[];

  constructor( key_: number, value_: string, toStore_:boolean,label_?: string,options_?: any){
    super(key_,value_,toStore_,label_,options_);
    this.options=options_;
  };
}

@Injectable()
export class serviceCl
{
  testQuiz(){
    console.log('started testQuiz')

    let q: Quiz=new Quiz([
      new Qt(0,"Quest 1",true,"Input QuestionText",
      [
        new Aw(0,"Answ 1 ",true,"Insert answer")
        ,new Aw(1,"Answ 2 ",true,"Insert answer")
      ]
    )
      ,new Qt(1,"Quest 2",true,"Input QuestionText",
      [
        new Aw(2,"Answ 3 ",true,"Insert answer")
        ,new Aw(3,"Answ 4 ",true,"Insert answer")
      ])
    ]);

    console.log(q);

    return q;
  }
  questShema()
  {
    let q: Qt[]=[
      new Qt(0,"",true,"Question text")
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
    let g:any={};
    for(let _q of q_){
      g[_q.key]=new FormControl(_q.value);
    }
    return new FormGroup(g);
  }

}

export class TestCl
{
  printAwCl(){
    let a=new Aw(0,"",true,"Quest text",);
    console.log(a);
  }
}
