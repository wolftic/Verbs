var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(8080);

var rooms = [];

io.on('connection', function(socket){
	
	//TODO: als een speler nieuw start add alle rooms die al aktief zijn
	
	console.log(socket.id);
	
	socket.on("CreateRoom", function(dataG){
		rooms.push(dataG);
		
		var dataS = {
			id: dataG.id,
			players: dataG.players,
		}
		socket.broadcast.emit("RoomCreated",dataS);
		
		socket.join(dataG);
		
		console.log("Socket: " + socket.id + " Created room: ");
		console.log(dataG);
	});
	
	socket.on("JoinRoom", function(dataG){
		socket.join(dataG);
		
		var dataS = {
			name: dataG.name,
		}
		
		io.sockets.in(dataG).emit("PlayerJoined", dataS);
		
		console.log("Socket: " + socket.id + " Joined room: ");
		console.log(dataG);
	});
	
	socket.on("ChangeTeam", function(dataG){
		
		var dataS = {
			name: dataG.name,
			team: dataG.team,
		}
		
		io.sockets.in(dataG).emit("OnChangeTeam", dataS)
	});
});


