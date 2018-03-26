import { Component, Input, Output, OnInit, EventEmitter }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import  {Aw,serviceCl} from '../Model/QtMd.component'



@Component({
  selector: 'aw-component'
  ,templateUrl: './aw.component.html'
  //,providers:[]
})

export class awComponent
{
  mcText: string;
  answer_: Aw[];
  from_: FormGroup;
  service_: serviceCl;

  @Output() answersCollection=new EventEmitter<Aw[]>();

  ngOnInit(){
    this.service_=new serviceCl();
    this.from_=this.service_.toControlGroup();
    console.log('Inited: ' + this.constructor.name)
  }

  saveAnswers(){
    console.log('saveAnswers');
    this.answersCollection.emit(this.answer_);
  }

}
