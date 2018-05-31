import { Component, OnInit ,Input} from '@angular/core';

import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,ModelContainer,HtmlItem,Question} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-dropdownng',
  templateUrl: './dropdownng.component.html',
  styleUrls: ['./dropdownng.component.css']
})
export class DropdownComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() htmlItem_:HtmlItem;

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    ServiceCl.log(["Constructor: " + this.constructor.name,this.htmlItem_]);
  }

  ngOnInit() {
    ServiceCl.log(["Inited: " + this.constructor.name,this.htmlItem_]);
  }
  changed(i){
    this.htmlItem_.HtmlSubmittedValue=i._value;
    if(ModelContainer.nodeToEdit instanceof Question){
      ModelContainer.CheckAnswerAmount(this.htmlItem_.HtmlSubmittedValue);
      ServiceCl.log(["Button checked: ",ModelContainer.editButtons_]);
    }
    ServiceCl.log(["changed",this.htmlItem_,i,ModelContainer.nodeToEdit]);
  }
}
