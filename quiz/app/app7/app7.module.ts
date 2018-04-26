import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NgModule ,Component, Input, Output, OnInit, EventEmitter} from '@angular/core';

import { CommonModule } from '@angular/common';

import {Service_} from './Services/services.component';

import { App7Component } from './app7.component';

import { ListComponent } from './list/list.component';
import { ItemComponent } from './item/item.component';
import { TestComponent } from './test/test.component';
import { MenuComponent } from './menu/menu.component';
import { MenuListComponent } from './menu-list/menu-list.component';
import { MenuEditComponent } from './menu-edit/menu-edit.component'

@NgModule({
  imports: [

 BrowserModule,ReactiveFormsModule,FormsModule,CommonModule
  ],
  declarations: [App7Component, ListComponent, ItemComponent, TestComponent, MenuComponent, MenuListComponent, MenuEditComponent],
  bootstrap:[App7Component],
  providers:[Service_]
})
export class App7Module { }
