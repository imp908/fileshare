import { Component, OnInit,Input, Output,EventEmitter } from '@angular/core';
import { FormGroup,FormControl } from '@angular/forms';

import {answerComponent} from '../Answer/answer.component';

import {HS} from '../Http/quiz.service';

import {serviceCl,Quiz} from '../Model/QtMd.component';


@Component({
  selector: 'httpClick-component'
  ,templateUrl: './httpClick.component.html'
  //,providers:[]
  ,providers: [ HS ]
})

export class httpClick
{
  className: string;
  url_:string="";
  service:serviceCl= new serviceCl();
  _quizGet:any;

  constructor (private hs_:HS){
    this.className=this.constructor.name;

  }

  GetQuiz(){
      serviceCl.log("GetQuiz");
      this._quizGet=this.hs_.getQuizes(this.url_);
      serviceCl.log(['Quizes HS ',this._quizGet]);
  }

}
