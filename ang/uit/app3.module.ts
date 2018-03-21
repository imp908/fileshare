import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule }          from '@angular/forms';
import { NgModule } from '@angular/core';

import { AppComponent3 } from '../app3/app3.component';

import {HeroChildComponent} from '../hr/hr.component';
import {HeroParentComponent} from '../hr/hr-pr.component';

import {ClickMeComponent} from '../click/click_me.component';

import {QuestionCp_}  from '../quest/questionCp.component'

@NgModule({
  declarations: [
    AppComponent3,HeroChildComponent,HeroParentComponent,ClickMeComponent,QuestionCp_
  ],
  imports: [
    BrowserModule,ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent3]
})
export class AppModule3 { }
