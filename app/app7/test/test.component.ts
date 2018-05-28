import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from '../Services/services.component'
import {Test,ModelContainer,Factory_,NodeCollection,HtmlItem } from '../Models/inits.component'

import {Button} from 'app/app7/Models/inits.component'

import * as SVG from 'assets/svg.js'

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {
  cName:string;
  test: boolean;

  nodesToPass_:NodeCollection;
  itemsToPass_:HtmlItem;

  constructor(private service:Service_){
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    this.genTest();

    ServiceCl.log(["Constructor: " + this.constructor.name,this.nodesToPass_]);
  }
  genTest(){
      this.nodesToPass_=Test.GenClasses(false,1,4);

      ServiceCl.log(this.nodesToPass_);
  }

  ngOnInit(){
    this.checkDropBox();
    ServiceCl.log(["Inited: " + this.constructor.name,this.nodesToPass_]);
  }
  click_($event){
    ServiceCl.log(["click_ : " + this.constructor.name,event]);
  }
  submitNew(event:any){
    ServiceCl.log(["submitNew: ",this.nodesToPass_,event]);
  }
  checkDropBox(){
    this.itemsToPass_=Factory_.CalendarDropDowns().array[2];
    ServiceCl.log(["checkDropBox: ",this.itemsToPass_]);
  }
}
