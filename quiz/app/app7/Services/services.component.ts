import { Injectable } from '@angular/core';

//static ervice class

export class ServiceCl{
    public static toLog:boolean=true;
    public static test:boolean=true;

    public static log(n:any){
      if(ServiceCl.toLog===true){
          console.log(n);
      }
    }
}


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
