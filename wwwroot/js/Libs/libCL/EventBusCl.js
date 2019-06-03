
// Example modules
// ---------------

//eventShema={type:{requiered:true},message:{requiered:true}};
//eventmodule registratormodule shema {AddEventListener(),RemoveEventListener(),DispatchEvent()}

///Contains collection of event type to listener registrator
class EventModuleCl {

    constructor()
    {
        this.listeners={};
    }
    GetListeners(){
        return this.listeners;
    }
    AddEventListener(event,callback){

        if(!event){  console.log("No event");}
        if(!callback){  console.log("No callback");} 

        if(event && callback){
            if(!(event.type in this.listeners)){
                this.listeners[event.type]=[];
            }
            this.listeners[event.type].push(callback);
        }   
    }
    RemoveEventListener(event,callback){
        var type;
        if(!event || !event.type || !callback){
            
        }
        type=event.type;

        if(!(type in this.listeners)){                     
            return;
        }
        var stack = this.listeners[type];
        for(var i =0,l=stack.length; i<l;i++){
            if(stack[i]===callback){
                stack.splice(i,1);
                return;
            }
        }       
    } 
    
    de(event){
        if(!(event.type in this.listeners)){
            return true;
        }
        var stack=this.listeners[event.type];
        for(var i =0,l=stack.length;i<l;i++){            
            stack[i].call(this,event);
        }    
    }
    get DispatchEvent(){
        return this.de;
    }
    
};

export {EventModuleCl};
