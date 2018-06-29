import { Component, OnInit, Input } from '@angular/core';

import {ServiceCl} from 'src/app/applist/Services/services.component'
import {NumberPickerControlNew} from 'src/app/applist/Models/POCOnew.component'

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {

  constructor() {

    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
  }

}
