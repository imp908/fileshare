/**
 * Created by apl on 03.12.2016.
 */
var app = angular.module('app', ['wu.masonry']) // ,'ui.bootstrap','ngTouch','ngAnimate'

app.directive('onScrollToBottom', function ($document) {
  // This function will fire an event when the container/document is scrolled to the bottom of the page
  return {
    restrict: 'A',
    link: function (scope, element, attrs) {
      var doc = angular.element($document)[0].body

      $document.bind('scroll', function () {

        // console.log('in scroll')
        // console.log("scrollTop + offsetHeight:" + (doc.scrollTop + doc.offsetHeight))
        // console.log("scrollHeight: " + doc.scrollHeight)

        if (doc.scrollTop + doc.offsetHeight >= doc.scrollHeight - 100) {
          // run the event that was passed through
          scope.$apply(attrs.onScrollToBottom)
        }
      })
    }
  }
})

// .directive('testD', function() {
//   return function(scope, element, attrs) {
//     console.log(attrs)
//     // if (scope.$last){
//     //   scope.$emit('LastElem')
//     // }
//     // scope.$watch('thing', function(){
//     //   var r = (Math.random()*255).toFixed(0)
//     //   var g = (Math.random()*255).toFixed(0)
//     //   var b = (Math.random()*255).toFixed(0)
//     //   angular.element(element).css('color','rgb('+r+','+g+','+b+')')
//     // })
//   }
// })

