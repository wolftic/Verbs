var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(8080);

var rooms = [];

io.on('connection', function(socket){	
	socket.on("CreateRoom", function(data){
		rooms.push(data);
		console.log(rooms);
	});
	
	socket.on("JoinRoom", function(data){

	});
});


