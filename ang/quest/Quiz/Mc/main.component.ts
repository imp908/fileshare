import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

@Component({
  selector: 'main-component2'
  ,templateUrl: './main.component.html'
  //,providers:[]
})

export class mainComponent2
{
  className: string;
  constructor(){this.className=this.constructor.name;}
  ngOnInit(){
    console.log('Inited: ' + this.constructor.name)
  }

}
