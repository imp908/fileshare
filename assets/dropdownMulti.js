
document.addEventListener("DOMContentLoaded", ctmCheck,true);

export function ctmCheck(){
  console.log("DropDown custom js inited");
  BindDropDownEvents();

}

function BindDropDownEvents(){

  var ddI=window.document.getElementsByClassName("dd-item");
  // console.log("BindDropDownEvents",ddI);

  if( (typeof(ddI) !== "undefined" ) && (ddI !== null )){

    for(let i=0;i<ddI.length;i++){
      if( (typeof(ddI[i]) !== "undefined" ) && (ddI[i] !== null ) ){
        // console.log("Binding");
        //ddI[i].addEventListener("click",function(){selected(ddI[i].value)},true)
        ddI[i].onclick =function(){TggleClass(ddI[i],"ddSelected")};
        ddI[i].onmouseover =function(){
          // ClassOnOFf(ddI[i],"ddMouseOver",true)
          ddMover( ddI[i],true);
        };
        ddI[i].onmouseout =function(){
          // mouseoutBackgroundCol_(ddI[i],"white")
          ddMover( ddI[i],false);
        };
      }
    }
  }
}

//Dropdown styling events
//---------------------------------

function ddMover(obj_,bool_){
  if(bool_==true){
    // console.log(["Mouseover"])
    ClassOnOff(obj_,"ddMouseOver",true);
    ClassOnOff(obj_,"ddMouseOut",false);
  }else{
    // console.log(["Mouseout"])
    ClassOnOff(obj_,"ddMouseOver",false);
    ClassOnOff(obj_,"ddMouseOut",true);
    var cl_=obj_.classList.contains("ddSelected");
    if(bool_==="undefined" || bool_ == null){
      ClassOnOff(obj_,"ddSelected",true);
    }
  }
}

//---------------------------------

function TggleClass(v,cls_){
     console.log([cls_,v]);
    var cl_=v.classList.contains(cls_);
    if(cl_==false){
      v.classList.toggle(cls_,true);
    }else {
      v.classList.toggle(cls_,false);
    }
}
export function ClassOnOff(obj_,cls_,bool_){
  console.log(["ClassOnOff",obj_])
  var cl_=obj_.classList.contains(cls_);
  if(bool_==="undefined" || bool_ == null){
    bool_=false;
  }

  if(cl_!=false && bool_==false){
    obj_.classList.toggle(cls_,bool_);
  }else{
    if(cl_==false && bool_!=false){
      obj_.classList.toggle(cls_,bool_);
    }
  }
}
