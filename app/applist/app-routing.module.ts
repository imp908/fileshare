import { NgModule }             from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PageNotFoundComponent } from './Templates/Pages/page-not-found/page-not-found.component';
import { MenuMainComponent } from './Templates/QuizMenus/menu-main/menu-main.component';
import { TestComponent } from './Templates/test/test.component';
import { PasspageComponent } from './Templates/QuizMenus/passpage/passpage.component';


const appListRoutes : Routes =[
  { path: 'ts',component: TestComponent },
  { path: 'edit',component: MenuMainComponent },
  { path: 'pass',component: PasspageComponent },
  { path: '',redirectTo: '/ts' , pathMatch:'full'},
  { path: '**', component: PageNotFoundComponent}
];

@NgModule({
  imports:[
    RouterModule.forRoot(
      appListRoutes,
      { enableTracing: false } //true } // <-- debugging purposes only
    )
  ],
  exports:[RouterModule]
})

export class AppRoutingModule{}
