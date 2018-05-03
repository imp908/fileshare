import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Button} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-click',
  templateUrl: './click.component.html',
  styleUrls: ['./click.component.css']
})

export class ClickComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() button:Button;
  @Input() obj_:any;

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    this.button=new Button();
    this.obj_=null;
  }

  ngOnInit() {
    ServiceCl.log(["Click inited: ",this.button,this.obj_])
  }

  clicked_(o_:any){
    ServiceCl.log(["Clicked: ",o_])
  }
}
