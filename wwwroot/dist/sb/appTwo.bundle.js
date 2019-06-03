var appTwo =
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
/******/ 	return __webpack_require__(__webpack_require__.s = "./js/Libs/AppTwo/appTwo.js");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./js/Libs/AppTwo/EmiterCl.js":
/*!************************************!*\
  !*** ./js/Libs/AppTwo/EmiterCl.js ***!
  \************************************/
/*! exports provided: EmiterCl */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EmiterCl", function() { return EmiterCl; });
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var EmiterCl =
/*#__PURE__*/
function () {
  function EmiterCl() {
    _classCallCheck(this, EmiterCl);
  }

  _createClass(EmiterCl, [{
    key: "RegisterEventModule",
    value: function RegisterEventModule(e) {
      this.emitModule = e;
    }
  }, {
    key: "Emit",
    value: function Emit(e) {
      if (this.emitModule) {
        this.emitModule.DispatchEvent(e);
      }
    }
  }]);

  return EmiterCl;
}();



/***/ }),

/***/ "./js/Libs/AppTwo/ListenerCl.js":
/*!**************************************!*\
  !*** ./js/Libs/AppTwo/ListenerCl.js ***!
  \**************************************/
/*! exports provided: ListenerCl */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ListenerCl", function() { return ListenerCl; });
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var ListenerCl =
/*#__PURE__*/
function () {
  function ListenerCl() {
    _classCallCheck(this, ListenerCl);
  }

  _createClass(ListenerCl, [{
    key: "Listen",
    value: function Listen(e) {
      console.log("Listening in ListenerCl");
      console.log(e);
    }
  }]);

  return ListenerCl;
}();



/***/ }),

/***/ "./js/Libs/AppTwo/MediatorCl.js":
/*!**************************************!*\
  !*** ./js/Libs/AppTwo/MediatorCl.js ***!
  \**************************************/
/*! exports provided: MediatorCl */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "MediatorCl", function() { return MediatorCl; });
/* harmony import */ var _libCl_EventBusCl_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../libCl/EventBusCl.js */ "./js/Libs/libCl/EventBusCl.js");
/* harmony import */ var _libCl_EventOneCl_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../libCl/EventOneCl.js */ "./js/Libs/libCl/EventOneCl.js");
/* harmony import */ var _EmiterCl_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./EmiterCl.js */ "./js/Libs/AppTwo/EmiterCl.js");
/* harmony import */ var _ListenerCl_js__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./ListenerCl.js */ "./js/Libs/AppTwo/ListenerCl.js");
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }







var MediatorCl =
/*#__PURE__*/
function () {
  function MediatorCl() {
    _classCallCheck(this, MediatorCl);

    this.eventBus = new _libCl_EventBusCl_js__WEBPACK_IMPORTED_MODULE_0__["EventModuleCl"]();
    this.emiter = new _EmiterCl_js__WEBPACK_IMPORTED_MODULE_2__["EmiterCl"]();
    this.listener = new _ListenerCl_js__WEBPACK_IMPORTED_MODULE_3__["ListenerCl"]();
    this.eventOne = new _libCl_EventOneCl_js__WEBPACK_IMPORTED_MODULE_1__["EventOneCl"]();
    this.eventTwo = new _libCl_EventOneCl_js__WEBPACK_IMPORTED_MODULE_1__["EventTwoCl"]();
  }

  _createClass(MediatorCl, [{
    key: "Register",
    value: function Register() {
      //register event one
      this.eventBus.AddEventListener(this.eventOne, this.listener.Listen);
      this.emiter.RegisterEventModule(this.eventBus); //register event two

      this.eventBus.AddEventListener(this.eventTwo, this.listener.Listen);
      this.emiter.RegisterEventModule(this.eventBus);
    }
  }, {
    key: "Emit",
    value: function Emit() {
      this.emiter.Emit(this.eventOne);
      this.emiter.Emit(this.eventTwo);
    }
  }]);

  return MediatorCl;
}();



