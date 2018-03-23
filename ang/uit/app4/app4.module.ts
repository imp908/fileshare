import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule }          from '@angular/forms';
import { NgModule } from '@angular/core';

import { AppComponent4 } from '../app4/app4.component';

import { FormsModule } from '@angular/forms';

import {mainComponent,testCl2} from '../quest/Quiz/MainComponent/mc.component';

@NgModule({
  declarations: [
    AppComponent4,testCl2,mainComponent
  ],
  imports: [
    BrowserModule,ReactiveFormsModule,FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent4]
})
export class AppModule4 { }
