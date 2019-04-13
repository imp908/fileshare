import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from '../app/app.component';
import { AppComponent2 } from './app2.component';

import { ClickMeComponent } from '../click/click_me.component';

@NgModule({
  declarations: [
    AppComponent2,ClickMeComponent
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [AppComponent2]
})
export class AppModule2 { }
