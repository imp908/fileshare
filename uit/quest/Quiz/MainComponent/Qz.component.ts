import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {dpComponent} from './dp.component';
import {qtComponent} from './qt.component';

import  {Quiz,serviceCl} from '../Model/QtMd.component'

@Component({
  selector: 'qz-component'
  ,templateUrl: './Qz.component.html'
  //,declarations: [qtComponent,dpComponent]
  ,providers:[serviceCl]
})

export class qzComponent
{
  mcText:string;
  quiz: Quiz;
  sc:serviceCl;

  constructor(){
    this.sc=new serviceCl();
    this.quiz=this.sc.testQuizExplicit();
  }

  ngOnInit(){

    console.log('Inited: ' + this.constructor.name)
    console.log(this.quiz.Qts[0].value)
  }
}
