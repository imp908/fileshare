import { Component, OnInit,Input, Output,EventEmitter }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {answerComponent} from '../Answer/answer.component';

import {Qt,Quiz,serviceCl} from '../Model/QtMd.component';

@Component({
  selector: 'quiz-component'
  ,templateUrl: './quiz.component.html'
  //,providers:[]
})


export class quizComponent
{
  className: string="quizesComponent";

  service_:serviceCl;

  @Input() quiz_:Quiz;

  constructor(){

  }

  //to quiz component
  createQuestion(){
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
