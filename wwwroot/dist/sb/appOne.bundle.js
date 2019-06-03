var appOne =
/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "F:\\backup\\workflow\\files\\PROJS\\core\\MVC\\mvccoresb\\wwwroot\\dist";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./js/Libs/AppOne/appOne.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./js/Libs/AppOne/Emiter.js":
/*!**********************************!*\
  !*** ./js/Libs/AppOne/Emiter.js ***!
  \**********************************/
/*! exports provided: EmiterNew */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EmiterNew", function() { return EmiterNew; });
///Sample emmitting event module
var En = function (module) {
  var registerEventModuleCallback;

  module.Init = function () {
    console.log('EmiterNew inited');
  };

  module.RegisterEventModule = function (callback) {
    console.log(callback);
    registerEventModuleCallback = callback;
  };

  module.Emit = function (e) {
    //console.log('Emit');
    if (registerEventModuleCallback) {
      registerEventModuleCallback(e);
    }
  };

  return module;
}(En || {});

function EmiterNew() {
  return En;
}

/***/ }),

/***/ "./js/Libs/AppOne/Listener.js":
/*!************************************!*\
  !*** ./js/Libs/AppOne/Listener.js ***!
  \************************************/
/*! exports provided: ListenerNew */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ListenerNew", function() { return ListenerNew; });
//Sample listener module
var Lstn = function (module) {
  module.Init = function () {
    console.log('ListenerNew inited');
  };

  module.Listen = function () {
    console.log('ListenOne');
  };

  return module;
}(Lstn || {});

function ListenerNew() {
  return Lstn;
}

/***/ }),

/***/ "./js/Libs/AppOne/ListenerTwo.js":
/*!***************************************!*\
  !*** ./js/Libs/AppOne/ListenerTwo.js ***!
  \***************************************/
/*! exports provided: ListenerNewTwo */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ListenerNewTwo", function() { return ListenerNewTwo; });
//Sample listener module
var Lstn = function (module) {
  module.Init = function () {
    console.log('ListenerNew inited');
  };

  module.Listen = function () {
    console.log('ListenTwo');
  };

  return module;
}(Lstn || {});

function ListenerNewTwo() {
  return Lstn;
}

/***/ }),

/***/ "./js/Libs/AppOne/appOne.js":
/*!**********************************!*\
  !*** ./js/Libs/AppOne/appOne.js ***!
  \**********************************/
/*! exports provided: SomeModuleOne, Mediator */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "SomeModuleOne", function() { return SomeModuleOne; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "Mediator", function() { return Mediator; });
/* harmony import */ var _lib_EventBus_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../lib/EventBus.js */ "./js/Libs/lib/EventBus.js");
/* harmony import */ var _Listener_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Listener.js */ "./js/Libs/AppOne/Listener.js");
/* harmony import */ var _Emiter_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Emiter.js */ "./js/Libs/AppOne/Emiter.js");
/* harmony import */ var _ListenerTwo_js__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./ListenerTwo.js */ "./js/Libs/AppOne/ListenerTwo.js");
//
//importing service sample bus




var eventOne = {
  type: "eventOne",
  message: "eventOne message"
};
var eventTwo = {
  type: "eventTwo",
  message: "eventTwo message" ///Test module to check initialization in MVC

};

var Smo = function (module) {
  module.Init = function () {
    console.log('SomeModuleOne inited');
  };

  return module;
}(Smo || {});

function SomeModuleOne() {
  return Smo;
}

