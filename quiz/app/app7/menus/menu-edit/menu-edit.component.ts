import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,NodeCollection,ModelContainer,Quiz,Question} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-menu-edit',
  templateUrl: './menu-edit.component.html',
  styleUrls: ['./menu-edit.component.css']
})
export class MenuEditComponent implements OnInit {
  cName:string;
  test: boolean;

  nodeToEdit_:NodeCollection;

  constructor(private service:Service_) {
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
  }
  ngOnInit(){
    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log([this.constructor.name+" NodeEmitted: ",s])
      this.nodeToEdit_=ModelContainer.nodeToEdit;
    });
  }
  nodeSave(n_:NodeCollection){
    ModelContainer.nodeSave(n_);
    ServiceCl.log([this.constructor.name + " nodeSave: ",n_])
  }
}
