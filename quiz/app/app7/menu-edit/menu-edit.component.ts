import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from '../Services/services.component'
import {Test,NodeCollection,ModelContainer,Quiz,Question} from '../Models/inits.component'



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

  ngOnInit() {
    ModelContainer.nodeEmitted.subscribe(s=>{
      ServiceCl.log([this.constructor.name+" NodeEmitted: ",s])
      this.nodeToEdit_=ModelContainer.nodeToEdit;
    });
  }
  nodeSave(n_:NodeCollection){
    ServiceCl.log([this.constructor.name + " nodeSave: ",n_])
  }
}
