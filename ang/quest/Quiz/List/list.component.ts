import { Component, OnInit,Input, Output,EventEmitter }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {answerComponent} from '../Answer/answer.component';

import {Qt,Quiz,serviceCl} from '../Model/QtMd.component';

@Component({
  selector: 'list-component'
  ,templateUrl: './list.component.html'
  //,providers:[]
})

export class listComponent
{
  className: string="listComponent";

  service_:serviceCl;

  @Input() quizes_:Quiz[];

  constructor(){
    
  }

}
