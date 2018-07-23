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
  style_:any;
  fgImage_:any;

  path:string;
  pictures:string[];
  constructor() {
    this.cssback=null;
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
    this.path="/assets/bkgd/";
    this.pictures=[
      "1.jpg"
      ,"2.jpg"
      ,"3.jpg"
      ,"4.jpg"
      ,"5.jpg"
      ,"6.jpg"
      ,"7.jpg"
      ,"8.jpg"
      ,"9.jpg"
    ]

    var pic=Math.floor(Math.random()*(this.pictures.length));

    this.style_={"background":"linear-gradient(to right,#23074d, #cc5333)"}
    // this.fgImage_= {"background":"url('/assets/bkgd/1.jpg') no-repeat center center fixed"};
    this.fgImage_= {"background":"url('"+this.path+this.pictures[pic]+"') center center fixed"};
    this.style_.background=FactoryNew.gradArr(2,30);
    ServiceCl.log(["Inited: " + this.constructor.name,this.style_,this.fgImage_]);
  }

  mouseenter_(e:any,i:any){
    if(i!=null){
      // i.style.boxShadow="15px 15px 20px 1px rgba(60, 60, 60, 0.6),inset 5px 5px 5px 1px rgba(60, 60, 60,1)";
      ServiceCl.log(["style: ",i.style.boxShadow]);
    }
    ServiceCl.log(["mouseenter_: " + this.constructor.name,e,i]);
  }
  mouseleave_(e:any,i:any){
    if(i!=null){
      i.style.boxShadow='';
      ServiceCl.log(["style: ",i.style.boxShadow]);
    }
    ServiceCl.log(["mouseleave_: " + this.constructor.name,e,i]);
  }
}
