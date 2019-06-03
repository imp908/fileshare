
//Sample listener module
var Lstn = (function(module){
        
    module.Init = function(){
        console.log('ListenerNew inited')
    }

    module.Listen = function(){
        console.log('ListenTwo')
    }

    return module;
})(Lstn || {})
export function ListenerNewTwo (){
    return Lstn;
}
