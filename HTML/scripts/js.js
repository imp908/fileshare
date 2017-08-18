//date formats
var dateFormats =[
	{ "NAME": "DAY", "VALUE" : "dd.mm.yy"},
	{ "NAME": "MONTH", "VALUE" : "M-yy"},
	{ "NAME": "YEAR", "VALUE" : "yy"}
]

//\\\\\\\\\\\\\\\\\
//prototype example
//прототип с методами для нового элемента
var MyTimerProto = Object.create(HTMLElement.prototype);
// свой метод tick
MyTimerProto.tick = function() {
	this.innerHTML=parseFloat(this.innerHTML)+1;
};
//регистрируем новый элемент в браузере
document.registerElement("my-timer", {
prototype: MyTimerProto
});
setInterval(function() {
	timer.tick();
}, 1000*60);
///////////////////

window.onclick = function(event){
console.log("Window clicked event:",event.target);
	//testModule.modalClose(event.target);
};

				
//On load event listeners add
window.onload = function(){
	console.log("Wondow load");
	//testModule.addListeners();
	fc.containsCheck();
	//testModule.getFromArr(dateFormats,"DAY");
	
$('#demo').pagination({
    dataSource: [1, 2, 3, 4, 5, 6, 7, 195],
    callback: function(data, pagination) 
	{
		// template method of yourself
		var html = dateFormats; //template(data);
		//dataContainer.html(html);
    }
});

};

document.addEventListener("DOMContentLoaded",function(){
	console.log("document addEventListener");
	testModule.addListeners();
	console.log("Document loaded");
});

//Module
var testModule = (
	function(){
		
	//constants for module
	var names=["Mon","Tue","Wed","Thi","Fri","Sat","Sun"];
	var idValue="IDValue",
		textValue = "NumValue";

		return {
			GO_: function()
			{
				console.log("GO_ started");			
				this.ArraySort_();
			}
			,ArraySort_: function()
			{
				console.log(this,"started");
				var arr = [5,1,3,4,6,7,9,8,5,2]
				console.log("unsorted",arr);
				var arr2=arr.sort(function(a,b){
				console.log(":",a," -",b,";");
				});
				var arr3=[];
				for(var i=0;i<=300000;i++)
				{
					arr3[i]=i;
				}
				arr3.shift[1];
				arr3.unshift[1];
				console.log("sorted",arr2.sort(),arr3);
			}		
			,dayName : function()
			{
				var sel = document.getElementById(textValue);
				console.log("Selected element :", textValue);
				var num = parseInt(sel.value)-1;			
				console.log("Day name:",names[num]);
				var text_ = document.getElementById(idValue);
				if(names[num])
				{
					text_.value=names[num];
				}				
				console.log("Element :", text_.value);				
				return names[num];
			}				
			,eventLog : function (data,obj)
			{
				console.log("by object :", obj) ;
				console.log("event fired :",data);
			}		
			,weekCheck : function (val)
			{
				if(val<1){
					return 1;
				}
				if(val>7)
				{
					return 7;
				}
			return val;	
			}		
			, addListeners : function ()
			{
				var sel = document.getElementById(textValue);

				if(sel)
				{
					console.log("JS Add event to :" , sel);
					//sel.addEventListener("onmouseover",function (){ console.log("Moove")});
					sel.onchange=function (data){
						this.value=testModule.weekCheck(parseInt(this.value));
					};
				}
				
				sel = document.getElementById(idValue);
				
				if(sel)
				{
					console.log("JS Add event to :" , sel);
					//sel.addEventListener("onmouseover",function (){ console.log("Moove")});
					sel.onmouseover=function (data){
						console.log("Over!!");
					};
					sel.onmouseout=function (data){
						console.log("Out!!");
					};
				}
				
				//closures as events add
				sel = document.getElementById("button2");
				if(sel)
				{								
					sel.onclick = function(){
						//console.log("clicked",this);
						counter();
						outer();
					};
				}
				
				//custom closures
				sel_ = document.getElementsByClassName("tabsli");
				if(sel_)
				{				
					console.log("Sel type ",typeof(sel_),sel_.length);
					for(var i =0;i<sel_.length;i++)
					{
						console.log("Sel item ",sel_[i]);
						sel_[i].setAttribute("onclick","testModule.tabClick(event)")
					}
					
					//sel_.onclick = this.tabClick();
					//sel_.setAttribute("onclick","testModule.tabClick()")
				}
				
				
				sel_ = document.getElementById("modal");
				
				if(sel_){
					console.log("modals");
					sel_.onclick =
					function(event){
						console.log("Modal click ",event.target);
						testModule.modalClose(event);
					};
				}
				else{console.log("No modals");}
				
				sel_ = document.getElementById("optionSelect");
				if(sel_)
				{
					console.log("option select events");
					
					sel_.size=1;
					
					sel_.onmouseover = function(){
							testModule.dropDownEnlarge (this) ;
						};						
					sel_.onmouseout = function(){
						testModule.dropDownDecrease (this) ;
					};
				}
				
			}
			, dropDownEnlarge : function (element_)
			{
				console.log("dropDownEnlarge ", element_);
				element_.size = element_.length;
			}
			, dropDownDecrease : function (element_)
			{
				console.log("dropDownDecrease ", element_);
				element_.size = 1;
			}
			, tabClick : function(event)
			{
				var items = document.getElementsByClassName("tabselem");
				for(var i =0;i<items.length;i++)
				{
					items[i].style.display="none";
				}
				var item = document.getElementById(event.currentTarget.id.replace("_",""));
				if(item)
				{
					item.style.display="block";
				}
				console.log("Custom tab clicked :",event.currentTarget.id);
			}
			, log : function(data)
			{
				console.log("Log : ", data)	;
			}
			, getValArr2d : function (data,val)
			{
				var arr1=[];
				for(var i = 0;i<data.length;i++)
				{
					arr1=data[i];
					if(arr1["NAME"]==val)
					{
						console.log(arr1["VALUE"]);
					}					
				}
			}
			/*Multi tab script*/
			, openCity : function(evt,cityName)
			{
				var i, tabcontent, tablinks;

				// Get all elements with class="tabcontent" and hide them
				tabcontent = document.getElementsByClassName("tabcontent");
				for (i = 0; i < tabcontent.length; i++) {
					tabcontent[i].style.display = "none";
				}

				// Get all elements with class="tablinks" and remove the class "active"
				tablinks = document.getElementsByClassName("tablinks");
				for (i = 0; i < tablinks.length; i++) {
					tablinks[i].className = tablinks[i].className.replace(" active", "");
				}

				// Show the current tab, and add an "active" class to the link that opened the tab
				document.getElementById(cityName).style.display = "block";			
				evt.currentTarget.className += " active";				
				
			}			
			, modalClose : function(event)
			{
				console.log("Modal close");
				var modal = document.getElementById("modal");
				var tag = document.getElementById("model-tag");
				
				if(event.target===modal || event.target === tag){
					modal.style.display="none";				
				}
			}
			, modalShow : function()
			{
				console.log("Modal show");
				var modal = document.getElementById("modal");
				modal.style.display="block";
			}
			, modalListGen : function()
			{
				var elem = document.getElementById("modal-content");
				if(elem){
					//var child = document.createElement();
				}
			}
			
		}
		
	}()
);

