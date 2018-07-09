import { Component, OnInit, Input } from '@angular/core';

import {ServiceCl} from 'src/app/applist/Services/services.component'
import {ButtonNew,HtmlItemNew} from 'src/app/applist/Models/POCOnew.component'
import {ModelContainerNew} from 'src/app/applist/Models/initsNew.component'


@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css']
})
export class ButtonComponent implements OnInit {

  @Input() _button:ButtonNew;
  @Input() _object:HtmlItemNew;

  constructor(){
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){
    ServiceCl.log(["Inited: " + this.constructor.name,this._button]);
  }
  clicked_(e){
    this._button._clicked=true;
    ModelContainerNew.buttonClicked(this._button,this._object,e);
    ServiceCl.log(["Clicked button with object",this._button,this._object]);
  }
}