app.controller('AppCtrl', function ($scope, $http, $sce, $rootScope) {
  $scope.categories = []
  $scope.sugestion = []
  $scope.news = []
  $scope.presenter = true
  $scope.gal = []
  $scope.galSRC = []
  $scope.UserSettings = {}
  $scope.birthday = []
  $scope.cnt = {}
  $scope.Favorites = []

  // $scope.StartLoad =  function () {
  //   $('#ololo').show()
  //   $('#blur').addClass('blur2px')
  // }
  // $scope.FinishLoad =  function () {
  //   $('#ololo').hide()
  //   $('#blur').removeClass('blur2px')
  // }

  $scope.StartLoad =  function () {
    $('#vloader').show()
    $('.vdatarow').hide()
    // $('#blur').addClass('blur2px')
  }
  $scope.FinishLoad =  function () {
    $('#vloader').hide()
    $('.vdatarow').show()
    // $('#blur').removeClass('blur2px')
  }

  // $scope.htmlPopover = $sce.trustAsHtml('<b style="color: red">I can</b> have <div class="label label-success">HTML</div> content')

  // var SearchPersonUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchPerson/'
  // var NewsFeedUrl = 'http://nspk.online/api/news.php?list=0&num=25'
  // var AccountUrl = 'http://msk1-vm-inapp01:8081/api/Account/'
  // var SearchByUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/SearchByFNameLName/'
  // var MenuUrl = 'static.data/static.menu.json'
  // var GalUrl = 'static.data/gal.json'
  // // var VacationsUrl = 'http://msk1-vm-ovisp01:8085/api/Person/HoliVationAcc'
  // var VacationsUrl = 'http://msk1-vm-ovisp01:8085/api/Person/HoliVation/saa'
  // var NoobsUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetPersonsLastTwoWeeks/last'
  // var UserSettingsUrl = 'http://msk1-vm-ovisp01:8084/api/UserSettings/'
  // var FavoritesUrl = 'http://msk1-vm-ovisp01:8084/api/PersonRelation/'
  // var PersonBirthdays = 'http://msk1-vm-ovisp01:8084/api/PersonBirthdays'


  var SearchPersonUrl = 'http://msk1-vm-ovisp01:84/api/Structure/SearchPerson/'
  var NewsFeedUrl = 'http://nspk.online/api/news.php?list=0&num=25'
  var AccountUrl = 'http://msk1-vm-ovisp01:8084/api/Account/'
  var SearchByUrl = 'http://msk1-vm-ovisp01:84/api/Structure/SearchByFNameLName/'
  var MenuUrl = 'static.data/static.menu.json'
  var GalUrl = 'static.data/gal.json'
  // var VacationsUrl = 'http://msk1-vm-ovisp01:8084/api/Person/HoliVationAcc'
  var VacationsUrl = 'http://msk1-vm-ovisp01:8085/api/Person/HoliVation/aleksandrovnv'
  var NoobsUrl = 'http://msk1-vm-inapp01.nspk.ru:81/api/Structure/GetPersonsLastTwoWeeks/last'
  var UserSettingsUrl = 'http://msk1-vm-ovisp01:8084/api/UserSettings/'
  var FavoritesUrl = 'http://msk1-vm-ovisp01:8084/api/PersonRelation/'
  var PersonBirthdays = 'http://msk1-vm-ovisp01:8084/api/PersonBirthdays'
  //var VacationsUrl = 'http://msk1-vm-ovisp01:8085/api/Person/HoliVationAcc'
  // $scope.showFull = function (path){
  //   console.log(path)
  // } 

  // $http.get(TestCollectUrl).then(function (respData) {
  //   $scope.birthday = JSON.parse(respData.data)
  // })

  function compareItems (a, b) {
    if ((a.birthday.split('.')[1] * 100) + a.birthday.split('.')[0] > ((b.birthday.split('.')[1] * 100) + b.birthday.split('.')[0])) return 1
    if ((a.birthday.split('.')[1] * 100) + a.birthday.split('.')[0] < ((b.birthday.split('.')[1] * 100) + b.birthday.split('.')[0]))  return -1
    if ((a.birthday.split('.')[1] * 100) + a.birthday.split('.')[0] == ((b.birthday.split('.')[1] * 100) + b.birthday.split('.')[0])) return 0
  }

  $http({
    method: 'GET',
    url: PersonBirthdays,
    withCredentials: true
  }).then(function (respData) {
    //console.log(respData.data)
    $scope.birthday = respData.data.sort(compareItems)
  })

  $scope.GetFavorites = function () {
    $http({
      method: 'GET',
      url: FavoritesUrl,
      withCredentials: true
    }).then(function (respData) {
      if (respData.data.length) {
        respData.data.forEach(function(item, i, arr) {
          arr[i].fvr = true})
        $scope.Favorites = respData.data
        $scope.presenter = false
        $scope.news.length = 0
        $scope.contacts = $scope.Favorites
        console.log(respData.data)
      // PiwikTrack("AddressBook", "Search", str)
      }else {    $.Notify({
          caption: 'Внимание',
          content: 'Раздел избранное пуст',
          type: 'warning',
          icon: "<span class='mif-warning'></span>"
        });}
    })
  }

  // $scope.ProceedContacts = function (str) {
  //   $http.get(SearchPersonUrl + str).then(function (respData) {
  //     $scope.contacts = respData.data
  //     $scope.contacts.forEach(elm => {
  //       $scope.Favorites.forEach(fv => {
  //         if (elm.id == fv.id) {elm.fvr = true} else {elm.fvr = false}
  //       })
  //       if (!$scope.Favorites.length) {elm.fvr = false}
  //     })
  //     $scope.presenter = false
  //     $scope.news.length = 0
  //     PiwikTrack('AddressBook', 'Search', str)
  //   })
  // }


  $scope.ProceedContacts = function (str) {
    $http.get(SearchPersonUrl + str).then(function (respData) {
      $scope.contacts = respData.data
      $scope.contacts.forEach(function(item, i, arr) {
        $scope.Favorites.forEach(function(item_, j, arr_) {
          if (item.id == item_.id) 
          { arr[i].fvr = true } 
          // else {arr[i].fvr = false}
        })
      })
      if (!$scope.Favorites.length) {$scope.contacts.forEach(function(item, i, arr) {arr[i].fvr = false})}
      $scope.presenter = false
      $scope.news.length = 0
      console.log(respData.data)
      PiwikTrack('AddressBook', 'Search', str)
    })
  }

  $scope.SearchContacts = function (str) {
    $http({
      method: 'GET',
      url: FavoritesUrl,
      withCredentials: true
    }).then(function (respData) {
      $scope.Favorites = respData.data
      console.log(respData.data)
      $scope.Favorites.forEach(function(item, i, arr) {
        arr[i].frv = true})
      $scope.ProceedContacts(str)
    })
  }

  $scope.FavoritesClick = function (e, obj) {
    if ($scope.contacts[e].fvr) { 
      $scope.contacts[e].fvr = false;
      $http({
        method: 'GET',
        url: FavoritesUrl + $scope.contacts[e].id,
        // headers: { 'Content-Type': 'text/plain' },
        // data: '"' + $scope.contacts[e].id + '"',
        withCredentials: true
      }).then(function (respData) {
        // console.log(respData)
      })      
    } else { 
      $scope.contacts[e].fvr = true
      $http({
        method: 'POST',
        url: FavoritesUrl,
        headers: { 'Content-Type': 'text/plain' },
        data: '"' + $scope.contacts[e].id + '"',
        withCredentials: true
      }).then(function (respData) {
      })  
    }
    console.log('"' + $scope.contacts[e].id + '"')
  }
  $scope.StartLoad() 
  $http({
    method: 'GET',
    url: VacationsUrl,
    withCredentials: true
  }).then(function (respData) {
    // console.log(respData.data)
    $scope.Vacations = respData.data
    $('#example').DataTable({
      data: $scope.Vacations.Vacations,
      paging: false,
      searching: false,
      ordering: false,
      info: false,
      // ajax: "data/orthogonal.txt",
      columns: [
        { data: 'LeaveType' },
        { data: 'DateStart' },
        { data: 'DateFinish' },
        { data: 'DaysSpent' }
      ]
    })

    $('#example2').DataTable({
      data: $scope.Vacations.Graph,
      paging: false,
      searching: false,
      ordering: false,
      info: false,
      // ajax: "data/orthogonal.txt",
      columns: [
        { data: 'LeaveType' },
        { data: 'DateStart' },
        { data: 'DateFinish' },
        { data: 'Days' }
      ]
    })

    $scope.FinishLoad(); 
  })



  $http.get(MenuUrl).then(function (respData) {
    $scope.categories = buildTree(respData.data.menu, 'categories')
  })

  $scope.clickDialog = function (obj) {
    $scope.cnt = obj
    PiwikTrack('AddressBook', 'bdayPanel', obj.title)
    metroDialog.toggle('#dialog9')
  }

  // $http.get(AccountUrl).then(function (respData) {
  //   $scope.account =respData.data
  //   console.log($scope.account)
  // })

  $scope.changeShowBirthday = function () {
    if ($scope.UserSettings.showBirthday) {$scope.UserSettings.showBirthday = true}else {$scope.UserSettings.showBirthday = false}
    // console.log($scope.UserSettings.showBirthday)

    $http({
      method: 'POST',
      url: UserSettingsUrl,
      headers: { 'Content-Type': 'text/plain' },
      data: $scope.UserSettings,
      // xhrFields: { withCredentials: true },
      withCredentials: true
    }).then(function (respData) {
       console.log(respData.data)
    })
  }

  $http({
    method: 'GET',
    url: UserSettingsUrl,
    withCredentials: true
  }).then(function (respData) {
    $scope.UserSettings = JSON.parse(respData.data.result[0].this)
    // console.log(respData.data)
  })

  $http({
    method: 'GET',
    url: AccountUrl,
    withCredentials: true
  }).then(function (respData) {
    $scope.account = respData.data
  // console.log($scope.account)
  })

  $http.get(NoobsUrl).then(function (respData) {
    $scope.noobs = respData.data
  // console.log($scope.noobs)
  })

  $http.get(NewsFeedUrl).then(function (respData) {
    $scope.news = respData.data.news
  })

  $http.get(GalUrl).then(function (respData) {
    $scope.galSRC = respData.data.gal
    $scope.gal = $scope.galSRC.slice(0, 15)
  })

  $scope.ain = new Awesomplete(document.getElementById('inpuut'), {
    list: $scope.sugestion,
    data: function (item, input) {
      return {
        label: item.suggestion, value: item.suggestion
      }
    }
  })
  var galSRCndx = 15
  $scope.loadNext = function () {
    // $scope.gal = $scope.gal.concat($scope.galSRC.slice(galSRCndx, galSRCndx + 15))
    // galSRCndx = galSRCndx + 15
    // console.log($scope.galSRC)
  }

  // --------------------------------------
  angular.element(document).ready(function () {})
  // --------------------------------------

  $scope.goHome = function (e) {
    // console.log($rootScope)
    // console.log($scope.presenter)
    if ($scope.news.length == 0)
      $http.get(NewsFeedUrl).then(function (respData) {
        $scope.ain.input.value = ''
        $scope.ain.evaluate()
        $scope.news = respData.data.news
        $scope.contacts.length = 0
        $scope.presenter = true
      })
  }

  $scope.search_input = function (e) {
    if ((e.target.value.length == 0) && (e.originalEvent.keyCode == 27) && ($scope.news.length == 0)) {
      $http.get(NewsFeedUrl).then(function (respData) {
        $scope.news = respData.data.news
        $scope.contacts.length = 0
        $scope.presenter = true
      })
    }

    if (e.target.value.length > 3) {
      if (e.originalEvent.keyCode == 27) {
        $scope.ain.input.value = ''
        $scope.ain.evaluate()
      }
      if (e.originalEvent.keyCode == 13) {
        $scope.ain.close()
        $scope.SearchContacts($scope.ain.input.value)
      }
      if (jQuery.inArray(e.originalEvent.keyCode , [13, 27, 37, 38, 39, 40]) == -1) {
        $http.get(SearchByUrl + e.target.value).then(function (respData) {
          $scope.sugestion = respData.data
          // console.log($scope.sugestion)
          $scope.ain.list = $scope.sugestion
          $scope.ain.evaluate()
        })
      }
    }
  }

  document.getElementById('inpuut').addEventListener('awesomplete-select', function (e) {
    var swt_ = false
    if (typeof $(e.origin)['0'].attributes['0'] == 'undefined') { swt_ = true } else { swt_ = ($(e.origin)['0'].attributes['0'].nodeValue == 'false') }
    if (swt_) { $scope.SearchContacts(e.text.label) }
  })
})

// $(window).scroll(function (e) {
//   // SmartSticky()
//   console.log($(window).scrollTop())
//   console.log($(window).scrollLeft())
// })
