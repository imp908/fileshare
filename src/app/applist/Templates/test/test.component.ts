import {Component,OnInit} from '@angular/core';
import {ButtonNew,HtmlItemNew} from 'src/app/applist/Models/POCOnew.component';
import {ServiceCl} from 'src/app/applist/Services/services.component';

import {TestNew} from 'src/app/applist/Models/initsNew.component';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  items_:{buttons_:HtmlItemNew;object_:HtmlItemNew;}[];

  constructor(){
    this.items_=TestNew.Buttons();
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit(){
    ServiceCl.log(["Inited: " + this.constructor.name,  this.items_]);
  }

}
