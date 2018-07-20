import { Component, OnInit } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component';

@Component({
  selector: 'app-page-not-found',
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.component.css']
})
export class PageNotFoundComponent implements OnInit {

  path:string;
  pictures:string[];
  constructor() {
      ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {

    this.path="/assets/img/404s/";
    this.pictures=[
      "404_0.jpg"
      ,"404_1.jpg"
      ,"404_2.jpg"
      ,"404_3.jpg"
      ,"pixar404.jpg"
      ,"404_4.jpg"
      ,"404_5.jpg"
      ,"404_6.jpg"
      ,"404_7.jpg"
      ,"404_8.jpg"
    ]

    var pic=Math.floor(Math.random()*(this.pictures.length));
    console.log()
    this.path+=this.pictures[pic];
    ServiceCl.log(["Inited: " + this.constructor.name,this.path,pic]);
  }

}
