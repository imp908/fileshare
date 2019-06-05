import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Button,ModelContainer} from 'app/app7/Models/inits.component'

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

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    this.button_=new Button();
    this.obj_=null;
  }

  ngOnInit() {
    ServiceCl.log(["Inited " + this.constructor.name,this.button_,this.obj_])
  }

  clicked_(o_:any){
    ServiceCl.log(["Clicked: ",o_])
    ModelContainer.nodeMethodCall(this.button_,o_);
  }
}
