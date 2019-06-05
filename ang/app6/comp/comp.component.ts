import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'comp',
  templateUrl: './comp.component.html',
  styleUrls: ['./comp.component.css']
})

export class CompComponent implements OnInit {

  counter:number;

  constructor() {this.counter=0; }

  increase(){
    this.counter+=1;
  }

  ngOnInit() {

  }

}
