import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,HtmlItem,ModelContainer} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-nodes',
  templateUrl: './nodes.component.html',
  styleUrls: ['./nodes.component.css']
})
export class NodesComponent implements OnInit {

    cName:string;
    test: boolean;

    @Input() htmlItems_:HtmlItem;

    constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    ServiceCl.log(['Constructor : ' + this.constructor.name])
  }

  ngOnInit() {
    ServiceCl.log(['Inited : ' + this.constructor.name,this.htmlItems_])
  }

  controlType(){
    //ServiceCl.log(["controlType: ",this.itemPassed_]);
    return ModelContainer.HtmlItemType(this.htmlItems_);

  }

}
