import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {ModelContainer,Quiz,NumberPickerControl} from 'app/app7/Models/inits.component'


@Component({
  selector: 'app-gappicker-ng',
  templateUrl: './gappicker-ng.component.html',
  styleUrls: ['./gappicker-ng.component.css']
})
export class GappickerNgComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() itemValue_:NumberPickerControl;
  min?:number;
  max?:number;

  cstmSpin:boolean;

  first:string;
  second:string;
  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;

    this.cstmSpin=true;

    ServiceCl.log(['Constructor : ' + this.constructor.name,this.itemValue_])
  }

  ngOnInit() {
    this.min=this.itemValue_.minN;
    this.max=this.itemValue_.maxN;
    if(this.itemValue_.cssClass=="fxvt"){
      this.first="shevron up";
      this.second="shevron down";
    }
    if(this.itemValue_.cssClass=="fxhr"){
      this.first="shevron left";
      this.second="shevron right";
    }
    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log(['nodeEmitted Received : ' + this.constructor.name,s])
    });

    ServiceCl.log(['Inited : ' + this.constructor.name,this.itemValue_,this.first])
  }
  increase(){
    if(this.max!=null){
      if(this.itemValue_.HtmlSubmittedValue+1<=this.max){
      this.itemValue_.HtmlSubmittedValue+=1;}
    }else{ this.itemValue_.HtmlSubmittedValue+=1;}
    ServiceCl.log(['increased to : ',this.itemValue_.HtmlSubmittedValue])
  }
  decrease(){
    if((this.min!=null)){
      if(this.itemValue_.HtmlSubmittedValue-1>=this.min){
      this.itemValue_.HtmlSubmittedValue-=1;}
    }else{  this.itemValue_.HtmlSubmittedValue-=1;}
    ServiceCl.log(['decreased to : ',this.itemValue_.HtmlSubmittedValue])
  }
  input_(){
    ServiceCl.log(['inputed to : ',this.itemValue_.HtmlSubmittedValue])
  }
}
