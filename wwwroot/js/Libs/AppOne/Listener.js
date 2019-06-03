
//Sample listener module
var Lstn = (function(module){
        
    module.Init = function(){
        console.log('ListenerNew inited')
    }

    module.Listen = function(){
        console.log('ListenOne')
    }

    return module;
})(Lstn || {})
export function ListenerNew (){
    return Lstn;
}
