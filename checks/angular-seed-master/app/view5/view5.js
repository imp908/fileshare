'use strict';
//https://www.daterangepicker.com/#example4

angular.module('myApp.view5', ['ngRoute','textAngular'])
.config(['$routeProvider', function($routeProvider,textAngular) {
  angular.lowercase = angular.$$lowercase;
  $routeProvider.when('/view5', {
    templateUrl: 'view5/view5.html',
    controller: 'view5Ctrl',
    css: 'view5.css'
  });


}])

.controller('view5Ctrl', ['$scope',
  function($scope) {

  }

]);
