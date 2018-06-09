import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component';
import {NodeCollection,ModelContainer,Quiz,itemButtons,Button,Factory_} from 'app/app7/Models/inits.component';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})

export class ItemComponent implements OnInit {
  cName:string;
  test: boolean;
  @Input() node_:NodeCollection;

  buttons_:Button;

  constructor(private service:Service_) {
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    // this.buttons_=new itemButtons();
    this.buttons_=Factory_.itemButtons();
    service.log('Constructor : ' + this.constructor.name)
    //service.test=true;
  }

  ngOnInit(){

  }
  itemSelect(n_:NodeCollection){
    ModelContainer.nodeSelect(n_);
    ServiceCl.log(["itemSelect",n_]);
  }
  itemDelete(n_:NodeCollection){
    ModelContainer.nodeDelete(n_);
    ServiceCl.log(["itemDelete",n_]);
  }

}
