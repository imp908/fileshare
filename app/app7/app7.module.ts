import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {Service_} from './Services/services.component';

import { App7Component } from './app7.component';

import { ListComponent } from './list/list.component';
import { ItemComponent } from './item/item.component'

@NgModule({
  imports: [
    BrowserModule
    ,CommonModule
  ],
  declarations: [App7Component, ListComponent, ItemComponent],
  bootstrap:[App7Component],
  providers:[Service_]
})
export class App7Module { }
