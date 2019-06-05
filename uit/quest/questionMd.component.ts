import { Injectable }       from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

interface IQuestionType {
  ID:number;
  name:string[];
}

export class QuestionType{
  ID:number;
  name:string;

  constructor(public ID_:number,public type_:string){
    this.ID=ID_;
    this.name=type_;
  }
}


export class QuestionItem
{
  ID:number;
  type:QuestionType;
  AnswerText?:string;
  AnswerItem?:any;

  constructor(  id_:number,
    type_:QuestionType,
    text_?:string,
    answer_?:any){
      this.ID=id_;
      this.type=type_;
      this.AnswerText=text_;

    }

}

@Injectable()
export class QuestionTypes{
  getQuestionTypes(){

    let questions: QuestionType[]=[
      new QuestionType(0,"Text box")
      ,new QuestionType(1,"Drop down")
      ,new QuestionType(2,"Check box")
      ,new QuestionType(3,"Radio buttons")
    ];

    return questions;
  }
  toControlGroup()
  {
    const group=new FormGroup({first: new FormControl(this.getQuestionTypes())});
    return group;

  }
}

@Injectable()
export class QuestionItems{
  getQuestionItems(){
    let q: QuestionItem[]=[
      new QuestionItem(0,new QuestionType(0,"Text box"),undefined)
      ,new QuestionItem(1,new QuestionType(0,"Drop down"),undefined)
      ,new QuestionItem(2,new QuestionType(0,"Check box"),undefined)
      ,new QuestionItem(3,new QuestionType(0,"Radio buttons"),undefined)
    ]
    return q;
  }
  toControlGroup()
  {
    const group=new FormGroup({first: new FormControl(this.getQuestionItems())});
    return group;

  }
}

//export {IQuestionType,QuestionType,QuestionExport};
