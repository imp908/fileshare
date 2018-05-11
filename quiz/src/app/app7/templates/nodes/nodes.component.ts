import { Component, OnInit ,Input} from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,ItemParameter,ModelContainer} from 'app/app7/Models/inits.component'

import { FormsModule }   from '@angular/forms';

@Component({
  selector: 'app-nodes',
  templateUrl: './nodes.component.html',
  styleUrls: ['./nodes.component.css']
})
export class NodesComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() nodesPassed_:ItemParameter;

  constructor(private service:Service_) {
    ServiceCl.log(["Constructor: " + this.constructor.name,this.nodesPassed_]);
    this.test=service.test;
  }

  ngOnInit() {
    ServiceCl.log(["Inited: " + this.constructor.name,this.nodesPassed_]);
  }

  clicked_(n:ItemParameter){
      ServiceCl.log(["clicked_: " , n,this.nodesPassed_]);
      let a=this.nodesPassed_.collection.getByItem(n);
      ServiceCl.log(["valueVal: " , a]);
      a.valueVal=!n.valueVal;
      ModelContainer.changeShowStatus("GapPicker");
  }
}
