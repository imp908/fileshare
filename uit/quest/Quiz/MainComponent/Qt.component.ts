import { Component, Input, Output, OnInit, EventEmitter }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import  {Qt,serviceCl} from '../Model/QtMd.component'



@Component({
  selector: 'qt-component'
  ,templateUrl: './qt.component.html'
  //,providers:[]
})

export class qtComponent
{
  mcText: string;
  question_: Qt;
  from_: FormGroup;
  service_: serviceCl;

  @Output() questionEmt=new EventEmitter<Qt>();

  ngOnInit(){
    this.service_=new serviceCl();

    this.from_=this.service_.toControlGroup();
    console.log('Inited: ' + this.constructor.name)
  }

  receiveAnswers($event){
    console.log('receiveanswers');
    this.question_.options=$event;
  }

  saveQuestion(){
    this.questionEmt.emit(this.question_);
  }

}
