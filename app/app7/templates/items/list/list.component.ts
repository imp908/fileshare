import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {NodeCollection,menuButtons,Button,ModelContainer,Question,Factory_} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})

export class ListComponent implements OnInit {
  cName:string;
  test: boolean;

  buttons_:Button;

  @Input() nodes_:NodeCollection;
  constructor(private service:Service_){
    //service.test=false;
    //ServiceCl.toLog=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    this.buttons_= Factory_.addButton();
    //this.nodes_=new Quiz();
    ServiceCl.log('Constructor : ' + this.constructor.name)
    //service.test=true;
  }

  ngOnInit(){
    ServiceCl.log('Inited : ' + this.constructor.name)

    ModelContainer.addNewToggle.subscribe(s=>{
      if(this.nodes_ instanceof Question){
        this.buttons_.disabled_=s;
      }
      ServiceCl.log(["received saveDisabled " + this.constructor.name,s,this.buttons_,this.nodes_]);
    });

  }

}
