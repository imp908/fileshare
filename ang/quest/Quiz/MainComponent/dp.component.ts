import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { FormGroup,FormControl } from '@angular/forms';

import  {Quiz,serviceCl} from '../Model/QtMd.component'



@Component({
  selector: 'dp-component'
  ,templateUrl: './dp.component.html'
  //,providers:[]
})

export class dpComponent
{
  @Input() qz_:Quiz;
  form_:FormGroup;
  sc:serviceCl;

  constructor(){
    this.sc=new serviceCl();
    this.qz_=new Quiz();
    this.form_=this.sc.toControlGroup();
  }
  ngOnInit(){
    console.log('Inited: ' + this.constructor.name)
    console.log(this.qz_)
  }


}
