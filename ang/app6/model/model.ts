import { Injectable } from '@angular/core';
import { FormGroup,FormControl }  from '@angular/forms';


import { HttpClient, HttpHeaders,HttpEvent } from '@angular/common/http';
import {Observable} from 'rxjs/Observable';


export class ClassImport{
  counter:number;

  constructor(){this.counter=0;}

  increase(){
      this.counter+=1;
  }

}

@Injectable()
export class ClassInject{
  counter:number;

  constructor(){this.counter=0;}

  increase(){
      this.counter+=1;
  }

}

export class ClassImportToInject{
  counter:number;

  constructor(){this.counter=0;}

  increase(){
      this.counter+=1;
  }

}