var Mdtr = function (module) {
  //imported service bus for registration:
  //listener-[event]->emiter
  var eventModule = _lib_EventBus_js__WEBPACK_IMPORTED_MODULE_0__["EventModule"];
  var listenerNew = _Listener_js__WEBPACK_IMPORTED_MODULE_1__["ListenerNew"];
  var emiterNew = _Emiter_js__WEBPACK_IMPORTED_MODULE_2__["EmiterNew"];
  var listenerNewTwo = _ListenerTwo_js__WEBPACK_IMPORTED_MODULE_3__["ListenerNewTwo"];

  module.New = function () {
    return new module();
  };

  module.Init = function () {
    console.log('Mediator inited');
  };

  module.Register = function () {
    console.log('Mediator Register inited');
    console.log(eventOne); //register eventone line (all from mvc button click)

    eventModule().AddEventListener(eventOne, listenerNew().Listen);
    emiterNew().RegisterEventModule(eventModule().DispatchEvent);
    eventModule().AddEventListener(eventTwo, listenerNewTwo().Listen);
    emiterNew().RegisterEventModule(eventModule().DispatchEvent);
  };

  module.EmitFromMediator = function () {
    emiterNew().Emit(eventOne);
    module.EmitFromMediatorTwo();
  };

  module.EmitFromMediatorTwo = function () {
    emiterNew().Emit(eventTwo);
  };

  return module;
}(Mdtr || {});

function Mediator() {
  return Mdtr;
}

/***/ }),

/***/ "./js/Libs/lib/EventBus.js":
/*!*********************************!*\
  !*** ./js/Libs/lib/EventBus.js ***!
  \*********************************/
