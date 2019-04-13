import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule }          from '@angular/forms';
import { NgModule ,Component, Input, Output, OnInit, EventEmitter} from '@angular/core';
import { App6Component } from '../app6/app6.component';
//npm install --save @ng-bootstrap/ng-bootstrap
import { FormsModule } from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';


import { ImpComponent } from './import/import.component';
import { InjComponent } from './inject/inject.component';
import { MainComponent } from './main/main.component';

import {ServiceComponent} from './serv/serv.service';
import {ClassInject,ClassImportToInject} from './model/model';

@NgModule({
  declarations: [
    App6Component
    ,ImpComponent
    ,InjComponent
    ,MainComponent
  ],
  imports: [
    BrowserModule,ReactiveFormsModule,FormsModule,

     // import HttpClientModule after BrowserModule.
     HttpClientModule,

    /*
    //matdesign animations
    ,BrowserAnimationsModule
    //matdesign components
    ,MatButtonModule, MatCheckboxModule

    ,MatTableModule
    */
  ],
  bootstrap: [App6Component]
  ,providers: [ServiceComponent,ClassInject,ClassImportToInject]
})
export class App6Module { }
