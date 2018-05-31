import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,NodeCollection,ModelContainer,Quiz,Question,editButtons,editNewButtons,Button} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-menu-edit',
  templateUrl: './menu-edit.component.html',
  styleUrls: ['./menu-edit.component.css']
})
export class MenuEditComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() nodeToEdit_:NodeCollection;
  editButtons_:Button;

  constructor(private service:Service_){
    //service.test=false;
    //ServiceCl.toLog=true;
    this.test=service.test;
    this.cName=this.constructor.name;
    // this.editButtons_ = ModelContainer.editButtons_;

    ServiceCl.log(['Constructor : ' + this.constructor.name, this.editButtons_,this.nodeToEdit_])
  }
  nodeTypeGet(){
    return this.nodeToEdit_.getType_();
  }
  ngOnInit(){
    ServiceCl.log(['Inited  : ' + this.constructor.name, this.editButtons_,this.nodeToEdit_])
    //edit new item
    ModelContainer.nodeAdded.subscribe(s=>{
      ServiceCl.log(['nodeAdded Received : ' + this.constructor.name,s])
      this.editButtons_=ModelContainer.editButtons_;
      this.nodeToEdit_=s;
      ModelContainer.nodeToEdit=this.nodeToEdit_;
      ServiceCl.log(['nodeToEdit_ :',this.nodeToEdit_])
    })

    //edit existing item
    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log(['nodeEmitted Received : ' + this.constructor.name, this.editButtons_,this.nodeToEdit_,s])
      this.editButtons_ = ModelContainer.editButtons_;
      this.nodeToEdit_=s;
      ModelContainer.nodeToEdit=  this.nodeToEdit_;
    });

    ModelContainer.nodeSavedNew.subscribe(s=>{
      ServiceCl.log([this.constructor.name + " nodeSave: ",s])
        this.editButtons_ = ModelContainer.editNewButtons_;
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
