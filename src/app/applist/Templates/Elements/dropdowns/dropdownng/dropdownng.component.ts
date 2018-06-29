import { Component, OnInit ,Input} from '@angular/core';

import {ServiceCl} from 'src/app/applist/Services/services.component'
import {HtmlItemNew} from 'src/app/applist/Models/POCOnew.component'

@Component({
  selector: 'app-dropdownng',
  templateUrl: './dropdownng.component.html',
  styleUrls: ['./dropdownng.component.css']
})
export class DropdownComponent implements OnInit {

  @Input() htmlItem_:HtmlItemNew;
  alert_:string;

  constructor(){

    ServiceCl.log(["Constructor: " + this.constructor.name,this.htmlItem_]);
  }

  ngOnInit(){


    ServiceCl.log(["Inited: " + this.constructor.name,this.htmlItem_]);
  }
  changed(i){
    this.htmlItem_.HtmlSubmittedValue=i._value;

    ServiceCl.log(["changed",this.htmlItem_,i]);
  }
}
