
function log(a){console.log(a)}

//App1 js
var app = angular.module('appOne',[]);

//Controller1 js
app.controller('defaultController', function($scope){
	$scope.id1="init at js id1";
	$scope.id2="init at js id2";
	$scope.color1="red";
		
});

//App2 js
var textApp = angular.module('textapp',[]);
//Controller2 js
textApp.controller('textController', function($scope){
	$scope.textmodel1="input";
	$scope.func1 = function(){
		$scope.textmodel2="Changed " + $scope.textmodel1;
	};
});

var secapp = angular.module('secapp',[]);
secapp.controller('secondaryController', function($scope){
	
});


var arr = ["a","b","c","d"]


$(document).ready(function(){
	
	console.log("Document ready st");
	log(app);
	log(textApp);
	
	
	angular.bootstrap(document.getElementById("App2"), ['textapp']);
	angular.bootstrap(document.getElementById("App3"), ['secapp']);
	
	console.log("Document ready fn");
	
})

