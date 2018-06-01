import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,NodeCollection,ModelContainer,Quiz,Question,editButtons,editNewButtons,Button} from 'app/app7/Models/inits.component'

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

    ServiceCl.log(['Constructor : ' + this.constructor.name, this.saveButtons_,this.nodeToEdit_])
  }
  nodeTypeGet(){
    return this.nodeToEdit_.getType_();
  }
  ngOnInit(){
    ServiceCl.log(['Inited  : ' + this.constructor.name, this.saveButtons_,this.nodeToEdit_])
    //edit new item
    ModelContainer.nodeAdded.subscribe(s=>{
      ServiceCl.log(['nodeAdded Received : ' + this.constructor.name,s])

      this.saveButtons_=ModelContainer.saveButtons_;
      this.nodeToEdit_=s;
      ModelContainer.nodeToEdit=this.nodeToEdit_;
      ServiceCl.log(['nodeToEdit_ :',this.nodeToEdit_])
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
      this.nodeToEdit_=null;
    });
    ModelContainer.nodeSaved.subscribe(s=>{
      ServiceCl.log([this.constructor.name + " nodeSave: ",s])
      this.nodeToEdit_=null;
    });

    ModelContainer.nodeDeleted.subscribe(s=>{
      ServiceCl.log(['nodeDeleted Received : ' + this.constructor.name,s])
      ModelContainer.nodesPassed_=null;
    });

  }

  nodeSave(n_:NodeCollection){
    ModelContainer.nodeSave(n_);
    ServiceCl.log([this.constructor.name + " nodeSave: ",n_])

  }
}
