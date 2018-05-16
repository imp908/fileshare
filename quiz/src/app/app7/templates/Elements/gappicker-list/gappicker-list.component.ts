import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {ModelContainer,Quiz,ItemParameter} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-gappicker-list',
  templateUrl: './gappicker-list.component.html',
  styleUrls: ['./gappicker-list.component.css']
})
export class GappickerListComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() itemValueArr_:[{key:string,value:number,min:number,max:number}];
  @Input() itemValueArrDrop_:[{key:string,values:[{value:number,checked:boolean}]}];

  constructor(private service:Service_){
    this.test=service.test;
    this.cName=this.constructor.name;

    ServiceCl.log(['Constructor : ' + this.constructor.name,this.itemValueArr_])
   }

  ngOnInit(){

    ServiceCl.log(['nodeEmitted Received : ' + this.constructor.name,this.itemValueArr_])
  }

}
