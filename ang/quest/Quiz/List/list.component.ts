import { Component, OnInit,Input, Output,EventEmitter }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {answerComponent} from '../Answer/answer.component';

import {Qt,Quiz,serviceCl,IPrimitiveCollection} from '../Model/QtMd.component';

@Component({
  selector: 'list-component'
  ,templateUrl: './list.component.html'
  //,providers:[]
})

export class listComponent
{
  className: string="listComponent";

  service_:serviceCl;

  @Input() quizes_:IPrimitiveCollection<Quiz>;
  _quiz:Quiz;

  constructor(){
    serviceCl.log('Constructor st: ' + this.constructor.name)
    this.service_=new serviceCl();
    serviceCl.log(["quizes get: ",this.quizes_]);
  }

  quizCreate()
  {
    serviceCl.log(["quizCreate"]);
    this._quiz=new Quiz(-1,"");
  }

  quizSend(qz_:Quiz){
    serviceCl.log(["quizSend",qz_]);
    this._quiz=qz_;
  }
  quizDelete($event){
    serviceCl.log(["quizdelete",$event]);
    this.quizes_.delete($event);
    this._quiz=null;
    serviceCl.log(["quizes_: ",this.quizes_]);
  }
  quizSave($event){
    serviceCl.log(["quizSave",$event]);
    this.quizes_.addUpdate($event);
    this._quiz=null;
    serviceCl.log(JSON.stringify(this.quizes_));
    serviceCl.log(["quizes_: ",this.quizes_]);
  }
  click(){
    serviceCl.log(["click"]);
    this._quiz=null;
  }
}
