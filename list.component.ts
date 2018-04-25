import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from '../Services/services.component'
import {Factory_,Quiz,Test,NodeCollection} from '../Models/inits.component'

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})

export class ListComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() nodes_:NodeCollection;
  constructor(private service:Service_){
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    //this.nodes_=new Quiz();    
    service.log('Constructor : ' + this.constructor.name)
    //service.test=true;
  }
  ngOnInit(){

  }

}
