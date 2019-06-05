import { Component, OnInit } from '@angular/core';
import { Injectable } from '@angular/core';

@Injectable()
export class ServiceComponent  {

  counter:number;

  constructor() {this.counter=0; }

  increase(){
    this.counter+=1;
  }


}
