import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {QuizNew,QuestionNew,AnswerNew,ButtonNew
,QuizItemNew,HtmlItemNew} from 'src/app/applist/Models/POCOnew.component';

import {ModelContainerNew,FactoryNew} from 'src/app/applist/Models/initsNew.component';


@Component({
  selector: 'app-menu-list',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.css']
})
export class MenuListComponent implements OnInit {

  @Input() _quizes:QuizItemNew;
  @Input() _questions:QuizItemNew;
  @Input() _answers:QuizItemNew;

  @Input() _buttonsQuiz:HtmlItemNew;
  @Input() _buttonsQuestion:HtmlItemNew;
  @Input() _buttonsAnswer:HtmlItemNew;

  constructor(){
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){

    this._buttonsQuiz=FactoryNew.ItemButtons("");
    this._buttonsQuestion=FactoryNew.ItemButtons("");
    this._buttonsAnswer=FactoryNew.ItemButtons("");

    ModelContainerNew.nodeEdit.subscribe(s=>{
      this._questions=ModelContainerNew.quizSelected;
      this._answers=ModelContainerNew.questionSelected;
      ServiceCl.log(["nodeEdit received by " + this.constructor.name
        ,this._quizes
      ]);
    });

    ServiceCl.log(["Inited: " + this.constructor.name,this._quizes,this._buttonsQuiz]);
  }

}
