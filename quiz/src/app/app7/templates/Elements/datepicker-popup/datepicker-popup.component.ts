import { Component, OnInit, Input } from '@angular/core';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {ModelContainer,Quiz,ItemParameter} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-datepicker-popup',
  templateUrl: './datepicker-popup.component.html',
  styleUrls: ['./datepicker-popup.component.css']
})
export class DatepickerPopupComponent implements OnInit {
  cName:string;
  test: boolean;

  itemParameter_:ItemParameter;
  startDate_:Date;
  model:NgbDateStruct;

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    this.startDate_=null;

    ServiceCl.log(['Constructor : ' + this.constructor.name, this.itemParameter_,this.model])
  }

  ngOnInit() {

    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log(['nodeEmitted Rreceived: ' + this.constructor.name, s])
      if(s instanceof Quiz){
        let a=s.itemParameter.collection.array.find(s=>s.name=="StartDate");
        ServiceCl.log(['ItemParameter StartDate: ' + this.constructor.name, a])
        if(a instanceof ItemParameter){
          this.itemParameter_=a;
        }
      }
      if((this.itemParameter_!=null)){
        if((this.itemParameter_.valueVal!=null)){
          this.dateToModel();
        }
      }
    });

    ServiceCl.log(['Inited : ' + this.constructor.name, this.itemParameter_,this.model])
  }

  changed(){
    this.modelToItemDate();
    ServiceCl.log(["changed ",this.model,this.itemParameter_])
  }
  toggled(){
    ServiceCl.log(["toggled ",this.model])
  }
  selectToday(){
    this.model={year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate()};
    this.modelToItemDate();
    ServiceCl.log(["selectToday ",this.model,this.itemParameter_.valueVal])
  }
  modelToItemDate(){
      ServiceCl.log(["modelToItemDate ",this.model,this.itemParameter_])
      this.itemParameter_.valueVal=new Date(this.model.year,this.model.month+1,this.model.day,0,0,0);
  }
  dateToModel(){
      ServiceCl.log(["dateToModel ",this.model,this.itemParameter_])
      this.model={year: this.itemParameter_.valueVal.getFullYear(), month: this.itemParameter_.valueVal.getMonth(), day:this.itemParameter_.valueVal.getDate()};
  }
}
