import { Component, OnInit ,Input} from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,ItemParameter,QuizParameter,ModelContainer,NodeCollection,Quiz} from 'app/app7/Models/inits.component'

import { FormsModule }   from '@angular/forms';

@Component({
  selector: 'app-nodes',
  templateUrl: './nodes.component.html',
  styleUrls: ['./nodes.component.css']
})
export class NodesComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() nodePassed_:NodeCollection;

  items:ItemParameter;

  constructor(private service:Service_) {
    this.bindItems();
    this.test=service.test;
    this.cName=this.constructor.name;
    ServiceCl.log(["Constructor: " + this.constructor.name,this.nodePassed_]);
  }

  ngOnInit() {
    this.bindItems();
    ServiceCl.log(["Inited: " + this.constructor.name,this.nodePassed_]);

  }
  bindItems(){
    if((this.nodePassed_!=null) &&
    (this.nodePassed_ instanceof Quiz)){
      this.items=this.nodePassed_.itemParameter;
    }
  }
  clicked_(n:ItemParameter){
    ServiceCl.log(["clicked_: " , this.items,n]);
    ModelContainer.checkedToggle(this.nodePassed_,n);
    if(this.items instanceof QuizParameter){
      this.items.conditionsCheck();
    }
    /* if(n.name=="Replayabe"){
      this.items=ModelContainer.changeShowStatus("GapPicker");
    }*/
  }
}
