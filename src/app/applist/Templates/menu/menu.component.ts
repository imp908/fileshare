import { Component, OnInit } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {NumberPickerControlNew} from 'src/app/applist/Models/POCOnew.component'

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {

  constructor() {

    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
  }

}
