import { Injectable } from '@angular/core';
import { FormGroup,FormControl }  from '@angular/forms';

class Aw{
  AnswerText: string;
  isChecked? : bool;
  constructor(aw:string,ich?:bool){};
}

class Qt{
  QuestionText: string;
  qType: string;
  answers: any;

  constructor(qTxt:string,qTp:string,answ:any){};
}

class Quiz
{
  Qts: Qt[];
}


class QtTxt extends Qt{

}


export class TestCl
{
  printAwCl(){
    let a=new Aw("Quest text",true);
    console.log(a);
  }
}
