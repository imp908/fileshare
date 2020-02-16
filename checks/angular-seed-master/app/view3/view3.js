'use strict';

angular.module('myApp.view3', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/view3', {
    templateUrl: 'view3/view3.html',
    controller: 'view3Ctrl'
  });
}])

.controller('view3Ctrl', ['$scope','$http','$cacheFactory',
  function($scope,$http,$cacheFactory) {
    var ctrl = this;
    ctrl.parseResponse = parseResponse;

    $scope.item = {value: 'value for 3', show: false};
    $scope.chSt = changeState;
    $scope.httpSend = httpSend;
    $scope.getCachedResp = getCachedResp;

    function changeState() {
      $scope.item.show = !$scope.item.show;
    };

    function httpSend(){
      let url = 'http://localhost:8083/';

      let param = {cache: true};
      var resp = getCachedResp();
      if(resp)
      {
        console.log('from cache')
        console.log(resp)
      }
      if(!resp)
      {
        //let param = {test:'test message'}
        $http.get(url,param)
        //$http({method: 'POST', url: url})
        .then(function successCallback(response) {
            // this callback will be called asynchronously
            // when the response is available
            console.log('angjs view3 resp',response);

        }, function errorCallback(response) {
            // called asynchronously if an error occurs
            // or server returns response with an error status.
            console.log('angjs view3 err',response);
        })
      }


    }

    function getCachedResp()
    {

      //https://www.themarketingtechnologist.co/caching-http-requests-in-angularjs/
      // $http.get('api/path',{cache: true});
      // var httpCache = $cacheFactory.get('$http');
      // var cachedResponse = httpCache.get('api/path');
      // httpCache.remove('api/path');

      console.log('finally')
      let url = 'http://localhost:8083/';
      var httpCache = $cacheFactory.get('$http');
      var cachedResponse = httpCache.get(url);
      var cachedResp1 = parseResponse(cachedResponse);
      if(cachedResp1)
      {
        return cachedResp1;
      }
      return null;

    }

    function parseResponse(cachedResponse){
      if(cachedResponse){
        console.log('parseResponse');
        console.log(cachedResponse);
        if(cachedResponse && Array.isArray(cachedResponse) && cachedResponse.length > 0
        && cachedResponse[1] && typeof cachedResponse[1] === 'string')
        {
          let message = JSON.parse(cachedResponse[1]);
          let body = (message && typeof message === 'object' && message.body) ? message.body : null;
          if(body)
          {
            return body;
          }
        }
      }
      return null;
    }

  }

]);
