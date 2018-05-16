import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {ModelContainer} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-gappicker-drop',
  templateUrl: './gappicker-drop.component.html',
  styleUrls: ['./gappicker-drop.component.css']
})
export class GappickerDropComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() itemValueDrop_:{key:string,values:[{value:number,checked:boolean}]}

  constructor(private service:Service_){
    this.test=service.test;
    this.cName=this.constructor.name;

    ServiceCl.log(['Constructor : ' + this.constructor.name,this.itemValueDrop_])
   }

  ngOnInit(){
    ServiceCl.log(['nodeEmitted Received : ' + this.constructor.name,this.itemValueDrop_])
  }

  clicked_(i_:any){
    i_.checked=!i_.checked;
    ServiceCl.log(['Clicked : ', i_])
  }

}
