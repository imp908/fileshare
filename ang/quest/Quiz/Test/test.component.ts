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

  quizes_:Quiz[]=[
    new Quiz()
    ,new Quiz()
  ]

  //pass to question
  question_:Qt;
  //pass to answer from question
  answers_:Aw[];


  constructor(){
    console.log('Constructor st: ' + this.constructor.name)

    this.service_=new serviceCl();
    this.className=this.constructor.name;

    //new test answer get
    this.service_.newAnswer();

    //init quiz with test quest
    this.quiz_=this.service_.testQuizExplicit();
    console.log('Quiz');console.log(this.quiz_);
    //serach array by field name value
    console.log(this.service_.getIDByFieldVal(this.quiz_.types.types_,'type','dropdown'))

    if(this.quiz_.selectedQuestion!=null){
      this.question_=this.quiz_.selectedQuestion;
      this.answers_= this.question_.options;
    }
    console.log('Constructor fn: ' + this.constructor.name);
  }

  ngOnInit(){
    console.log('Inited: ' + this.constructor.name);
    console.log(this.className);
    console.log(this.answers_);
    console.log(this.quiz_);
  }


  //to answer component
  addAnswer(a:Aw){
    console.log('addAnswer')
    console.log(a)
    //this.answers_.push(a);
    this.question_.addAnswer(a);

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



  //to quiz component
  createQuestion()
  {
    console.log('createQuestion')
    this.quiz_.selectedQuestion=this.quiz_.newQuestionInit();
  }
  editQuestion(q:Qt){
    console.log('editQuestion')
    console.log(q)
      this.quiz_.selectedQuestion=q;
  }

  saveQuestion($event){
    console.log("savedQuestion($event)")
    console.log($event)
    this.quiz_.addQuestion($event);
    this.quiz_.selectedQuestion=this.quiz_.newQuestionInit();
    console.log(this.quiz_)
  }

  deleteQuestion(q:Qt){
    console.log('deleteQuestion')
    console.log(q)
    this.quiz_.deleteQuestion(q);
  }

}
