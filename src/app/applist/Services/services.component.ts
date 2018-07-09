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
