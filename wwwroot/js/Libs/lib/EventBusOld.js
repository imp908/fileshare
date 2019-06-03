
//Event module check
//--------------------------------------
var EventsTestModule =(function(_module){

    var eventWithModulesCheck = function ()
    {
        console.log("eventWithModulesCheck");    
        var _eventModule = EventModule;
        var _listenerModule = ListenerModule;
        var _emitModule = EmitModule;
    
        var event01 = {type:'ev0',message:"first event"};
        _eventModule.AddEventListener(event01,_listenerModule.Receive);
        _emitModule.Emit(event01,_eventModule.DispatchEvent);
    
        var event02 = {type:'ev1',message:"new event"};
        _eventModule.AddEventListener(event02,_listenerModule.Receive);
        _emitModule.Emit(event02,_eventModule.DispatchEvent);
    
        var _emitModule2 = AnotherEmitModule;
        _emitModule2.OtherEmit(event01,_eventModule.DispatchEvent);
    
        console.log("All to all listen");
        var lsitenerModule2 = AnotherListenerModule;
        _eventModule.AddEventListener(event01,lsitenerModule2.AnotherReceive);
        _eventModule.AddEventListener(event02,lsitenerModule2.AnotherReceive);
    
        _emitModule.Emit(event01,_eventModule.DispatchEvent);
        _emitModule.Emit(event02,_eventModule.DispatchEvent);
    
        _emitModule2.OtherEmit(event01,_eventModule.DispatchEvent);
        _emitModule2.OtherEmit(event02,_eventModule.DispatchEvent);
    }

    //!TODO:=> move to mediator module
    var preregisteredEmiterCheck = function(){
        
        var event1 = {type:"event3",message:"event3 message"}
        var event2 = {type:"event4",message:"event4 message"}

        var _eventEmiterListenerRegistrator = EventModule;
        var _listener = ListenerModule;
        var _preregisteredEmiter = EmitModulePreregistered;


        //register event->listener.func
        _eventEmiterListenerRegistrator.AddEventListener(event1,_listener.Receive);
        //register eventregistrator->eventEmiter.dispatch
        _preregisteredEmiter.RegisterEventModule(_eventEmiterListenerRegistrator.DispatchEvent);

        //fire registered event to first listener
        console.log("fire registered event to first listener");
        _preregisteredEmiter.Emit(event1);



        var _listenerAnother = AnotherListenerModule;
        //register another listener for ev1
        _eventEmiterListenerRegistrator.AddEventListener(event1,_listenerAnother.AnotherReceive);  

        //fire registered event to 2 listeners
        console.log("fire registered event to 2 listeners");
        _preregisteredEmiter.Emit(event1);



        //clear up
        _eventEmiterListenerRegistrator.RemoveEventListener(event1,_listener.Receive);
        _eventEmiterListenerRegistrator.RemoveEventListener(event1,_listenerAnother.AnotherReceive);
        //no any events fire
        _preregisteredEmiter.Emit(event1);


    };

    var multipleRegistratorsOneEmiterCheck = function(){

        var event1 = {type:"event3",message:"event3 message"}
        var event2 = {type:"event4",message:"event4 message"}

        var _eventEmiterListenerRegistrator = EventModule;
        var _listener = ListenerModule;

        var _eventEmiterListenerRegistrator2 = EventModule;
        var _listener2 = AnotherListenerModule;
        
        var _preregisteredEmiter = EmitModulePreregistered;
        
        //registration event 2 listener 1
        _eventEmiterListenerRegistrator.AddEventListener(event2,_listener.Receive);
        _preregisteredEmiter.RegisterEventModule(_eventEmiterListenerRegistrator.DispatchEvent);
        
        //registration event 2 listener 2
        _eventEmiterListenerRegistrator2.AddEventListener(event2,_listener2.AnotherReceive)
        _preregisteredEmiter.RegisterEventModule(_eventEmiterListenerRegistrator2.DispatchEvent);
        
        //emit from emit moule
        _preregisteredEmiter.Emit(event2);

    }
    
    _module.Init = multipleRegistratorsOneEmiterCheck;

    return _module;
})(EventsTestModule || {});





// Example modules
// ---------------

//eventShema={type:{requiered:true},message:{requiered:true}};
//eventmodule registratormodule shema {AddEventListener(),RemoveEventListener(),DispatchEvent()}

///Contains collection of event type to listener registrator
var EventModule = (function(_module){

    _module.listeners={};

    _module.GetListeners=function(){
        return em.listeners;
    }
    _module.AddEventListener=function(event,callback){
        if(!(event.type in _module.listeners)){
            _module.listeners[event.type]=[];
        }
        _module.listeners[event.type].push(callback);
    }
    _module.RemoveEventListener=function(event,callback){
        var type;
        if(!event || !event.type || !callback){
            
        }
        type=event.type;

        if(!(type in _module.listeners)){                     
            return;
        }
        var stack = _module.listeners[type];
        for(var i =0,l=stack.length; i<l;i++){
            if(stack[i]===callback){
                stack.splice(i,1);
                return;
            }
        }       
    }
    _module.DispatchEvent=function(event){
        if(!(event.type in _module.listeners)){
            return true;
        }
        var stack=_module.listeners[event.type];
        for(var i =0,l=stack.length;i<l;i++){            
            stack[i].call(this,event);
        }
    
    }

    return _module;

})(EventModule || {});

///Template for listen module
var ListenerModule = (function(_module){
    
    _module.Receive=function(e){
        console.log("ListenerModule Received");
        console.log(e);
    }

    return _module;

})(ListenerModule || {});

///template for emiter with registered event module
var EmitModulePreregistered = (function(_module){
    
    var registerEventModuleCallback;

    _module.RegisterEventModule=function(callback){
        registerEventModuleCallback=callback;
    }

    _module.Emit=function(e){
        if(registerEventModuleCallback){
            registerEventModuleCallback(e);
        }
    }

    return _module;

})(EmitModulePreregistered || {})



var EventRegistratorModule = (function(_module){
    
    var eventsRegistrator;

    //{event:{type,message},ListenerModule.AnyFunction,EmitModule.FunctionWithCllback}
    _module.RegisterShema={event:{required:true},listenerF:{required:true},emiterF:{required:true}};
    _module.Register=function(param){
        //event to listener parameter register
        eventsRegistrator.AddEventListener(param.event,param.listenerF);
        //register registrator event to emiter
        param.emiterF.call(eventsRegistrator.DispatchEvent);
    }
    _module.Init=function(){

    }

})(EventRegistratorModule || {});





//Another emiter and listener
var AnotherListenerModule = (function(_module){
    
    _module.AnotherReceive=function(e){
        console.log("AnotherListenerModule AnotherReceive");
        console.log(e);
    }

    return _module;

})(AnotherListenerModule || {});

///Template for emit module
var EmitModule = (function(_module){

    _module.Emit = function(e,callback){
        console.log("EmitModule Emit")
        callback(e);        
    }

    return _module;

})(EmitModule || {})
var AnotherEmitModule = (function(_module){

    _module.OtherEmit = function(e,callback){
        console.log("AnotherEmitModule OtherEmit")
        callback(e);
    }

    return _module;
    
})(AnotherEmitModule || {})
//?event emiters with module callback registration
var EmitModulePreregistered2 = (function(_module){
    
    var registerEventModuleCallback;

    _module.RegisterEventModule=function(callback){
        registerEventModuleCallback=callback;
    }

    _module.Emit=function(e){
        if(registerEventModuleCallback){
            registerEventModuleCallback(e);
        }
    }

    return _module;

})(EmitModulePreregistered2 || {})
