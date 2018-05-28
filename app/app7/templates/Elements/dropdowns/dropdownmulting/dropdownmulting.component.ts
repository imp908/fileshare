import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,ModelContainer,HtmlItem} from 'app/app7/Models/inits.component'

import { IMultiSelectOption } from 'angular-2-dropdown-multiselect';

@Component({
  selector: 'app-dropdownmulting',
  templateUrl: './dropdownmulting.component.html',
  styleUrls: ['./dropdownmulting.component.css']
})
export class DropdownmultingComponent implements OnInit {

  cName:string;
  test: boolean;

  @Input() htmlItem_:HtmlItem;

  optionsModel: number[];
  myOptions: IMultiSelectOption[];

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
    this.myOptions = [
         { id: 1, name: 'Option 1' },
         { id: 2, name: 'Option 2' },
         { id: 3, name: 'Option 3' },
     ];
   ServiceCl.log(["Inited: " + this.constructor.name]);
  }
  onChange(e:any){
    ServiceCl.log(["DropDown changed: ",e]);
  }

}
