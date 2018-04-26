import { Component, OnInit } from '@angular/core';
import {ServiceCl,Service_} from '../Services/services.component'
import {Test,NodeCollection,ModelContainer} from '../Models/inits.component'

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})

export class MenuComponent implements OnInit {
  cName:string;
  test: boolean;

  nodesPassed_:NodeCollection;

  nodeSelected_:NodeCollection;
  nodesSelected_:NodeCollection;

  constructor(private service:Service_){
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    this.genTest();
  }
  genTest(){
    ModelContainer.nodesPassed_=Test.GenClasses(false,1,4);
    this.nodesPassed_= ModelContainer.nodesPassed_;
    ServiceCl.log(["nodesPassed_",this.nodesPassed_]);
  }
  ngOnInit(){

  }

}
