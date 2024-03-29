
//Changing part

//---------------------------------

function setAttribute(obj,attr,val){
  if(!(isUndefined(obj) && isUndefined(attr) && isUndefined(val))){
    obj.setAttribute(attr,val);
  }
}



//Generating getElementsByTagName

//---------------------------------

function innerHTML_(child_,text_){
    if(!isNull(child_) && !isNull(text_)){
      child_.innerHTML=text_;
    }else {
      child_=null;
      log("text_ not added")
    }
    return child_;
}

//checs document body and element undefined and returns child or NULL

function appendChild_(document_body_,element_){
  let child=null;
  if(!isNull(document_body_) && !isNull(element_)){
    child=document_body_.appendChild(element_);
  }else {
    log("Child not appended")
  }

  return child;
}

//checks document undefined adds element by name

function createElement(document_ ,name_){
  let element_=null;
  if(!isNull(document_)){
    element_=document_.createElement(name_);
    if(!isNull(element_)){return element_;}else {
      log("element_ not created");
    }
  }else {
    console.log("document_ not initied");
  }

  return element_
}

function curBody_(){
  let _doc=curDoc_();
  if(!isUndefined(_doc) && !isUndefined(window.document.body)){
    return window.document.body;
  }else {
    log("window.document.body is undefined");
    return null;
  }
}
function curDoc_(){

  if(!isUndefined(curWin_()) && !isUndefined(window.document)){
    return window.document;
  }else {
    log("window.document is undefined");
    return null;
  }
}
function curWin_(){

  if(!isUndefined(window)){
    return window;
  }else {
    log("window is undefined");
    return null;
  }
}

function log(obj,toLog){
    if(
      !(typeof(toLog) === "undefined" || toLog ==="null" || toLog==false)
    ){
      console.log(obj,toLog);
    }
}

function isNull(obj){

  if( !isUndefined(obj) && obj!==null)
  {
    //console.log("not null or undefined",obj);
    return false;
  }else {
    //console.log("null or undefined",obj);
    return true;
  }
}
function isUndefined(obj,toLog){

    if(typeof(obj) === "undefined"){
      log(["is undefined",obj])
      return true;
    }
    log(["defined",obj])
    return false;

}

function callCatch_(function_){
	printName(arguments.callee,'Started');
	if(typeof function_==='function')
	{
		try{
			function_.call();
		}
		catch(e)
		{
			console.log('Error catched');
			var tostr=function_.toString();
			var argc=function_.name;
			console.log(argc + ' throw error: ' + e);
		}
		printName(arguments.callee,'Finished');
	}
}

//receives function. Prints it's name
function printName(function_,cond_){
	console.log(function_.name + ' ' +cond_);
}

//prints name and error. used in ajax
function errPrint_(f_,e_){
	console.log('Function:' + f_ + ' Throws err: ' + e_);
}

function scrollTrack(){
  console.log(["scrollTrack Fired",window.document.body.scrollTop ]);
}
//---------------------------------



//Loading external JS in vanilla js

//---------------------------------
function dynamicallyLoadScript(url,callback) {
  if(!isUndefined(document)){
    var script = document.createElement("script"); // Make a script DOM node
    if(!isUndefined(script)){
      script.src = url; // Set it's src to the provided URL
      if(!isUndefined(document.head)){

        script.onreadystatechange = callback;
        script.onload = callback;
        document.head.appendChild(script); // Add it to the end of the head section of the page (could change 'head' to 'body' to add it to the end of the body section instead)
        log(["script :",script])

      }
    }
  }
}




//Initialization

//---------------------------------

function initialize(){
  if(!isUndefined(window)
  && !isUndefined(window.document)
  && !isUndefined(window.document.readyState) ){

    addListeners();

    log("window and document inited")

    dynamicallyLoadScript("js_2.js",loaded_js_2);

    let document_=window.document;
    let document_body=document_.body;

    genFlexes();

    log(window);
    log(document_);
    log(document_body);
  }
}

function addListeners(){
  window.document.body.addEventListener("scroll", scrollTrack);
}

