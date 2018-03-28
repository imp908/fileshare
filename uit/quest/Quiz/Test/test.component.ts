import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {answerComponent} from '../Answer/answer.component';

import {Aw,Qt,Quiz,serviceCl} from '../Model/QtMd.component';


@Component({
  selector: 'test-component'
  ,templateUrl: './test.component.html'
  //,providers:[]
})

export class testComponent
{
  className: string;

  service_:serviceCl;
  quiz_:Quiz;
  question_:Qt;
  answers_:Aw[];
  newAnswer_:Aw;

  constructor(){
    console.log('Constructor st: ' + this.constructor.name)

    this.service_=new serviceCl();
    this.className=this.constructor.name;

    this.newAnswer_=this.service_.newAnswer();

    this.quiz_=this.service_.testQuizExplicit();
    this.question_=this.quiz_.Qts[0];
    this.answers_= this.question_.options;
    console.log('Constructor fn: ' + this.constructor.name)
  }

  ngOnInit(){
    console.log('Inited: ' + this.constructor.name)
    console.log(this.className);
    console.log(this.answers_);
    console.log(this.newAnswer_);
    console.log(this.quiz_);
  }

  addAnswer(a:Aw){
    console.log('addAnswer')
    console.log(a)
    //this.answers_.push(a);
    this.question_.addAnswer(a);
    this.newAnswer_=this.service_.newAnswer();
    console.log(this.question_.options);
  }

  deleteAnswerListen($event){
    console.log('deleteAnswer')
    console.log($event)
    console.log($event.key)
    console.log(this.answers_)
    this.question_.deleteAnswer($event);
    console.log(this.question_.options);
  }
}
