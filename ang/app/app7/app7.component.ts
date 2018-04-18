import { Component, OnInit } from '@angular/core';
import {ServiceCl,Service_} from './Services/services.component'
import {Test} from './Models/inits.component'

@Component({
  selector: 'app-app7',
  templateUrl: './app7.component.html',
  styleUrls: ['./app7.component.css']
})
export class App7Component implements OnInit {
  cName:string;
  test: boolean;

  constructor(private service:Service_) {
      service.test=true;
      service.toLog=true;
      this.test=service.test;
      this.cName=this.constructor.name;
      service.log(service.test)
      service.log(this.test)
      service.log('Constructor : ' + this.constructor.name)

      Test.GO();
  }

  ngOnInit() {

  }

}
