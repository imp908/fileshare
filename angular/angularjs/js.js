var myApp = angular.module('myApp',[]);

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
	$scope.GET_=function(depth){console.log($scope.list_); AJQGet(this.testUrl,depth); }

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
	//var objAJ=JSON.stringify(obj_);
	var objAJ=obj_;
	console.log("stringified object:" + objAJ);

	$.ajax({
	type:method_,
	url: restServerURL ,
	accepts: "application/json",
	dataType:"json",
	data:objAJ,
	xhrFields: {
		withCredentials: true
	},
	//beforeSend: function (xhr) {        xhr.setRequestHeader('Authorization', make_base_auth("neo4j", "root"));},
	success: function(data, xhr, textStatus){
				  //alert("query success!");
		  //process query results here

		 //alert(JSON.stringify(data, null, 4));
		 console.log(data);
	},
	error:function(jqXHR, textStatus, errorThrown){
				   alert(errorThrown);
	}
	});
}

function AJQGet(URL_,depth_)
{
	restServerURL=URL_+"/"+depth_;

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
				   alert(errorThrown);
	}
	});
}
