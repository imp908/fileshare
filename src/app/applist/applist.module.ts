import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApplistComponent } from './applist.component';

import { DatepickerPopupComponent } from
'src/app/applist/Templates/Elements/datepicker-popup/datepicker-popup.component';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { ButtonComponent } from './Templates/Elements/button/button.component';
import { ItemComponent } from './Templates/Elements/item/item.component';
import { MenuComponent } from './Templates/menu/menu.component';
import { TestComponent } from './Templates/test/test.component';
import { ItemsComponent } from './Templates/Elements/items/items.component';

@NgModule({
  declarations: [ApplistComponent,DatepickerPopupComponent, ButtonComponent, ItemComponent, MenuComponent, TestComponent, ItemsComponent],
  imports: [
    BrowserModule,ReactiveFormsModule,FormsModule,CommonModule
    ,NgbModule.forRoot()

  ]
  ,bootstrap:[ApplistComponent]
})
export class ApplistModule { }
