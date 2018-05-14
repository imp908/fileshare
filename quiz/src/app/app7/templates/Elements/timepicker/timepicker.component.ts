import { Component, OnInit } from '@angular/core';
import {NgbTimeStruct} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-timepicker',
  templateUrl: './timepicker.component.html',
  styleUrls: ['./timepicker.component.css']
})

export class TimepickerComponent implements OnInit {

  time: NgbTimeStruct={hour: 13, minute: 30, second: 30};

  seconds_ = true;
  meridian_ = true;

  toggleMeridian() {
    this.meridian_ = !this.meridian_;
    console.log(this.time);
  }

  toggleSeconds() {
    this.seconds_ = !this.seconds_;
    console.log(this.time);
  }

  constructor() { }

  ngOnInit() {

  }

}
