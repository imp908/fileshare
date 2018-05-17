import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NgModule, Component, Input, Output, OnInit, EventEmitter} from '@angular/core';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { CommonModule } from '@angular/common';

import {Service_} from './Services/services.component';

import { App7Component } from './app7.component';

import { ListComponent } from './templates/items/list/list.component';
import { ItemComponent } from './templates/items/item/item.component';
import { TestComponent } from './test/test.component';
import { MenuComponent } from './templates/menus/menu/menu.component';
import { MenuListComponent } from './templates/menus/menu-list/menu-list.component';
import { MenuEditComponent } from './templates/menus/menu-edit/menu-edit.component';
import { ClickComponent } from './templates/buttons/click/click.component';
import { ButtonComponent } from './templates/buttons/button/button.component';
import { NodesComponent } from './templates/nodes/nodes.component';
import { DatepickerPopupComponent } from './templates/Elements/datepicker-popup/datepicker-popup.component';
import { TimepickerComponent } from './templates/Elements/timepicker/timepicker.component';
import { NodeComponent } from './templates/nodes/node/node.component';
import { GappickerNgComponent } from './templates/Elements/gappicker-list/gappicker-ng/gappicker-ng.component';
import { GappickerListComponent } from './templates/Elements/gappicker-list/gappicker-list.component';
import { GappickerDropComponent } from './templates/Elements/gappicker-list/gappicker-drop/gappicker-drop.component';
import { TestinputComponent } from './test/testinput/testinput.component';
import { TestHtmlItemComponent } from './test/test-html-item/test-html-item.component';

@NgModule({
  imports: [
    BrowserModule,ReactiveFormsModule,FormsModule,CommonModule
    ,NgbModule.forRoot()
  ],
  declarations:[App7Component, ListComponent, ItemComponent, TestComponent, MenuComponent, MenuListComponent, MenuEditComponent, ClickComponent,ButtonComponent, NodesComponent, DatepickerPopupComponent, TimepickerComponent, NodeComponent, GappickerNgComponent, GappickerListComponent, GappickerDropComponent, TestinputComponent, TestHtmlItemComponent],
  bootstrap:[App7Component],
  providers:[Service_]
})
export class App7Module { }