//module 2
var fc = (
function(){
	return{

	containsCheck : function(){
		var arr = ["A","B","C"];
		console.log("Array " , arr ," includes A ", arr.includes("A"));
	}
	,max : function(arr){
		var min=0;		
		for(var i=0;i<arr.length;i++)
		{
			if(i==0){min=arr[i]};
			if(min<arr[i]){min=arr[i];}
		}
		return min;
	}
	,min : function(arr){
		var max=0;		
		for(var i=0;i<arr.length;i++)
		{
			if(i==0){max=arr[i]};
			if(max>arr[i]){max=arr[i];}
		}
		return max;
	}
	, randomCheck : function(arr){
		console.log("Min val:",fc.min(arr),"max val:",fc.max(arr));
	}
	, randomArr : function()
	{
		var arr=[];
		var length=Math.round(Math.random()*10+1);
		for(var i=0;i<length;i++)
		{
			var val = Math.random()*10-1+1;
			arr.push(val);
		}
		console.log("Generated arr length of:",length); console.dir(arr);
		return arr;
	}

			}			
			}
());

//closure 1
var counter = (function ctp()
{
	//console.log("counter fired");
	var ct = 0;
	
	return function ()
	{	
		//console.log("ctp fired");
		console.log(ct);
		return ct+=1;
	}	
	
})();


//closure 2
var outer =(function out() {
	var outerVar=0;

	return function() {
		var innerVar=1;		
		outerVar+=innerVar;
		console.log(outerVar);
		return outerVar;
	}	
}
)();


var elemntsModule =(
function(){
	return{
		FunctionToID : function(elementName,functionName){
			var elem = document.getElementById(elementName);
			console.log(elem);
		}
	}
	
}()
);
