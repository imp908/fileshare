import { Component, Input, OnInit,Output, EventEmitter }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {Aw} from '../Model/QtMd.component';

@Component({
  selector: 'answer-component'
  ,templateUrl: './answer.component.html'
  //,providers:[]
})

export class answerComponent
{
  className: string;
  @Input() answer_: Aw;
  @Output() deleteAnswer_=new EventEmitter<Aw>();

  constructor(){
    this.className=this.constructor.name;
    this.answer_=new Aw(0,"",true,"test answer");
  }

  ngOnInit(){
    console.log('Inited: ' + this.constructor.name)
    console.log(this.answer_);
  }

  deleteAnswer(a:Aw)
  {
    console.log("deleteAnswer");
    console.log(a);
    this.deleteAnswer_.emit(a);
  }
}
