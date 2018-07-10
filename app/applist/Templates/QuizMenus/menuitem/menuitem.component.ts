import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {QuizNew,QuestionNew,AnswerNew
,ButtonNew
,QuizItemNew} from 'src/app/applist/Models/POCOnew.component';

import {ModelContainerNew,FactoryNew} from 'src/app/applist/Models/initsNew.component';


@Component({
  selector: 'app-menuitem',
  templateUrl: './menuitem.component.html',
  styleUrls: ['./menuitem.component.css']
})
export class MenuitemComponent implements OnInit {

  @Input() _quizes:QuizItemNew;
  @Input() _buttons:ButtonNew[];

  _AddNewButton:ButtonNew[];
  _addObj:QuizItemNew;
  constructor() {
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){
    let _itmName="";

    //Button text and new object instance passed by click
    if(this._quizes instanceof QuizItemNew){_itmName="Quiz"; this._addObj=FactoryNew.NewQuizItemObj(new QuizNew(null));}
    if(this._quizes instanceof QuizNew){_itmName="Question"; this._addObj=FactoryNew.NewQuizItemObj(new QuestionNew(null));}
    if(this._quizes instanceof QuestionNew){_itmName="Answer";this._addObj=FactoryNew.NewQuizItemObj(new AnswerNew(null));}

    this._AddNewButton=FactoryNew.AddNewButton(_itmName);
    ServiceCl.log(["Inited: " + this.constructor.name,this._quizes]);
  }

}
