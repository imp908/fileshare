import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {QuizNew,QuestionNew,AnswerNew
,ButtonNew} from 'src/app/applist/Models/POCOnew.component';

@Component({
  selector: 'app-menu-list',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.css']
})
export class MenuListComponent implements OnInit {

  @Input() _quizes:QuizNew[];
  @Input() _questions:QuestionNew[];
  @Input() _answers:AnswerNew[];

  @Input() _buttons:ButtonNew[];

  constructor(){
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){
    ServiceCl.log(["Inited: " + this.constructor.name,
    this._quizes,this._questions,this._answers]);
  }

}
