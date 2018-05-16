import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {ModelContainer,Quiz,ItemParameter} from 'app/app7/Models/inits.component'


@Component({
  selector: 'app-gappicker-ng',
  templateUrl: './gappicker-ng.component.html',
  styleUrls: ['./gappicker-ng.component.css']
})
export class GappickerNgComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() itemValue_:{key:string,value:number,min:number,max:number};
  min:number;
  max:number;

  cstmSpin:boolean;

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;

    this.cstmSpin=true;

    ServiceCl.log(['Constructor : ' + this.constructor.name,this.itemValue_])
  }

  ngOnInit() {
    this.min=this.itemValue_.min;
    this.max=this.itemValue_.max;
    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log(['nodeEmitted Received : ' + this.constructor.name,s])
    });

    ServiceCl.log(['Inited : ' + this.constructor.name,this.itemValue_])
  }
  increase(){
    if(this.itemValue_.value+1<=this.max){
      this.itemValue_.value+=1;
    }
    ServiceCl.log(['increased to : ',this.itemValue_.value])
  }
  decrease(){
    if(this.itemValue_.value-1>=this.min){
      this.itemValue_.value-=1;
    }
    ServiceCl.log(['decreased to : ',this.itemValue_.value])
  }
  input_(){
      ServiceCl.log(['inputed to : ',this.itemValue_.value])
  }
}
