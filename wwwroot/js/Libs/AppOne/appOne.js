//

//importing service sample bus
import {EventModule} from '../lib/EventBus.js'

import {ListenerNew} from './Listener.js'
import {EmiterNew} from './Emiter.js'

import {ListenerNewTwo} from './ListenerTwo.js'

var eventOne = {type:"eventOne",message:"eventOne message"}
var eventTwo = {type:"eventTwo",message:"eventTwo message"}

///Test module to check initialization in MVC
var Smo =(function(module){
        
    module.Init = function(){
        console.log('SomeModuleOne inited')
    }

    return module;
})(Smo || {})
export function SomeModuleOne (){
    return Smo;
}


var Mdtr = (function(module){

    //imported service bus for registration:
    //listener-[event]->emiter
    var eventModule = EventModule;

    var listenerNew = ListenerNew;
    var emiterNew = EmiterNew;

    var listenerNewTwo = ListenerNewTwo;

    module.New = function(){
        return new module;
    }

    module.Init = function(){
        console.log('Mediator inited')
    }

    module.Register = function(){
        console.log('Mediator Register inited');
        console.log(eventOne);

        //register eventone line (all from mvc button click)
        eventModule().AddEventListener(eventOne,listenerNew().Listen);
        emiterNew().RegisterEventModule(eventModule().DispatchEvent);

        eventModule().AddEventListener(eventTwo,listenerNewTwo().Listen);
        emiterNew().RegisterEventModule(eventModule().DispatchEvent);
    }

    module.EmitFromMediator = function()
    {
        emiterNew().Emit(eventOne);
        
        module.EmitFromMediatorTwo();
    }

    module.EmitFromMediatorTwo = function()
    {
        emiterNew().Emit(eventTwo);
    }

    return module;
})(Mdtr || {})
export function Mediator (){
    return Mdtr;
}
