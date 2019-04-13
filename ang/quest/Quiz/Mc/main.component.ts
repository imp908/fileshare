import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {serviceCl} from '../Model/QtMd.component';

@Component({
  selector: 'main-component2'
  ,templateUrl: './main.component.html'
  //,providers:[]
})

export class mainComponent2
{
  test:boolean=true;
  className: string;
  constructor(){this.className=this.constructor.name;}
  ngOnInit(){
    serviceCl.log('Inited: ' + this.constructor.name)
    this.test=serviceCl.test;
  }

}
