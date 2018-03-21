interface IPerson{
  firstName: string;
  lastName: string;
}

export {IPerson};




interface IQuestionType {
  ID:number;
  type:string[];
}

class QuestionType{
  ID:number;
  type:string[];

  constructor(public ID_:number,public type_:string[]){
    this.ID=ID_;
    this.type=type_;
  }
}

export class QuestionExport{
  getQuestions(){

    let questions= new QuestionType( 0, ['a','b','c']);

    return questions;
  }
}

export {IQuestionType};
