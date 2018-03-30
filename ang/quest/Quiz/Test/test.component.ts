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
  //default
  className: string;
  service_:serviceCl;

  quiz_:Quiz=new Quiz();

  _quizes:Quiz[]=[];

  //pass to question
  question_:Qt;
  //pass to answer from question
  answers_:Aw[];


  constructor(){
    serviceCl.toLog =false;

    serviceCl.log('Constructor st: ' + this.constructor.name)
    this.service_=new serviceCl();

    this.service_.test();

    this.className=this.constructor.name;

    //new test answer get
    serviceCl.newAnswer();

    //init quiz with test quest
    this.quiz_=this.service_.testQuizExplicit();
    serviceCl.log('Quiz');serviceCl.log(this.quiz_);
    //serach array by field name value
    serviceCl.log(this.service_.getIDByFieldVal(this.quiz_.types.types_,'type','dropdown'))

    if(this.quiz_.selectedQuestion!=null){
      this.question_=this.quiz_.selectedQuestion;
      this.answers_= this.question_.options;
    }
    serviceCl.log('Constructor fn: ' + this.constructor.name);
  }

  ngOnInit(){
    serviceCl.log('Inited: ' + this.constructor.name);
    serviceCl.log(this.className);
    serviceCl.log(this.answers_);
    serviceCl.log(this.quiz_);
  }


  //to answer component
  addAnswer(a:Aw){
    serviceCl.log('addAnswer')
    serviceCl.log(a)
    //this.answers_.push(a);
    this.question_.addAnswer(a);

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



  //to quiz component
  createQuestion()
  {
    serviceCl.log('createQuestion')
    this.quiz_.selectedQuestion=this.quiz_.newQuestionInit();
  }
  editQuestion(q:Qt){
    serviceCl.log('editQuestion')
    serviceCl.log(q)
      this.quiz_.selectedQuestion=q;
  }

  saveQuestion($event){
    serviceCl.log("savedQuestion($event)")
    serviceCl.log($event)
    this.quiz_.addQuestion($event);
    this.quiz_.selectedQuestion=this.quiz_.newQuestionInit();
    serviceCl.log(this.quiz_)
  }

  deleteQuestion(q:Qt){
    serviceCl.log('deleteQuestion')
    serviceCl.log(q)
    this.quiz_.deleteQuestion(q);
  }

}
