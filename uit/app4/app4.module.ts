import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule }          from '@angular/forms';
import { NgModule ,Component, Input, Output, OnInit, EventEmitter} from '@angular/core';

import { AppComponent4 } from '../app4/app4.component';

//npm install --save @ng-bootstrap/ng-bootstrap
import { FormsModule } from '@angular/forms';
//	npm install --save @angular/material @angular/cdk
import {MatButtonModule, MatCheckboxModule} from '@angular/material';
//  npm install --save @angular/animations
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';

import {mainComponent} from '../quest/Quiz/MainComponent/mc.component';
import {awComponent} from '../quest/Quiz/MainComponent/aw.component';
import {qtComponent} from '../quest/Quiz/MainComponent/qt.component';
import {dpComponent} from '../quest/Quiz/MainComponent/dp.component';
import {qzComponent} from '../quest/Quiz/MainComponent/Qz.component';

import {MatTableModule} from '@angular/material/table';


@NgModule({
  declarations: [
    AppComponent4
    ,mainComponent,awComponent,qtComponent,dpComponent,qzComponent

  ],
  imports: [
    BrowserModule,ReactiveFormsModule,FormsModule
    //matdesign animations
    ,BrowserAnimationsModule
    //matdesign components
    ,MatButtonModule, MatCheckboxModule

    ,MatTableModule
  ],
  providers: [],
  bootstrap: [AppComponent4]
})
export class AppModule4 { }
