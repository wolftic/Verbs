var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(8080);

io.on('connection', function(socket){
	
});

