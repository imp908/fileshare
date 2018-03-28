
import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {QuestionTypes,QuestionItems,QuestionType,QuestionItem}   from '../questionMd.component';

@Component({
  selector: 'question-type'
  ,templateUrl: './QuestionTp.component.html'
  ,providers:[QuestionTypes,QuestionItems]
})


export class QuestionComponents{
  questTypes: QuestionType[];
  serv: QuestionTypes;

  qi:QuestionItems;
  questionItems: QuestionItem[];
  qitm:QuestionItem;

  form: FormGroup;
  selectedType: string;
  jsString: string;

  constructor(){
    this.serv=new QuestionTypes();
    this.qi=new QuestionItems();

    //question type names for dropdown select
    this.questTypes=this.serv.getQuestionTypes();
    //collection of question objects
    this.questionItems=this.qi.getQuestionItems();

    this.form=this.toFormGroup();
    console.log(this.questTypes);
    console.log(this.questionItems);
  }

  toFormGroup()
  {
    console.log("toFormGroup");
    console.log(this.questTypes);
    const group=this.serv.toControlGroup();
    return group;

  }

  onChange(value: string)
  {
    console.log("onChange");
    this.selectedType=value;

    console.log(this.questionItems);
    console.log(value);

    this.qitm=this.questionItems.filter(x=> x.type.name==value )[0];
    console.log(this.qitm);
    console.log(this.selectedType);
    console.log(this.qitm.type.type_);
  }
  ngOnInit(){

  }
  onSubmit() {
    console.log("onSubmit");
    this.jsString = JSON.stringify(this.form.value);
  }



}
