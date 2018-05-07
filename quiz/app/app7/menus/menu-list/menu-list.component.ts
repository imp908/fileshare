import { Component, OnInit } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,NodeCollection,ModelContainer,Quiz,Question} from 'app/app7/Models/inits.component'


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

  nodesPassed_:NodeCollection;
  typePassed_:string;
  constructor(private service:Service_) {
    //service.test=false;
    ServiceCl.log(["ModelContainer",ModelContainer])
    this.test=service.test;
    this.cName=this.constructor.name;
    this.genTest();
  }
  genTest(){
    this.nodesPassed_=Test.GenClasses(true,3,5);
    ModelContainer.nodesPassed_=this.nodesPassed_;
    ServiceCl.log(["nodesPassed_",this.nodesPassed_,ModelContainer]);
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
