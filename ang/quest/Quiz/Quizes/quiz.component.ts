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

  @Output() editQuiz_= new EventEmitter<Quiz>();
  @Output() saveQuiz_= new EventEmitter<Quiz>();

  constructor(){

  }

  //to quiz component
  createQuestion(){
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
  getQuiz($event){
    serviceCl.log('getQuiz')
    serviceCl.log($event)
  }
  deleteQuiz(qz_:Quiz)
  {
    serviceCl.log('deleteQuiz')
    serviceCl.log(qz_)
    this.quiz_=null;
    this.editQuiz_.emit(qz_);
  }
  saveQuiz(qz_:Quiz)
  {
    serviceCl.log('saveQuiz')
    serviceCl.log(qz_)
    this.quiz_=null;
    this.saveQuiz_.emit(qz_);
  }
}
