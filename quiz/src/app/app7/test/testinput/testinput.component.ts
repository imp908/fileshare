import { Component, OnInit } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'

import {NodeCollection  ,Collection_,ModelContainer
  ,HtmlItem,TextControl,CheckBoxControl,RadioButtonControl,DatePickerControl,NumberPickerControl
  ,Button
} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-testinput',
  templateUrl: './testinput.component.html',
  styleUrls: ['./testinput.component.css']
})
export class TestinputComponent implements OnInit {
  cName:string;
  test: boolean;

  inputTypesModel:{key:string,value:number
    ,textbox_:{value_:string,displayValue_:string,pattern_:string,maxLength_:number,minLength_:number}
    ,checkbox_:boolean
    ,radio_:{name_:string,val_:string,values_:[{value_:string}]}
    ,color_:any
    ,date_?: Date
    ,dateL_?: string, month_:string
    ,number_:{value_:number,min_:number,max_:number}
    ,range_:{value_:number,min_:number,max_:number}
    ,files_:{value_:any,multiple_:true,fileList_:any,accept_:string}
  }



  constructor(private service:Service_){
    this.test=service.test;
    this.cName=this.constructor.name;

    this.inputModelInit();

  }

  ngOnInit() {
  }

  inputModelInit(){
    this.inputTypesModel={key:"0",value:1
    ,textbox_:{value_:"val",displayValue_:"test txt",pattern_:"[0-9]",maxLength_:null,minLength_:null}
    ,checkbox_:true
    ,radio_:
      {name_:"n1",val_:"X1"
      ,values_:[{value_:"X1"},{value_:"Y2"},{value_:"Z3"}]}
      ,color_:"#d248be",date_:new Date(),dateL_:null,month_:null
      ,number_:{value_:1,min_:-1,max_:3}
      ,range_:{value_:0,min_:-100,max_:100}
      ,files_:{value_:"",multiple_:true,fileList_:null,accept_:"\".png, .jpg, .jpeg\""}
    };
  }

  click_($event){
    ServiceCl.log(["click_ : " + this.constructor.name,event]);
  }

  changeTextbox_(event:string ){
    this.inputTypesModel.textbox_.value_=event;
    ServiceCl.log(["changeTextbox_ : " + this.constructor.name,event]);
  }
  color_($event){
    ServiceCl.log(["color_ : " + this.constructor.name,this.inputTypesModel.color_,event]);
  }
  submit_($event){
    ServiceCl.log(["submit_ : " + this.constructor.name,this.inputTypesModel,event]);
  }
  checked_($event){
    this.inputTypesModel.checkbox_=!this.inputTypesModel.checkbox_;
    ServiceCl.log(["checked_ : " + this.constructor.name]);
  }
  files_(event: any){
    ServiceCl.log(["files_ : " + this.constructor.name,event]);
    this.inputTypesModel.files_.fileList_=event.srcElement.files;
  }


  submitNew(event:any){
    ServiceCl.log(["submitNew: ",event]);
  }

}
