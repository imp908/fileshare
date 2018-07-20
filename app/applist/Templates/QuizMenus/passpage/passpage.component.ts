import { Component, OnInit } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {ModelContainerNew,FactoryNew} from 'src/app/applist/Models/initsNew.component';

@Component({
  selector: 'app-passpage',
  templateUrl: './passpage.component.html',
  styleUrls: ['./passpage.component.css']
})
export class PasspageComponent implements OnInit {

  cssback:string;
  style_:string;
  constructor() {
    this.cssback=null;

  }

  ngOnInit() {
    this.style_='position:absolute;height:100%;width:100%;z-index:-1;'
    this.style_+=FactoryNew.GradientGen();
  }

}
