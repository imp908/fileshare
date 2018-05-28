import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,HtmlItem,ModelContainer} from 'app/app7/Models/inits.component'
@Component({
  selector: 'app-nodeitem',
  templateUrl: './nodeitem.component.html',
  styleUrls: ['./nodeitem.component.css']
})
export class NodeitemComponent implements OnInit {

      cName:string;
      test: boolean;

      @Input() htmlItem_:HtmlItem;

  constructor(private service:Service_) {
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;

    ServiceCl.log(["Constructor: " + this.constructor.name,this.htmlItem_]);
   }

  ngOnInit() {
    ServiceCl.log(["Inited: " + this.constructor.name,this.htmlItem_]);
  }
  controlType(){
    return ModelContainer.HtmlItemType(this.htmlItem_);
  }
}
