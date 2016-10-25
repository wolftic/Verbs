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
		
		console.log(room.id);
		
		rooms.push(room);
		
		var dataS = {
			id: room.id,
			players: room.players,
		}
		socket.broadcast.emit("RoomCreated",dataS);
		
		socket.join(dataG.id);
		
		//console.log(dataG);
		
	});
	
	socket.on("JoinRoom", function(dataG){
		socket.join(dataG.id);
		
		for(var i = 0; i < rooms.length; i++){
			if(dataG.id == rooms[i].id){
				rooms[i].players = dataG.playersInRoom;
				console.log(rooms[i].players);
			}
		}
		
		var dataS = {
			playerName: dataG.playerName,
			id: dataG.id,
			playersInRoom: dataG.playersInRoom,
		}
		socket.to(dataG.id).emit("PlayerJoined",dataS);
		
		//console.log(dataG);
	});
	
	socket.on("ChangeTeam", function(dataG){
		console.log(dataG.name);
		var res = dataG.name.split(",");
		
		var dataS = {
			name: res[0],
			team: dataG.team,
		}
		
		io.sockets.in(res[1]).emit("PlayerChangedTeam", dataS)
		
		console.log(res[1]);
	});
	socket.on('disconnect', function () {
		console.log("Socket: " + socket.id + " Disconnected");
	});
});


