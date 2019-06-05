import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {NodeCollection,menuButtons,Button} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})

export class ListComponent implements OnInit {
  cName:string;
  test: boolean;

  button_:Button;

  @Input() nodes_:NodeCollection;
  constructor(private service:Service_){
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    this.button_= new menuButtons();
    //this.nodes_=new Quiz();
    ServiceCl.log('Constructor : ' + this.constructor.name)
    //service.test=true;
  }
  ngOnInit(){
    ServiceCl.log('Inited : ' + this.constructor.name)
  }

}
