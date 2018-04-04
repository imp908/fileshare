import { Component, OnInit,Input, Output,EventEmitter }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {answerComponent} from '../Answer/answer.component';

import {Aw,Qt,Quiz,serviceCl,answerTypes} from '../Model/QtMd.component';


@Component({
  selector: 'question-component'
  ,templateUrl: './question.component.html'
  //,providers:[]
})

export class questionComponent
{
  className: string;

  service_:serviceCl;

  @Input() question_:Qt;
  @Output() saveQuestion_= new EventEmitter<Qt>();

  answers_:Aw[];
  newAnswer_:Aw;

  answerTypes_:answerTypes=new answerTypes();

  constructor(){
    serviceCl.log('Constructor st: ' + this.constructor.name)
    this.service_=new serviceCl();
    this.className=this.constructor.name;

    if(
      (typeof(this.question_)!='undefined') &&
      (typeof(this.question_.options)!='undefined')
    ){
      serviceCl.log('question options')
      this.answers_= this.question_.options;
      serviceCl.log('Question:' + this.question_)
      serviceCl.log('answer' + this.question_.options)

    }

    this.newAnswer_=serviceCl.newAnswer();
    serviceCl.log('questTypes: '); serviceCl.log(this.answerTypes_)
    serviceCl.log('Constructor fn: ' + this.constructor.name)
  }

  ngOnInit(){
    serviceCl.log('Inited: ' + this.constructor.name)
    serviceCl.log(this.className);
    serviceCl.log(this.answers_);
    serviceCl.log(this.newAnswer_);

  }

  addAnswer(a:Aw){
    serviceCl.log('addAnswer')
    serviceCl.log(a)
    //this.answers_.push(a);
    this.question_.addAnswer(a);
    this.newAnswer_=serviceCl.newAnswer();
    serviceCl.log(this.question_.options);
  }

  deleteAnswerListen($event){
    serviceCl.log('deleteAnswer')
    serviceCl.log($event)
    serviceCl.log($event.key)
    serviceCl.log(this.answers_)
    this.question_.deleteAnswer($event);
    serviceCl.log(this.question_.options);
  }

  saveQuestion(q:Qt){
    serviceCl.log('saveQuestion')
    serviceCl.log(q)
    this.saveQuestion_.emit(q);
  }

  typeChange(s:string){
    serviceCl.log('typeChange'+ s)
    this.answerTypes_.bindSelected(s);
  }

}
