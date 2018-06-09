import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,ModelContainer,Factory_} from 'app/app7/Models/inits.component'
import {NodeCollection,Quiz,Question,Answer,Button} from 'app/app7/Models/inits.component'


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
  AnswerToEdit:Answer;

  @Input() nodesPassed_:NodeCollection;
  typePassed_:string;
  constructor(private service:Service_){
      //service.test=false;
      ServiceCl.log(["Constructor: " + this.constructor.name]);
      this.test=service.test;
      this.cName=this.constructor.name;

    }
    ngOnInit(){

      // this.genTest();
      this.conatinerBind();

      ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log([this.constructor.name+" NodeEmitted: ",s])
        this.conatinerBind();
      });
      ModelContainer.nodeSavedNew.subscribe(s=>{
        ServiceCl.log([this.constructor.name+" nodeSaveNew received: ",s])
          this.conatinerBind();
      });
      ModelContainer.nodeSaved.subscribe(s=>{
        ServiceCl.log([this.constructor.name+" nodeSave received: ",s])
          this.conatinerBind();
      });
      ModelContainer.nodeDeleted.subscribe(s=>{
        ServiceCl.log([this.constructor.name+" nodeDelete received: ",s])
          this.conatinerBind();
      });

      ServiceCl.log(["Inited: " + this.constructor.name,this.nodesPassed_]);
    }
    conatinerBind(){

      //Binding variables

      this.nodesPassed_=ModelContainer.nodesPassed_;
      this.QuizToEdit=ModelContainer.QuizToEdit;
      this.QuestionToEdit=ModelContainer.QuestionToEdit;
      this.AnswerToEdit=ModelContainer.AnswerToEdit;

      //Modelcontainer initialization

      // ModelContainer.CheckCycleDisplay();
      // ModelContainer.saveButtons_=new Button();
      // ModelContainer.saveNewButtons_=new Button();
      // ModelContainer.saveButtons_.collection.add(Factory_.saveButton());
      // ModelContainer.saveNewButtons_.collection.add(Factory_.saveNewButton());

      ServiceCl.log([this.constructor.name+" container binded ",
      this.QuizToEdit,this.QuestionToEdit,this.AnswerToEdit]);
    }
    genTest(){
      this.nodesPassed_=Test.GenClasses(true,3,5);
      ModelContainer.nodesPassed_=this.nodesPassed_;
      ServiceCl.log(["nodesPassed_",this.nodesPassed_,ModelContainer]);
    }


}
