var myApp = angular.module('myApp',[]);

var array = [
	{id: 1, date:'Mar 12 2012 10:00:00 AM'},{id: 2, date:'Mar 8 2012 08:00:00 AM'},
	{id: 3, date:'Mar 1 2012 10:00:00 AM'},{id: 4, date:'Mar 13 2012 08:00:00 AM'}
];

function ArrSorttest(){
	var arrSorted=
	array.sort(function(a,b){
		var c= new Date(a.date);
		var d= new Date(b.date);
		return c-d;
	});
	console.log(arrSorted);
}
function ArrSorttestDesc(){
	var arrSorted=
	array.sort(function(a,b){
		var c= new Date(a.date);
		var d= new Date(b.date);
		return d-c;
	});
	console.log(arrSorted);
}

myApp.controller('TestController', ['$scope', function($scope) {
	$scope.ArrSorttest=function(){ArrSorttest(); }
	$scope.ArrSorttestDesc=function(){ArrSorttestDesc(); }
	$scope.ownnewsShow=true;
}]);

myApp.controller('GreetingController', ['$scope', function($scope) {
	$scope.greeting = 'Hola!';
}]);

myApp.controller('CountController', ['$scope', function($scope) {
	$scope.double = function(value) { return value * 2; };
}]);

myApp.controller('LogController', function($scope){

	$scope.customer = {
	  address1 : 'address1',
	  address2 : 'address2',
	  city:'city'
	};

	$scope.log_=function(){	console.log($scope.customer);}
	$scope.thErr_=thErr_;
	$scope.callCatch_=callCatch_;
});

myApp.controller('ListController', function($scope){

	$scope.list_ = [
		{GUID:"0",content_:"ct1"},
		{GUID:"1",content_:"ct2"}
	];

	testUrl="http://msk1-vm-ovisp01:8184/api/news2";

	$scope.bindURL_= function(url_){
		this.testUrl=url_;
	}

	testPOST={GUID:"123",content_:"ct"};
	testPUT={GUID:"123",content_:"ct2"};
	/*
	$scope.POST_=function(){console.log($scope.list_); AJQ(testUrl,"POST",testPOST); }
	$scope.PUT_=function(){console.log($scope.list_); AJQ(testUrl,"PUT",testPUT); }
	*/

	var testObj=testPOST;
	var depth=2;

	$scope.POST_=function(testObj){console.log($scope.list_); AJQ(this.testUrl,"POST",testObj); }
	$scope.PUT_=function(testObj){console.log($scope.list_); AJQ(this.testUrl,"PUT",testObj); }
	$scope.GET_js=function(depth){console.log($scope.list_); AJQGet(this.testUrl,depth); }
	$scope.GET_txt=function(depth){console.log($scope.list_); AJQGetTxt(this.testUrl,depth); }
	$scope.GET_http=function(depth){console.log($scope.list_); xHttpWithCred(this.testUrl); }

});



$.ajaxSetup({
    contentType : 'application/json',
    processData : false
});

$.ajaxPrefilter( function( options, originalOptions, jqXHR ) {
    if (options.data){
        //options.data=JSON.stringify(options.data);
    }
});


function AJQ(restServerURL,method_,obj_)
{
	//var objAJ_=JSON.stringify(obj_);
	var objAJ_=obj_;
	//var objAJ_=encodeURI(obj_);

	console.log("stringified object:" + JSON.stringify(objAJ_));
	console.log("clean object:" + objAJ_);
	console.log("encUri object:" + encodeURI(objAJ_));
	var fName=arguments.callee.name;

	$.ajax({
	type:method_,
	url: restServerURL ,
	accepts: "application/json",
	dataType:"text",
	data:objAJ_,
	xhrFields: {
		withCredentials: true
	},
	//beforeSend: function (xhr) {xhr.setRequestHeader('Authorization', make_base_auth("neo4j", "root"));},
	success: function(data, xhr, textStatus){
				  //alert("query success!");
		  //process query results here

		 //alert(JSON.stringify(data, null, 4));
		 console.log(data);
	}
		//,error:function(jqXHR, textStatus, errorThrown){alert(errorThrown);console.log("Ajax error");}
		,error:function(jqXHR, textStatus, errorThrown){
			errPrint_(fName,errorThrown);
		}
	});
}

function AJQGet(URL_,depth_)
{

	//var objAJ_=JSON.stringify(obj_);
	//var objAJ_=encodeURI(obj_);

	restServerURL=URL_;
	if(typeof depth_!=='undefined'){
		restServerURL=URL_+"/"+depth_;
	}

	var fName=arguments.callee.name;
console.log("URL:" + restServerURL);

		$.ajax({
		type:'GET',
		url: restServerURL ,
		accepts: "application/json",
		dataType:"json",
		xhrFields: {
			withCredentials: true
		},
		//beforeSend: function (xhr) {xhr.setRequestHeader('Authorization', make_base_auth("neo4j", "root"));},
		success: function(data, xhr, textStatus){
	  	//alert("query success!");
		  //process query results here

	 		//alert(JSON.stringify(data, null, 4));
		 	console.log(data);
		},
		error:function(jqXHR, textStatus, errorThrown){
					   //alert(errorThrown);
						 errPrint_(fName,errorThrown);
		}
		});

}

function AJQGetTxt(URL_,depth_)
{

	//var objAJ_=JSON.stringify(obj_);
	//var objAJ_=encodeURI(obj_);

	restServerURL=URL_;
	if(typeof depth_!=='undefined'){
		restServerURL=URL_+"/"+depth_;
	}

	var fName=arguments.callee.name;
	console.log("URL:" + restServerURL);

		$.ajax({
		type:'GET',
		url: restServerURL ,
		accepts: "text\plain",
		xhrfields: { withcredentials: true},
		//beforeSend: function (xhr) {xhr.setRequestHeader('Authorization', make_base_auth("neo4j", "root"));},
		success: function(data, xhr, textStatus){
  	//alert("query success!");
	  //process query results here
 		//alert(JSON.stringify(data, null, 4));
	 	console.log(data);
		},
		error:function(jqXHR, textStatus, errorThrown){
					   //alert(errorThrown);
						 errPrint_(fName,errorThrown);
		}
		});

}

function xHttpWithCred(url_)
{
	var xhttp = new XMLHttpRequest();
	xhttp.open("GET", url_, true);
	xhttp.withCredentials = true;
	xhttp.send();
	xhttp.onreadystatechange = function(){
	if (xhttp.readyState === XMLHttpRequest.DONE) {
	  if (xhttp.status === 200)
	    console.log(xhttp.responseText);
	  else
	    console.log('There was a problem with the request.');
	}
	};
}

function thErr_()
{
	printName(arguments.callee,'Started');
	try{
	 throw new Error('oops');
	}
	catch(e)
	{
		var myName = arguments.callee.toString();
		myName = myName.substr('function '.length);
		myName = myName.substr(0, myName.indexOf('('));

		console.log('Function: '+myName,'Error: '+e.message);
	}
	printName(arguments.callee,'Finished');
}

function callCatch_(function_)
{
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
function printName(function_,cond_)
{
	console.log(function_.name + ' ' +cond_);
}

//prints name and error. used in ajax
function errPrint_(f_,e_)
{
	console.log('Function:' + f_ + ' Throws err: ' + e_);
}
