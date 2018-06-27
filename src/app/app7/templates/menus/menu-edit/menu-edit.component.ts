import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,NodeCollection,ModelContainer,Quiz,Question,Button} from 'app/app7/Models/inits.component'

import {Factory_} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-menu-edit',
  templateUrl: './menu-edit.component.html',
  styleUrls: ['./menu-edit.component.css']
})
export class MenuEditComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() nodeToEdit_:NodeCollection;
  saveButtons_:Button;

  constructor(private service:Service_){
    //service.test=false;
    //ServiceCl.toLog=true;
    this.test=service.test;
    this.cName=this.constructor.name;
    // this.saveButtons_ = ModelContainer.saveButtons_;

    ServiceCl.log(['Constructor : ' + this.constructor.name, this.saveButtons_, this.nodeToEdit_])
  }

  ngOnInit(){
    ServiceCl.log(['Inited  : ' + this.constructor.name, this.saveButtons_, this.nodeToEdit_])
    //edit new item
    ModelContainer.nodeAdded.subscribe(s=>{
      ServiceCl.log(['nodeAdded Received start: ' + this.constructor.name
      ,s])

      // this.saveButtons_=ModelContainer.saveButtons_;
      this.saveButtons_=ModelContainer.saveNewButtons_;
      this.nodeToEdit_=s;
      ModelContainer.nodeToEdit=this.nodeToEdit_;
      ServiceCl.log(['nodeAdded Received finished: ' + this.constructor.name
      ,this.nodeToEdit_,this.saveButtons_,ModelContainer.saveNewButtons_])

    })

    //edit existing item
    ModelContainer.nodeEmitted.subscribe(s=>{
      this.saveButtons_ = ModelContainer.saveButtons_;
      // this.saveButtons_=Factory_.saveButton();
      this.nodeToEdit_=s;
      ModelContainer.nodeToEdit=this.nodeToEdit_;
      ServiceCl.log(['nodeEmitted Received : ' + this.constructor.name, this.saveButtons_,this.nodeToEdit_,s])
    });

    ModelContainer.nodeSavedNew.subscribe(s=>{
      ServiceCl.log([this.constructor.name + " nodeSave: ",s])
        this.saveButtons_ = ModelContainer.saveNewButtons_;
      ModelContainer.nodeToEdit=null;
      this.nodeToEdit_=ModelContainer.nodeToEdit;
    });
    ModelContainer.nodeSaved.subscribe(s=>{
      ServiceCl.log([this.constructor.name + " nodeSave: ",s])
      ModelContainer.nodeToEdit=null;
      this.nodeToEdit_=ModelContainer.nodeToEdit;
    });

    ModelContainer.nodeDeleted.subscribe(s=>{
      ServiceCl.log(['nodeDeleted received: ' + this.constructor.name,s])
      this.nodeToEdit_=ModelContainer.nodeToEdit;
    });

    ModelContainer.saveDisabled.subscribe(s=>{

      if(this.saveButtons_!=null){
        this.saveButtons_.disabled_=s;
        ServiceCl.log(["saveDisabled received: " + this.constructor.name,s]);
      }

    });


  }

  nodeSave(n_:NodeCollection){
    ModelContainer.nodeSave(n_);
    ServiceCl.log([this.constructor.name + " nodeSave: ",n_])

  }
}
