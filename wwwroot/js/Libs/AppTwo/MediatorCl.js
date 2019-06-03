import {EventModuleCl} from '../libCl/EventBusCl.js'
import {EventOneCl} from '../libCl/EventOneCl.js'
import {EventTwoCl} from '../libCl/EventOneCl.js'

import {EmiterCl} from './EmiterCl.js'
import {ListenerCl} from './ListenerCl.js'

class MediatorCl{
    
    constructor(){
        this.eventBus = new EventModuleCl();
        this.emiter = new EmiterCl();
        this.listener = new ListenerCl();

        this.eventOne = new EventOneCl();
        this.eventTwo = new EventTwoCl();
    }

    Register(){
        //register event one
        this.eventBus.AddEventListener(this.eventOne,this.listener.Listen);
        this.emiter.RegisterEventModule(this.eventBus);

        //register event two
        this.eventBus.AddEventListener(this.eventTwo,this.listener.Listen);
        this.emiter.RegisterEventModule(this.eventBus);
    }

    Emit(){
        this.emiter.Emit(this.eventOne);
        this.emiter.Emit(this.eventTwo);
    }
 
}

export {MediatorCl}