import { Component, OnInit,Input } from '@angular/core';
import {ServiceCl,Service_} from 'app/app7/Services/services.component'
import {Test,NodeCollection,ModelContainer} from 'app/app7/Models/inits.component'
import * as $ from 'jquery';

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
    ServiceCl.log("Constructor: " + this.constructor.name);
    //service.test=false;
    this.test=service.test;
    this.cName=this.constructor.name;

    this.jqueryCheck();


    /*
    $('.datepicker').datepicker({
      format: 'mm/dd/yyyy',
      startDate: '-3d'
    });
    */

  }
  ngOnInit(){
    ServiceCl.log("Inited: " +this.constructor.name);
  }

  jqueryCheck(){
    $(document).ready(
      function() {
        ServiceCl.log(["jquery inited on document.ready "+$.fn.jquery]);
      }
    );
  }


}
