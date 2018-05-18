import { Component, OnInit, Input } from '@angular/core';
import { NodeCollection } from 'app/app7/Models/inits.component';
import {ServiceCl} from 'app/app7/Services/services.component';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css']
})
export class ButtonComponent implements OnInit {

  @Input() buttons_:NodeCollection;
  @Input() object_:any;
  constructor() {
    ServiceCl.log(["Constructed " + this.constructor.name])
  }

  ngOnInit(){
    ServiceCl.log(["Inited " + this.constructor.name,this.buttons_,this.object_])
  }

}
