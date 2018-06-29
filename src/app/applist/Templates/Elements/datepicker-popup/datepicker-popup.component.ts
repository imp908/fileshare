import { Component, OnInit, Input } from '@angular/core';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {HtmlItemNew} from 'src/app/applist/Models/POCOnew.component'


@Component({
  selector: 'app-datepicker-popup',
  templateUrl: './datepicker-popup.component.html',
  styleUrls: ['./datepicker-popup.component.css']
})
export class DatepickerPopupComponent implements OnInit {

  @Input() htmlItem_:HtmlItemNew;
  startDate_:Date;
  model:NgbDateStruct;

  constructor() {

    this.startDate_=null;

    ServiceCl.log(['Constructor : ' + this.constructor.name, this.htmlItem_,this.model])
  }

  ngOnInit() {
    this.htmlItem_=new HtmlItemNew(null);
    this.dateToModel();

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
      ServiceCl.log(["modelToItemDate ",this.model,this.htmlItem_])
      if(this.model!=null){
        this.htmlItem_.HtmlSubmittedValue.getFullYear=this.model.year;
        this.htmlItem_.HtmlSubmittedValue.getMonth=this.model.month-1;
        this.htmlItem_.HtmlSubmittedValue.getDate=this.model.day;
        //=new Date(this.model.year,this.model.month-1,this.model.day,0,0,0);
      }

  }
  dateToModel(){
      if((this.htmlItem_!=null) && (this.htmlItem_.HtmlSubmittedValue!=null)){
        this.model={
          year: this.htmlItem_.HtmlSubmittedValue.getFullYear
          , month: this.htmlItem_.HtmlSubmittedValue.getMonth+1
          , day:this.htmlItem_.HtmlSubmittedValue.getDate};
      }
      ServiceCl.log(["dateToModel ",this.model,this.htmlItem_])
  }
  modelChange(e:any){
    this.modelToItemDate();
    ServiceCl.log(["modelChange ",e])
  }
}
