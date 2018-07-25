import { Component, OnInit, Input } from '@angular/core';

import {ServiceCl} from 'src/app/applist/Services/services.component'
import {ModelContainerNew,FactoryNew} from 'src/app/applist/Models/initsNew.component';
import {QuizNew,QuestionNew,ButtonNew} from 'src/app/applist/Models/POCOnew.component';



@Component({
  selector: 'app-passpage',
  templateUrl: './passpage.component.html',
  styleUrls: ['./passpage.component.css']
})
export class PasspageComponent implements OnInit {

  cssback:string;
  style_:any;
  fgImage_:any;

  path:string;
  pictures:string[];

  svgs:string;

  @Input() _quiz:QuizNew;
  @Input() _button:ButtonNew;

  _buttonNext:ButtonNew;
  _buttonPrevious:ButtonNew;

  _question:QuestionNew;
  constructor(){
    this.cssback=null;
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
    this.path="/assets/bkgd/";
    this.pictures=new Array<string>();
    for(let i=10;i<=26;i++){this.pictures.push(i+".jpg")};

    var pic=Math.floor(Math.random()*(this.pictures.length));

    this.style_={"background":"linear-gradient(to right,#23074d, #cc5333)"}
    this.style_.background=FactoryNew.gradArr(2,30);

    // this.fgImage_= this.path+this.pictures[pic];
    // this.fgImage_= {"background":"url('"+this.path+this.pictures[pic]+"') no-repeat center center fixed"};
    this.fgImage_= {"background-image":"url('"+this.path+this.pictures[pic]+"')"};

    this.svgs="/assets/svg/arrRtAng.svg";

    //for test only
    if(ModelContainerNew.quizSelected==null){
    ModelContainerNew.Init();
    let qz=ModelContainerNew.QuizesPassed.array[FactoryNew.rnd(0,ModelContainerNew.QuizesPassed.array.length)];
      if(qz instanceof QuizNew){
        ModelContainerNew.quizSelected=qz;
      }
    }

    this._quiz=ModelContainerNew.quizSelected;
    this._question=ModelContainerNew.questionSelected;
    this._button=FactoryNew.StartButton("");
    this._buttonNext=ModelContainerNew.buttonNext_;
    this._buttonPrevious=ModelContainerNew.buttonPrevious_;

    ModelContainerNew.start.subscribe((s:QuizNew)=>{
      this.start_(s);
      ServiceCl.log(["start received by: "  + this.constructor.name,s]);
    });

    ModelContainerNew.swap.subscribe((s:QuestionNew)=>{
      ServiceCl.log(["swap received by: "  + this.constructor.name,s]);
    });

    ServiceCl.log(["Inited: " + this.constructor.name,this.style_,this.fgImage_, this._quiz]);
  }

  mouseenter_(e:any,i:any){

    // ServiceCl.log(["mouseenter_: " + this.constructor.name,e,i]);
  }
  mouseleave_(e:any,i:any){

    // ServiceCl.log(["mouseleave_: " + this.constructor.name,e,i]);
  }

  start_(q_:QuestionNew){
    if(q_ instanceof QuestionNew){
      this._quiz=null;
      this._question=ModelContainerNew.questionSelected;
    }
    ServiceCl.log(["start_",this._quiz,this._question]);
  }

  previous_(){
    ServiceCl.log(["previous_",this.constructor.name]);
  }
  next_(){
    ServiceCl.log(["next_",this.constructor.name]);
  }

}
