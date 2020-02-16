'use strict';

// Declare app level module which depends on views, and core components
angular.module('myApp', [
  'ngRoute',
  'myApp.view1',
  'myApp.view2',
  'myApp.view3',
  'myApp.view4',
  'myApp.view5',
  'myApp.version'
]).
config(['$locationProvider', '$routeProvider','$provide', function($locationProvider, $routeProvider,$provide) {
  $locationProvider.hashPrefix('!');
  $routeProvider.otherwise({redirectTo: '/view1'});

  // TextArea plugin
  // ------------------
  $provide.decorator('taOptions', ['taRegisterTool', '$delegate', function(taRegisterTool, taOptions){
		// $delegate is the taOptions we are decorating
		// register the tool with textAngular
		taRegisterTool('colourRed', {
			iconclass: "fa fa-square red",
			action: function(){
				this.$editor().wrapSelection('forecolor', 'red');
			}
		});
    taRegisterTool('colourBlack', {
			iconclass: "fa fa-square blue",
			action: function(){
				this.$editor().wrapSelection('forecolor', 'blue');
			}
		});
    taRegisterTool('colourBlue', {
			iconclass: "fa fa-square black",
			action: function(){
				this.$editor().wrapSelection('forecolor', 'black');
			}
		});
		// add the button to the default toolbar definition
		taOptions.toolbar[1].push('colourRed','colourBlack','colourBlue');
		return taOptions;
	}]);
}]);
