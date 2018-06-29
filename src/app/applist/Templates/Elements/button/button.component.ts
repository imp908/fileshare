import { Component, OnInit, Input } from '@angular/core';

import {ServiceCl} from 'src/app/applist/Services/services.component'
import {NumberPickerControlNew} from 'src/app/applist/Models/POCOnew.component'

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css']
})
export class ButtonComponent implements OnInit {

  constructor() {

    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
  }

}
