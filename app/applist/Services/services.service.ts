import { Injectable } from '@angular/core';

//injecatable service for singletone instance

@Injectable()
export class Service_{
    public toLog:boolean=true;
    public test:boolean=true;

    public log(n:any){
      if(this.toLog===true){
        console.log(n);
      }
    }
}
