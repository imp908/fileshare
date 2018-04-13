import { Component, OnInit,Input, Output,EventEmitter } from '@angular/core';
import { FormGroup,FormControl } from '@angular/forms';

import {answerComponent} from '../Answer/answer.component';

import {HS} from '../Http/quiz.service';

import {serviceCl,Quiz,IPrimitiveCollection,PrimitiveCollection} from '../Model/QtMd.component';


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
  qz:Quiz;
  //_quizes:Quiz[];
  _quizes:IPrimitiveCollection<Quiz>;


  constructor (private hs_:HS){
      this.className=this.constructor.name;
      this._quizes=new PrimitiveCollection<Quiz>();
  }

  GetQuiz(){
      serviceCl.log("GetQuiz");

      //not calls any method
      serviceCl.log(['Quizes before ',this._quizes]);

      this.hs_.getQuizResponse3(this.url_)
      .subscribe(r=>{console.log(r); this._quizes.array=r;})
      ;

      //test
      //this._quizesSend=this.service.genericQuizCollection();

      serviceCl.log(['Quizes after ',this._quizes]);
  }

  AddQuiz(){
      serviceCl.log("AddQuiz");

      //test
      //this._quizesSend=this.service.genericQuizCollection();

      this.hs_.quizPost(this.url_,this._quizes);
      serviceCl.log(["Quiz",this._quizes, "to ",this.url_]);
  }

}
