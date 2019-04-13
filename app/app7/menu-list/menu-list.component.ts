import { Component, OnInit } from '@angular/core';
import {ServiceCl,Service_} from '../Services/services.component'
import {Test,NodeCollection,ModelContainer,Quiz,Question} from '../Models/inits.component'


@Component({
  selector: 'app-menu-list',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.css']
})
export class MenuListComponent implements OnInit {
  cName:string;
  test: boolean;

  QuizToEdit:Quiz;
  QuestionToEdit:Question;
  AnswerToEdit:Question;

  constructor(private service:Service_) {
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
  }

  ngOnInit() {

    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log([this.constructor.name+" NodeEmitted: ",s])
      this.QuizToEdit=ModelContainer.QuizToEdit;
      this.QuestionToEdit=ModelContainer.QuestionToEdit;
      this.AnswerToEdit=ModelContainer.AnswerToEdit;
    });


  }

}