//executes on load
document.addEventListener("DOMContentLoaded", initialize);
//window.onload=function(){  log("window.onload")  init();}




//JS Loaded part

//---------------------------------
var loaded_js_2= function(){
  tolog("External log loaded");
};





//==============================================================================






//Generation part

//---------------------------------

//append tag creates element returns child

function createAndAppendChild(tag,parent_){
  let parent=Object;
  if(!isUndefined(parent_)){parent=curDoc_()}else {
    parent=parent_;
  }

  let elem=createElement(parent,tag);
  let ch=appendChild_(curBody_(),elem);
  return ch;
}

//inserts innerHTML to child

function createAndAppendChildWithInner(tag,text){
 let ch=createAndAppendChild("div");
 let ih=innerHTML_(ch,text);
}


//Usage part

//---------------------------------

function gen(tag,text,count){
  for(o=0;o<Math.abs(count);o++){
    createAndAppendChildWithInner(tag,text + (o+1))
  }
}

function appendTag(flex_,tag_){
  let flex=Object;
  let tag="div";

  if(!isNull(tag_)){
    //console.log("Tag not null",tag,tag_);
    tag=tag_;
  }

  let f=createElement(curDoc_(),tag);

  if(isNull(flex_)){
    //console.log("flex is null")
    flex=appendChild_(curBody_(),f);
  }else{
      //console.log("flex is ",flex,f) ;
    flex=appendChild_(flex_,f);

  }

  return flex;
}

function fillChildTagCount(flex_,tag_,text_,count_){
  let flex=Object;
  let tag="div";
  let count=3;

  if(!isUndefined(tag_)){tag=tag_;}
  if(!isUndefined(count_)){count=count_;}


  let f=createElement(curDoc_(),tag);

  if(isNull(flex_)){
    flex=appendChild_(curBody_(),"div");
  }else{flex=flex_;}

  for(let i=0;i<count;i++){
    let elem=createElement(curDoc_(),tag);
    let ch=appendChild_(flex,elem);

    if(!isNull(text_)){let ih=innerHTML_(ch,text_);}

  }

  return flex;
}

//Generates hierarhy of 3x3x3 flex containers column,row,column

function genFlexes(){

  let hdrBx=appendTag(curBody_(),"div");
  setAttribute(hdrBx,"class","fxtst");
  // setAttribute(hdrBx,"class","hdr");

  // let hdrFx=appendTag(hdrBx,"div");
  // setAttribute(hdrFx,"class","fxtst");
  // setAttribute(hdrFx,"class","flex-container");

  // let hdr=appendTag(hdrFx,"div");
  // setAttribute(hdr,"class","flex-container fxvt");

  for(i4=0;i4<10;i4++){
    // let ch=appendTag(hdr,"div");
    let txt=" {" + i4 + "} Genned " + ";";
    let ch0=fillChildTagCount(hdrBx,"p",txt,1);
  }

  for(let i =0;i<hdrBx.childNodes.length;i++ ){
    setAttribute(hdrBx.childNodes[i],"class","fxitmtst");
  }





  let i0=0;
  let fa=appendTag();
  setAttribute(fa,"class","menu");

  let f0=appendTag(fa);
  setAttribute(f0,"class","flex-container");

  for(let i=0;i<2;i++){
    let f1 = appendTag(f0);
    //console.log("flex " +i,f1)
    setAttribute(f1,"class","flex-container fxvt");

      for(let i2=0;i2<3;i2++){
        let f2 = appendTag(f1);
        //console.log("flex " +i2,f2)
        setAttribute(f2,"class","flex-container fxhr");

          for(let i3=0;i3<5;i3++){
            let f3 = appendTag(f2);
            //console.log("flex " +i3,f3)

            setAttribute(f3,"class","flex-container fxvt");

            for(i4=0;i4<3;i4++){
              let txt=" {" + i0 + "} item in "+i+","+i2+","+i3+"; ";
              fillChildTagCount(f3,"div",txt,1);
              i0+=1;
            }

          }
      }

  }

}
