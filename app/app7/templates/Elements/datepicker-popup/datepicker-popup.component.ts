import { Component, OnInit, Input } from '@angular/core';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {ModelContainer,Quiz,ItemParameter,HtmlItem} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-datepicker-popup',
  templateUrl: './datepicker-popup.component.html',
  styleUrls: ['./datepicker-popup.component.css']
})
export class DatepickerPopupComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() htmlItem_:HtmlItem;
  startDate_:Date;
  model:NgbDateStruct;

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    this.startDate_=null;

    ServiceCl.log(['Constructor : ' + this.constructor.name, this.htmlItem_,this.model])
  }

  ngOnInit() {
    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log(['nodeEmitted Received : ' + this.constructor.name,s])
    });
    this.dateToModel();

    /*
    ModelContainer.nodeEmitted.subscribe(s=>{
        ServiceCl.log(['nodeEmitted Rreceived: ' + this.constructor.name, s])
      if(s instanceof Quiz){

        let a=s.itemParameter.collection.array.find(s=>s.name=="StartDate");
        ServiceCl.log(['ItemParameter StartDate: ' + this.constructor.name, a])
        if(a instanceof ItemParameter){
          this.htmlItem_=a;
        }

          if((this.htmlItem_!=null)){
            if((this.htmlItem_.valueVal!=null)){
              this.dateToModel();
            }
          }

        }
      });

    */

    ServiceCl.log(['Inited : ' + this.constructor.name,this.htmlItem_,this.model])
  }
  navigate_($event){
        ServiceCl.log(["navigate_ ",$event])
  }
  changed(e:any){
    this.modelToItemDate();
    ServiceCl.log(["changed ",this.model,this.htmlItem_])
  }
  toggled(){
    ServiceCl.log(["toggled ",this.model])
  }
  selectToday(){
    this.model={year: new Date().getFullYear(), month: new Date().getMonth()+1, day: new Date().getDate()};
    this.modelToItemDate();
    ServiceCl.log(["selectToday ",this.model,this.htmlItem_.HtmlSubmittedValue])
  }
  modelToItemDate(){
      if(this.model!=null){
        this.htmlItem_.HtmlSubmittedValue.getFullYear=this.model.year;
        this.htmlItem_.HtmlSubmittedValue.getMonth=this.model.month-1;
        this.htmlItem_.HtmlSubmittedValue.getDate=this.model.day;
        //=new Date(this.model.year,this.model.month-1,this.model.day,0,0,0);
      }
      ServiceCl.log(["modelToItemDate ",this.model,this.htmlItem_])
  }
  dateToModel(){
      if((this.htmlItem_!=null) && (this.htmlItem_.HtmlSubmittedValue!=null)){
        this.model={
          year: this.htmlItem_.HtmlSubmittedValue.getFullYear()
          , month: this.htmlItem_.HtmlSubmittedValue.getMonth()+1
          , day:this.htmlItem_.HtmlSubmittedValue.getDate()};
      }
      ServiceCl.log(["dateToModel ",this.model,this.htmlItem_])
  }
  modelChange(e:any){
    this.modelToItemDate();
    ServiceCl.log(["modelChange ",e])
  }
}
