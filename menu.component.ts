import { Component, OnInit } from '@angular/core';
import {ServiceCl,Service_} from '../Services/services.component'
import {Factory_,Quiz,Test,NodeCollection} from '../Models/inits.component'

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  cName:string;
  test: boolean;

  nodes_:NodeCollection;
  constructor(private service:Service_){
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    this.genTest();
  }
  genTest(){
      this.nodes_=Test.GenClasses(false,1,4);
      ServiceCl.log(this.nodes_);
  }
  ngOnInit() {
  }

}
