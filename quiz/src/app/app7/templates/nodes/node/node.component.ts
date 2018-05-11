import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,ItemParameter,ModelContainer} from 'app/app7/Models/inits.component'

@Component({
  selector: 'app-node',
  templateUrl: './node.component.html',
  styleUrls: ['./node.component.css']
})
export class NodeComponent implements OnInit {

    cName:string;
    test: boolean;

    @Input() nodesPassed_:ItemParameter;

    constructor(private service:Service_) {
    this.test=service.test;
    ServiceCl.log(['Constructor : ' + this.constructor.name])
  }

  ngOnInit() {
    ServiceCl.log(['Inited : ' + this.constructor.name])
  }

}
