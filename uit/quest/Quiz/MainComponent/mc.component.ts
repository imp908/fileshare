import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import  {qzComponent} from './Qz.component'

@Component({
  selector: 'main-component'
  ,templateUrl: './mc.component.html'
  //,providers:[]
})

export class mainComponent
{

  ngOnInit(){
    console.log('Inited: ' + this.constructor.name)

  }
}
