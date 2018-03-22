import { Injectable }       from '@angular/core';

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
  text?:string;
  answer?:any;

  constructor(  id_:number,
    type_:QuestionType,
    text_?:string,
    answer_?:any){
      this.ID=id_;
      this.type=type_;
      this.text=text_;
      this.answer=answer_;
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
}

@Injectable()
export class QuestionItems{
  getQuestionItems(){
    let q: QuestionItem[]=[

      new QuestionItem(0,new QuestionType(0,"Text box"),undefined,undefined)
      ,new QuestionItem(0,new QuestionType(0,"Drop down"),undefined,undefined)

    ]

    return q;
  }
}
//export {IQuestionType,QuestionType,QuestionExport};
