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
import { ItemComponent } from './Templates/Elements/items/item/item.component';
import { TestComponent } from './Templates/test/test.component';
import { ItemsComponent } from './Templates/Elements/items/items.component';
import { MenuListComponent } from './Templates/QuizMenus/menu-list/menu-list.component';
import { MenuEditComponent } from './Templates/QuizMenus/menu-edit/menu-edit.component';
import { MenuMainComponent } from './Templates/QuizMenus/menu-main/menu-main.component';

import { DropdownmultiComponent } from './Templates/Elements/dropdowns/dropdownmulti/dropdownmulti.component';

import { GappickerNgComponent } from './Templates/Elements/gappicker-ng/gappicker-ng.component';


@NgModule({
  declarations: [ApplistComponent,DatepickerPopupComponent
    , ButtonComponent, ItemComponent, TestComponent
    , ItemsComponent, MenuListComponent, MenuEditComponent, MenuMainComponent
    ,DropdownmultiComponent
    ,GappickerNgComponent
    ],
  imports: [
    BrowserModule,ReactiveFormsModule,FormsModule,CommonModule
    ,NgbModule.forRoot()


  ]
  ,bootstrap:[ApplistComponent]
})
export class ApplistModule { }
