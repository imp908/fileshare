import { Component, OnInit } from '@angular/core';
import {ServiceCl,Service_} from '../Services/services.component'

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  cName:string;
  test: boolean;
  constructor(private service:Service_) {
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;
    service.log('Constructor : ' + this.constructor.name)
    //service.test=true;
  }

  ngOnInit() {
  }

}
