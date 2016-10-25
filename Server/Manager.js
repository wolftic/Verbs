var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(8080);

var rooms = [];

io.on('connection', function(socket){
	
	//TODO: als een speler nieuw start add alle rooms die al aktief zijn
	
	console.log("Socket: " + socket.id + " Connected");
	
	socket.on("CreateRoom", function(dataG){
		var room = {
			id: dataG.id,
			players: dataG.playersInRoom,
		}
		
		rooms.push(room);
		
		var dataS = {
			id: room.id,
			players: room.players,
		}
		socket.broadcast.emit("RoomCreated",dataS);
		
		socket.join(dataG.id);
		
		console.log(dataG);
		
	});
	
	socket.on("JoinRoom", function(dataG){
		socket.join(dataG.id);
		
		var dataS = {
			name: dataG.playerName,
			id: dataG.id,
			players: dataG.playersInRoom,
		}
		socket.to(dataG.id).emit("PlayerJoined",dataS);
		
		
		console.log(dataG);
	});
	
	socket.on("ChangeTeam", function(dataG){
		
		var dataS = {
			name: dataG.name,
			team: dataG.team,
		}
		
		io.sockets.in(dataG).emit("PlayerChangedTeam", dataS)
		
		console.log(dataG);
	});
	socket.on('disconnect', function () {
		console.log("Socket: " + socket.id + " Disconnected");
	});
});


