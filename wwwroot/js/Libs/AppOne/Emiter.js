///Sample emmitting event module
var En = (function(module){
    
    var registerEventModuleCallback;

    module.Init = function(){
        console.log('EmiterNew inited')
    }
    
    module.RegisterEventModule=function(callback){
        console.log(callback);
        registerEventModuleCallback=callback;
    }

    module.Emit = function(e){
        //console.log('Emit');
        if(registerEventModuleCallback){
            registerEventModuleCallback(e);
        }
    }

    return module;
})(En || {})
export function EmiterNew (){
    return En;
}