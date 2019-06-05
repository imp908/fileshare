import { Component, OnInit } from '@angular/core';

import {CompComponent} from '../comp/comp.component';

import {ServiceComponent} from '../serv/serv.service';

import {ClassImport,ClassInject,ClassImportToInject} from '../model/model';

@Component({
  selector: 'inject',
  templateUrl: './inject.component.html',
  styleUrls: ['./inject.component.css']
})
export class InjComponent implements OnInit {

  cp:CompComponent;

  cpInj:ServiceComponent;
  cl:ClassImport;
  clInj:ClassInject;
  clImpInj: ClassImportToInject;

  constructor(
    private ser:ServiceComponent,
    private clInj_:ClassInject,
    private clImpInj_: ClassImportToInject
  ){
    this.cp=new CompComponent();
    this.cpInj=ser;
    this.cl=new ClassImport();
    this.clInj=clInj_;
    this.clImpInj=clImpInj_;
}
  increase(){
    this.cp.increase();
    this.cpInj.increase();
    this.cl.increase();
    this.clInj.increase();
    this.clImpInj.increase();
  }
  ngOnInit() {

  }

}
