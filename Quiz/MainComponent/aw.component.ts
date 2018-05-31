import { Component, Input, Output, OnInit, EventEmitter }   from '@angular/core';
import { FormGroup,FormControl ,FormsModule,NgForm}      from '@angular/forms';

import  {Aw,serviceCl} from '../Model/QtMd.component'



@Component({
  selector: 'aw-component'
  ,templateUrl: './aw.component.html'
  //,providers:[]
})

export class awComponent
{
  mcText: string;
  answer_: Aw[];
  form_: FormGroup;
  service_: serviceCl;

  @Output() answersCollection=new EventEmitter<Aw[]>();

  constructor(){
    this.service_=new serviceCl();
    this.answer_=[
      new Aw(0,"a",true,"Input answer text")
      ,new Aw(1,"b",false,"Input answer text")
      ,new Aw(2,"c",true,"Input answer text")
    ];
    //this.form_=this.service_.toControlGroupFM(this.answer_);
    //this.form_=this.service_.toControlGroupOb(this.answer_);
    console.log(this.form_)
  }

  ngOnInit(){
    console.log('Inited: ' + awComponent.name)
    console.log(this.answer_)
    console.log(JSON.stringify(this.form_.value))
  }
  ngSubmit(){
    console.log('ngSubmit')
    console.log(JSON.stringify(this.form_.value))
  }
  saveForm(){
    console.log('saveAnswers');
    console.log(this.answer_);
    console.log(JSON.stringify(this.form_.value))
    this.answersCollection.emit(this.answer_);
  }
  submitForm(f: NgForm){
    console.log('submitAnswers');
    console.log(f);
    console.log(JSON.stringify(f.value))
  }
  submintAnswer(a: Aw[]) {
    console.log('submintAnswer')
    console.log(a)
  }

}