/*! exports provided: EventModule, ListenerModule, EmitModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EventModule", function() { return EventModule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ListenerModule", function() { return ListenerModule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EmitModule", function() { return EmitModule; });
//Event module check
//--------------------------------------
var EventsTestModule = function (_module) {
  var eventWithModulesCheck = function eventWithModulesCheck() {
    console.log("eventWithModulesCheck");
    var _eventModule = EventModule;
    var _listenerModule = ListenerModule;
    var _emitModule = EmitModule;
    var event01 = {
      type: 'ev0',
      message: "first event"
    };

    _eventModule.AddEventListener(event01, _listenerModule.Receive);

    _emitModule.Emit(event01, _eventModule.DispatchEvent);

    var event02 = {
      type: 'ev1',
      message: "new event"
    };

    _eventModule.AddEventListener(event02, _listenerModule.Receive);

    _emitModule.Emit(event02, _eventModule.DispatchEvent);

    var _emitModule2 = AnotherEmitModule;

    _emitModule2.OtherEmit(event01, _eventModule.DispatchEvent);

    console.log("All to all listen");
    var lsitenerModule2 = AnotherListenerModule;

    _eventModule.AddEventListener(event01, lsitenerModule2.AnotherReceive);

    _eventModule.AddEventListener(event02, lsitenerModule2.AnotherReceive);

    _emitModule.Emit(event01, _eventModule.DispatchEvent);

    _emitModule.Emit(event02, _eventModule.DispatchEvent);

    _emitModule2.OtherEmit(event01, _eventModule.DispatchEvent);

    _emitModule2.OtherEmit(event02, _eventModule.DispatchEvent);
  }; //!TODO:=> move to mediator module


  var preregisteredEmiterCheck = function preregisteredEmiterCheck() {
    var event1 = {
      type: "event3",
      message: "event3 message"
    };
    var event2 = {
      type: "event4",
      message: "event4 message"
    };
    var _eventEmiterListenerRegistrator = EventModule;
    var _listener = ListenerModule;
    var _preregisteredEmiter = EmitModulePreregistered; //register event->listener.func

    _eventEmiterListenerRegistrator.AddEventListener(event1, _listener.Receive); //register eventregistrator->eventEmiter.dispatch


    _preregisteredEmiter.RegisterEventModule(_eventEmiterListenerRegistrator.DispatchEvent); //fire registered event to first listener


    console.log("fire registered event to first listener");

    _preregisteredEmiter.Emit(event1);

    var _listenerAnother = AnotherListenerModule; //register another listener for ev1

    _eventEmiterListenerRegistrator.AddEventListener(event1, _listenerAnother.AnotherReceive); //fire registered event to 2 listeners


    console.log("fire registered event to 2 listeners");

    _preregisteredEmiter.Emit(event1); //clear up


    _eventEmiterListenerRegistrator.RemoveEventListener(event1, _listener.Receive);

    _eventEmiterListenerRegistrator.RemoveEventListener(event1, _listenerAnother.AnotherReceive); //no any events fire


    _preregisteredEmiter.Emit(event1);
  };

  var multipleRegistratorsOneEmiterCheck = function multipleRegistratorsOneEmiterCheck() {
    var event1 = {
      type: "event3",
      message: "event3 message"
    };
    var event2 = {
      type: "event4",
      message: "event4 message"
    };
    var _eventEmiterListenerRegistrator = EventModule;
    var _listener = ListenerModule;
    var _eventEmiterListenerRegistrator2 = EventModule;
    var _listener2 = AnotherListenerModule;
    var _preregisteredEmiter = EmitModulePreregistered; //registration event 2 listener 1

    _eventEmiterListenerRegistrator.AddEventListener(event2, _listener.Receive);

    _preregisteredEmiter.RegisterEventModule(_eventEmiterListenerRegistrator.DispatchEvent); //registration event 2 listener 2


    _eventEmiterListenerRegistrator2.AddEventListener(event2, _listener2.AnotherReceive);

    _preregisteredEmiter.RegisterEventModule(_eventEmiterListenerRegistrator2.DispatchEvent); //emit from emit moule


    _preregisteredEmiter.Emit(event2);
  };

  _module.Init = multipleRegistratorsOneEmiterCheck;
  return _module;
}(EventsTestModule || {}); // Example modules
// ---------------
//eventShema={type:{requiered:true},message:{requiered:true}};
//eventmodule registratormodule shema {AddEventListener(),RemoveEventListener(),DispatchEvent()}
///Contains collection of event type to listener registrator


var Evnt = function (_module) {
  _module.listeners = {};

  _module.GetListeners = function () {
    return _module.listeners;
  };

  _module.AddEventListener = function (event, callback) {
    if (!(event.type in _module.listeners)) {
      _module.listeners[event.type] = [];
    }

    _module.listeners[event.type].push(callback);
  };

  _module.RemoveEventListener = function (event, callback) {
    var type;

    if (!event || !event.type || !callback) {}

    type = event.type;

    if (!(type in _module.listeners)) {
      return;
    }

    var stack = _module.listeners[type];

    for (var i = 0, l = stack.length; i < l; i++) {
      if (stack[i] === callback) {
        stack.splice(i, 1);
        return;
      }
    }
  };

  _module.DispatchEvent = function (event) {
    if (!(event.type in _module.listeners)) {
      return true;
    }

    var stack = _module.listeners[event.type];

    for (var i = 0, l = stack.length; i < l; i++) {
      stack[i].call(this, event);
    }
  };

  return _module;
}(Evnt || {});

function EventModule() {
  return Evnt;
} ///Template for listen module

var Lstn = function (_module) {
  _module.Receive = function (e) {
    console.log("ListenerModule Received");
    console.log(e);
  };

  return _module;
}(ListenerModule || {});

function ListenerModule() {
  return Lstn;
} ///template for emiter with registered event module

var Emt = function (_module) {
  var registerEventModuleCallback;

  _module.RegisterEventModule = function (callback) {
    registerEventModuleCallback = callback;
  };

  _module.Emit = function (e) {
    if (registerEventModuleCallback) {
      registerEventModuleCallback(e);
    }
  };

  return _module;
}(Emt || {});

function EmitModule() {
  return Evnt;
}

var EventRegistratorModule = function (_module) {
  var eventsRegistrator; //{event:{type,message},ListenerModule.AnyFunction,EmitModule.FunctionWithCllback}

  _module.RegisterShema = {
    event: {
      required: true
    },
    listenerF: {
      required: true
    },
    emiterF: {
      required: true
    }
  };

  _module.Register = function (param) {
    //event to listener parameter register
    eventsRegistrator.AddEventListener(param.event, param.listenerF); //register registrator event to emiter

    param.emiterF.call(eventsRegistrator.DispatchEvent);
  };

  _module.Init = function () {};
}(EventRegistratorModule || {});

/***/ })

/******/ });
//# sourceMappingURL=appOne.bundle.js.map