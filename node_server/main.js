var events = require('events');

var eventEmitter = new events.EventEmitter();

var connectHandler = function connected(){
	console.log('connected' );
	
	eventEmitter.emit('data_received');
};


eventEmitter.on('connection',connectHandler);

eventEmitter.on('data_received',
function(){
	console.log('data_received succesfully');
});

eventEmitter.emit('connection');

console.log('Programm ended');
