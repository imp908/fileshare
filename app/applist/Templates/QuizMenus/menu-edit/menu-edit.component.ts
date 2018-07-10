import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'

import {ButtonNew,QuizItemNew} from 'src/app/applist/Models/POCOnew.component';
import {ModelContainerNew} from 'src/app/applist/Models/initsNew.component';

@Component({
  selector: 'app-menu-edit',
  templateUrl: './menu-edit.component.html',
  styleUrls: ['./menu-edit.component.css']
})
export class MenuEditComponent implements OnInit {

  @Input() _item:QuizItemNew;
  @Input() _editButtons:ButtonNew[];

  constructor(){
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){
    ServiceCl.log(["Inited: " + this.constructor.name,this._item,this._editButtons]);

    ModelContainerNew.stateChanged.subscribe(s=>{
      this._item=ModelContainerNew.nodeSelected;
      ServiceCl.log(["stateChanged received by " + this.constructor.name
      ,this._item,this._editButtons]);
    });

  }

}
