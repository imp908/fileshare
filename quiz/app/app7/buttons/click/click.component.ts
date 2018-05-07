import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Button,ModelContainer} from 'app/app7/Models/inits.component'

import 'assets/popper.min.js'

@Component({
  selector: 'app-click',
  templateUrl: './click.component.html',
  styleUrls: ['./click.component.css']
})

export class ClickComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() button_:Button;
  @Input() obj_:any;

  constructor(private service:Service_){
    //ServiceCl.toLog=true;
    this.test=service.test;
    //ServiceCl.toLog=true;
    this.cName=this.constructor.name;
    this.button_=new Button();
    this.obj_=null;
    //ServiceCl.log(['Constructor : ' + this.constructor.name,this.button_,this.obj_])
  }

  ngOnInit() {
    //ServiceCl.log(["Inited " + this.constructor.name,this.button_,this.obj_])

  }

  clicked_(o_:any){
    ServiceCl.log(["Clicked: ",o_])
    ModelContainer.nodeMethodCall(this.button_,o_);
  }
}
