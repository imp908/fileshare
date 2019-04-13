import { Component, OnInit,Input } from '@angular/core';
import {NodeCollection,ModelContainer} from 'app/app7/Models/inits.component'
import {ServiceCl,Service_} from 'app/app7/Services/services.component'


@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css']
})

export class ButtonComponent implements OnInit {

  @Input() _buttons:NodeCollection;
  @Input() _obj:any;

  cName:string;
  test: boolean;

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
  }

  ngOnInit() {
    ServiceCl.log(["Inited " + this.constructor.name,this._buttons,this._obj])
    
  }

}
