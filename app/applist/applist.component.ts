import { Component, OnInit } from '@angular/core';
import {TestNew} from 'src/app/applist/Models/initsNew.component'

@Component({
  selector: 'app-applist',
  templateUrl: './applist.component.html',
  styleUrls: ['./applist.component.css']
})
export class ApplistComponent implements OnInit {

  constructor() {

  }

  ngOnInit() {
    TestNew.GO();
  }

}
