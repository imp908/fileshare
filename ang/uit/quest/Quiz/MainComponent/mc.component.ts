import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {mainComponent,TestCl} from '../Model/QtMd.component';

@Component({
  selector: 'main-component'
  ,templateUrl: './mc.component.html'
  //,providers:[]
})

export class mainComponent
{
  mcText:string;

  ngOnInit(){
    console.log('Inited')
    console.log(this)
    this.mcText="txt";
  }
}

@Component({
  selector: 'main-component'
  ,templateUrl: './mc.component.html'
  //,providers:[]
})
export class testCl2
{
  mcText2:string;

  ngOnInit(){
    console.log('Inited')
    console.log(this)
    this.mcText2="test str";
  }
}
