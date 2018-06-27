import { Component, OnInit ,Input} from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,HtmlItem,ModelContainer,NodeCollection,Quiz} from 'app/app7/Models/inits.component'

import {Collection_} from 'app/app7/Models/inits.component'

import { FormsModule }   from '@angular/forms';

@Component({
  selector: 'app-grps',
  templateUrl: './nodegr.component.html',
  styleUrls: ['./nodegr.component.css']
})
export class NodesGroups implements OnInit {
  cName:string;
  test: boolean;

  @Input() nodePassed_:NodeCollection;

  htmlItemsGroup_:Collection_<NodeCollection>;

  constructor(private service:Service_){
    this.test=service.test;
    this.cName=this.constructor.name;
    ServiceCl.log(["Constructor: " + this.constructor.name,this.nodePassed_]);
  }

  ngOnInit(){
    this.bindItems();

    ServiceCl.log(["Inited: " + this.constructor.name,this.nodePassed_,this.htmlItemsGroup_]);
  }
  bindItems(){
    if((this.nodePassed_!=null) &&
    (this.nodePassed_.collection.type_ =="HtmlItem")){
      this.htmlItemsGroup_=this.nodePassed_.collection;
      ServiceCl.log(["bindItems: " + this.constructor.name , this.htmlItemsGroup_]);
    }
  }

}
