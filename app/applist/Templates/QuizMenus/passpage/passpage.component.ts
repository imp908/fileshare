import { Component, OnInit, Input } from '@angular/core';
import {ServiceCl} from 'src/app/applist/Services/services.component'
import {ModelContainerNew,FactoryNew} from 'src/app/applist/Models/initsNew.component';
import {QuizNew,ButtonNew} from 'src/app/applist/Models/POCOnew.component';



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

  @Input() _quiz:QuizNew;
  @Input() _button:ButtonNew;
  constructor() {
    this.cssback=null;
    ServiceCl.log(["Constructor: " + this.constructor.name]);
  }

  ngOnInit() {
    this.path="/assets/bkgd/";
    this.pictures=new Array<string>();
    for(let i=10;i<=26;i++){this.pictures.push(i+".jpg")};

    var pic=Math.floor(Math.random()*(this.pictures.length));

    this.style_={"background":"linear-gradient(to right,#23074d, #cc5333)"}
    this.style_.background=FactoryNew.gradArr(2,30);

    // this.fgImage_= this.path+this.pictures[pic];
    // this.fgImage_= {"background":"url('"+this.path+this.pictures[pic]+"') no-repeat center center fixed"};
    this.fgImage_= {"background-image":"url('"+this.path+this.pictures[pic]+"')"};

    ServiceCl.log(["Inited: " + this.constructor.name,this.style_,this.fgImage_, this._quiz]);
  }

  mouseenter_(e:any,i:any){
    if(i!=null){
      i.style.boxShadow="10px 10px 15px 1px rgba(60, 60, 60, 0.4),inset 2px 2px 10px 1px rgba(150, 150, 150,0.5),inset -2px -2px 10px 1px rgba(150, 150, 150,0.5)";
    }
    // ServiceCl.log(["mouseenter_: " + this.constructor.name,e,i]);
  }
  mouseleave_(e:any,i:any){
    if(i!=null){
      i.style.boxShadow='';
    }
    // ServiceCl.log(["mouseleave_: " + this.constructor.name,e,i]);
  }
}