/***/ }),

/***/ "./js/Libs/AppTwo/appTwo.js":
/*!**********************************!*\
  !*** ./js/Libs/AppTwo/appTwo.js ***!
  \**********************************/
/*! exports provided: AppTwo */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "AppTwo", function() { return AppTwo; });
/* harmony import */ var _MediatorCl_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./MediatorCl.js */ "./js/Libs/AppTwo/MediatorCl.js");
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

//importing service sample bus 


var AppTwo =
/*#__PURE__*/
function () {
  function AppTwo() {
    _classCallCheck(this, AppTwo);

    this.mtr = new _MediatorCl_js__WEBPACK_IMPORTED_MODULE_0__["MediatorCl"]();
  }

  _createClass(AppTwo, [{
    key: "Init",
    value: function Init() {
      console.log('AppTwo Inited');
      this.mtr.Register();
    }
  }, {
    key: "Emit",
    value: function Emit() {
      this.mtr.Emit();
    }
  }]);

  return AppTwo;
}();



/***/ }),

/***/ "./js/Libs/libCl/EventBusCl.js":
/*!*************************************!*\
  !*** ./js/Libs/libCl/EventBusCl.js ***!
  \*************************************/
/*! exports provided: EventModuleCl */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EventModuleCl", function() { return EventModuleCl; });
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

// Example modules
// ---------------
//eventShema={type:{requiered:true},message:{requiered:true}};
//eventmodule registratormodule shema {AddEventListener(),RemoveEventListener(),DispatchEvent()}
///Contains collection of event type to listener registrator
var EventModuleCl =
/*#__PURE__*/
function () {
  function EventModuleCl() {
    _classCallCheck(this, EventModuleCl);

    this.listeners = {};
  }

  _createClass(EventModuleCl, [{
    key: "GetListeners",
    value: function GetListeners() {
      return this.listeners;
    }
  }, {
    key: "AddEventListener",
    value: function AddEventListener(event, callback) {
      if (!event) {
        console.log("No event");
      }

      if (!callback) {
        console.log("No callback");
      }

      if (event && callback) {
        if (!(event.type in this.listeners)) {
          this.listeners[event.type] = [];
        }

        this.listeners[event.type].push(callback);
      }
    }
  }, {
    key: "RemoveEventListener",
    value: function RemoveEventListener(event, callback) {
      var type;

      if (!event || !event.type || !callback) {}

      type = event.type;

      if (!(type in this.listeners)) {
        return;
      }

      var stack = this.listeners[type];

      for (var i = 0, l = stack.length; i < l; i++) {
        if (stack[i] === callback) {
          stack.splice(i, 1);
          return;
        }
      }
    }
  }, {
    key: "de",
    value: function de(event) {
      if (!(event.type in this.listeners)) {
        return true;
      }

      var stack = this.listeners[event.type];

      for (var i = 0, l = stack.length; i < l; i++) {
        stack[i].call(this, event);
      }
    }
  }, {
    key: "DispatchEvent",
    get: function get() {
      return this.de;
    }
  }]);

  return EventModuleCl;
}();

;


/***/ }),

/***/ "./js/Libs/libCl/EventOneCl.js":
/*!*************************************!*\
  !*** ./js/Libs/libCl/EventOneCl.js ***!
  \*************************************/
/*! exports provided: EventOneCl, EventTwoCl */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EventOneCl", function() { return EventOneCl; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "EventTwoCl", function() { return EventTwoCl; });
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EventOneCl = function EventOneCl() {
  _classCallCheck(this, EventOneCl);

  this.Message = "event one";
  this.type = "EventOneCl";
};



var EventTwoCl = function EventTwoCl() {
  _classCallCheck(this, EventTwoCl);

  this.Message = "event two";
  this.type = "EventTwoCl";
  this.param = 0;
};



/***/ })

/******/ });
//# sourceMappingURL=appTwo.bundle.js.map