import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,NodeCollection,ModelContainer} from 'app/app7/Models/inits.component'


//declare var $: any;

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})

export class MenuComponent implements OnInit {
  cName:string;
  test: boolean;

  @Input() nodesPassed_:NodeCollection;

  constructor(private service:Service_){

    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;

    /*
    $('.datepicker').datepicker({
      format: 'mm/dd/yyyy',
      startDate: '-3d'
    });
    */
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }
  ngOnInit(){
    ServiceCl.log(["Inited: " + this.constructor.name,this.nodesPassed_]);
  }


}
