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
  nodeToPass_:NodeCollection;
  buttonsToPass_:Button;

  constructor(private service:Service_){
    //service.test=false;

    this.test=service.test;
    this.cName=this.constructor.name;

    ServiceCl.log(["Constructor: " + this.constructor.name,this.nodesToPass_]);
  }
  genTest(){
      this.nodesToPass_=Test.GenClasses(false,1,4);
      this.nodeToPass_=this.nodesToPass_.collection.array[0];
      ModelContainer.nodeToEdit=this.nodeToPass_;
      ModelContainer.nodesPassed_=this.nodesToPass_;
      ModelContainer.CheckCycleDisplay();
      ServiceCl.log(this.nodesToPass_);
  }

  ngOnInit(){
    // this.itemsToPass_=new HtmlItem(0,"months","1","option","",null,true,null,null);
    // console.log(["Items: ",this.itemsToPass_]);
    
    this.genTest();
    this.checkButton();
    ServiceCl.log(["Inited: " + this.constructor.name,this.nodesToPass_]);
  }
  click_($event){
    ServiceCl.log(["click_ : " + this.constructor.name,event]);
  }
  submitNew(event:any){
    ServiceCl.log(["submitNew: ",this.nodesToPass_,event]);
  }
  checkDropBox(){
    // this.itemsToPass_=Factory_.CalendarDropDowns().array[2];
    this.nodesToPass_.collection=Factory_.CalendarDropDowns();
    ServiceCl.log(["checkDropBox: ",this.itemsToPass_]);
  }
  checkButton(){
    this.buttonsToPass_=new Button;
    this.buttonsToPass_.collection.add(Factory_.editButton());
    this.buttonsToPass_.collection.add(Factory_.editNewButton());
  }
}
