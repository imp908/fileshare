import { Component, OnInit,Input } from '@angular/core';

import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,ModelContainer,HtmlItem} from 'app/app7/Models/inits.component'



@Component({
  selector: 'app-dropdownmulti',
  templateUrl: './dropdownmulti.component.html',
  styleUrls: ['./dropdownmulti.component.css']
})
export class DropdownmultiComponent implements OnInit {
  cName:string;
  test: boolean;

  itemColor:any;
  _mouseover:boolean;
  _mouseout:boolean;

  @Input() htmlItem_:HtmlItem;
  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    ServiceCl.log(["Constructor: " + this.constructor.name,this.htmlItem_]);
  }

  ngOnInit() {
    // d.ctmCheck();
    this._mouseover=false;
    this._mouseout=true;
    ServiceCl.log(["Inited: " + this.constructor.name,this.htmlItem_]);
  }
  clicked_(event_,obj_){
    ServiceCl.log(["clicked_ in " +  this.constructor.name,event_,obj_])
    this.toggle_(obj_);

  }
  isSelected(i_){
    if(i_.HtmlSubmittedValue==null){
      i_.HtmlSubmittedValue=false;
      return false;
    }
    if(i_.HtmlSubmittedValue==false){
      return false;
    }
    return true;
  }
  toggle_(i_){
    if(i_.HtmlSubmittedValue==null){
      i_.HtmlSubmittedValue=false;
    }
    i_.HtmlSubmittedValue=!i_.HtmlSubmittedValue;
  }
  mouseenter_(e){
    ServiceCl.log(["mouseenter_",e ]);
    e.target.classList.toggle("ddMouseOver",true);
    e.target.classList.toggle("ddMouseOut",false);

    // d.ClassOnOff(i_,"ddMouseOver",true);
    // d.ClassOnOff(i_,"ddMouseOut",false);

  }
  mouseleave_(e){
    ServiceCl.log(["mouseleave_",e ]);
    e.target.classList.toggle("ddMouseOver",false);
    e.target.classList.toggle("ddMouseOut",true);

  }
}
