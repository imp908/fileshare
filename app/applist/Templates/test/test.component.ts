import {Component,OnInit} from '@angular/core';
import {ButtonNew,HtmlItemNew,QuizItemNew} from 'src/app/applist/Models/POCOnew.component';
import {QuizNew,QuestionNew,AnswerNew} from 'src/app/applist/Models/POCOnew.component';

import {FactoryNew,ModelContainerNew} from 'src/app/applist/Models/initsNew.component';

import {ServiceCl} from 'src/app/applist/Services/services.component';

import {TestNew} from 'src/app/applist/Models/initsNew.component';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})

export class TestComponent implements OnInit {

  //test for nuttons collection with item object
  items_:{buttons_:HtmlItemNew;object_:HtmlItemNew;}[];
  controlls_:HtmlItemNew;

  ItemButtons_:HtmlItemNew;
  editButtons_:HtmlItemNew;
  quizItems_:QuizItemNew;

  quizes_: Array<QuizNew>;
  questions_:Array<QuestionNew>;
  answers_:Array<AnswerNew>;

  TestItemButtons_:HtmlItemNew;
  objectOne:HtmlItemNew;
  objectTwo:HtmlItemNew;

  // Menu checker
  _quizes:QuizNew[];
  _questions:QuestionNew[];
  _answers:AnswerNew[];

  _buttons:ButtonNew[];

  passingQuiz:QuizNew;
  passignQuestion:QuestionNew;
  passButton:ButtonNew;

  constructor(){

    TestNew.GO();

    ModelContainerNew.Init();


    this.items_=TestNew.Buttons();

    this.controlls_=new HtmlItemNew(null);

    // this.controlls_.array=TestNew.ControllsBulkGen();

    this.quizes_=new Array<QuizNew>();

    this.controlls_=TestNew.ControllsGroupsGen();

    this.quizItems_=TestNew.QuizList();
    this.ItemButtons_=FactoryNew.ItemButtons("");
    this.editButtons_=FactoryNew.EditButtons("");

    this.TestItemButtons_=new HtmlItemNew(null);
    this.TestItemButtons_.array=new Array<HtmlItemNew>(
      TestNew.TestItemButtons("fxvt","fxhr")
      ,TestNew.TestItemButtons("fxhr","fxvt"));
    this.objectOne=this.quizes_[0];
    this.objectTwo=this.quizes_[1];

    //pass page quiz test
    let qzTmp=ModelContainerNew.QuizesPassed.array[0];
    if(qzTmp instanceof QuizNew){this.passingQuiz=qzTmp;}
    let btnPs=FactoryNew.StartButton("Quiz");
    if(btnPs instanceof ButtonNew){ this.passButton=btnPs;}
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){

    ModelContainerNew.stateChanged.subscribe(s=>{
      ServiceCl.log(["stateChanged received by " + this.constructor.name]);
    });

    ServiceCl.log(["Inited: " + this.constructor.name, this.quizItems_]);
  }

}
