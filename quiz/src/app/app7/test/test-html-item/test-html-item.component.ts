import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {NodeCollection  ,Collection_,ModelContainer
  ,HtmlItem,TextControl,CheckBoxControl,RadioButtonControl,DatePickerControl,NumberPickerControl
  ,Button
} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-test-html-item',
  templateUrl: './test-html-item.component.html',
  styleUrls: ['./test-html-item.component.css']
})
export class TestHtmlItemComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() itemPassed_:NodeCollection;

  submitBtn_:Button = new Button(0,"Submit New","Submit New2",null,"btn btn-primary",false,"Submit act");

  //new form controll part
  //------------------------
  text_:TextControl=new TextControl(0,"Tb","text_nm","Type text","Type here",null,2,4);
  check_:CheckBoxControl=new CheckBoxControl(0,"Cb","To Check or not to check",true);
  radio_:RadioButtonControl=new RadioButtonControl(0,"Rb","What to shoose?","Choice 2",new Collection_<HtmlItem>([
    new HtmlItem(0,"Rb","Choice 1","option","",null,null)
    ,new HtmlItem(1,"Rb","Choice 2","option","",null,null)
    ,new HtmlItem(2,"Rb","Choice 3","option","",null,null)
  ]));
  date_:DatePickerControl=new DatePickerControl(0,"Dp","Choose date",new Date(2001,11,11,11,11,1))
  number_:NumberPickerControl=new NumberPickerControl(0,"Np","Choose number",1,-2,2);

  constructor(private service:Service_) {
    this.test=service.test;
    this.cName=this.constructor.name;
    this.itemPassed_=this.number_;

    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
  }
  changeTextbox_(event:string ){

    ServiceCl.log(["changeTextbox_ : " + this.constructor.name,event]);
  }
  //new form controll part
  //------------------------
  controlType(){
    //ServiceCl.log(["controlType: ",this.itemPassed_]);
    return ModelContainer.HtmlItemType(this.itemPassed_);

  }


}
