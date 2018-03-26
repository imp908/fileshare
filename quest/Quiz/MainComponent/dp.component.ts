import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { FormGroup,FormControl } from '@angular/forms';

import  {Quiz} from '../Model/QtMd.component'



@Component({
  selector: 'dp-component'
  ,templateUrl: './dp.component.html'
  //,providers:[]
})

export class dpComponent
{
  @Input() qz_:Quiz;
  form_:FormGroup;

  ngOnInit(){
    console.log('Inited: ' + this.constructor.name)
    console.log(this.qz_)
  }


}
