import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {HtmlItemNew} from 'src/app/applist/Models/POCOnew.component'
import { IMultiSelectOption } from 'angular-2-dropdown-multiselect';

@Component({
  selector: 'app-dropdownmulting',
  templateUrl: './dropdownmulting.component.html',
  styleUrls: ['./dropdownmulting.component.css']
})
export class DropdownmultingComponent implements OnInit {

  @Input() htmlItem_:HtmlItemNew;

  optionsModel: number[];
  myOptions: IMultiSelectOption[];

  constructor() {

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
