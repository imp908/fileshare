import { Component, OnInit, Input } from '@angular/core';

import {ServiceCl} from 'src/app/applist/Services/services.component'
import {HtmlItemNew,QuizItemNew,TextControlNew} from 'src/app/applist/Models/POCOnew.component'
import {FactoryNew} from 'src/app/applist/Models/initsNew.component';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})

export class ItemComponent implements OnInit {

  @Input() _item:HtmlItemNew;
  @Input() _object:HtmlItemNew;
  constructor(){
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){
    ServiceCl.log(["Inited: " + this.constructor.name,this._item,this._object]);
  }
  typeCheck(){
    return FactoryNew.TypeCheck(this._item);
  }
  instanceCheck(){
    return FactoryNew.InstanceCheck(this._item);
  }
  clicked_(e_){
    if(FactoryNew.TypeCheck(this._item)=='CheckBox'){
      this._item.HtmlSubmittedValue=!this._item.HtmlSubmittedValue;
      ServiceCl.log(["Checkbox clicked",this._item]);
    }
    ServiceCl.log(["Item clicked event for item and object: ",e_,this._item,this._object]);
  }
  changeTextbox_(e){
    if(this._item instanceof TextControlNew){
      this._item.HtmlSubmittedValue=e;
      this._item.displayValue=this._item.HtmlSubmittedValue;
    }
    ServiceCl.log(["changeTextbox_: ",e,this._item])
  }
  isQuizItem(i_:HtmlItemNew){
    console.log(["isQuizItem ",i_,i_ instanceof QuizItemNew])
    if(i_ != null){
      if(i_ instanceof QuizItemNew){
        return true;
      }
      return false;
    }
    return null;
  }
}
