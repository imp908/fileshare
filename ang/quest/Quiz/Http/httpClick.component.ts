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
  _quizesSend:IPrimitiveCollection<Quiz>;
  constructor (private hs_:HS){
      this.className=this.constructor.name;
      this._quizes=new PrimitiveCollection<Quiz>();
  }

  GetQuiz(){
      serviceCl.log("GetQuiz");

      //not calls any method
      serviceCl.log(['Quizes before ',this._quizes]);

      //old CALL async without subscription
      //this.hs_.quizResp2(this.url_).subscribe();
      //this._quizes=this.hs_.quizes_;

      this.hs_.getQuizResponse2(this.url_)
      //.map(s=>console.log(s.body))
      .subscribe(r=>{console.log(r); this._quizes.array=r;})
      ;
//    .map(data => {this.data = data});

      //this._quizes=this.service.genericQuizCollection();
      serviceCl.log(['Quizes after ',this._quizes]);
  }

  AddQuiz(){
      serviceCl.log("AddQuiz");
      //this._quizesSend=this.service.genericQuizCollection();
      this.hs_.quizPost(this.url_,this._quizes);
      serviceCl.log(["Quiz",this._quizesSend, "to ",this.url_]);
  }
}
