var io = require('socket.io')({
	transports: ['websocket'],
});

io.attach(8080);

var rooms = [];

io.on('connection', function(socket){
	
	var currentClient;
		
		
	//TODO: als een speler nieuw start add alle rooms die al aktief zijn
	
	console.log("Socket: " + socket.id + " Connected");
	
	socket.on("CreateRoom", function(dataG){
		
		var room = {
			id: dataG.id,
			players: dataG.playersInRoom,
		}
		
		currentClient = {
			id: socket.id,
			name: dataG.playerName,
		}
		console.log(currentClient);
	
		
		//console.log(room.id);
		
		rooms.push(room);
		
		var dataS = {
			id: room.id,
			players: room.players,
		}
		socket.broadcast.emit("RoomCreated",dataS);
		
		socket.join(dataG.id);
		
	});
	
	socket.on("JoinRoom", function(dataG){
		socket.join(dataG.id);
		
		currentClient = {
			id: socket.id,
			name: dataG.playerName,
		}
		
		console.log(currentClient);
		
		for(var i = 0; i < rooms.length; i++){
			if(dataG.id == rooms[i].id){
				rooms[i].players = dataG.playersInRoom;
				//console.log(rooms[i].players);
			}
		}
		
		var dataS = {
			playerName: dataG.playerName,
			id: dataG.id,
			playersInRoom: dataG.playersInRoom,
		}
		socket.broadcast.emit("PlayerJoined",dataS);
		
	});
	
	socket.on("ChangeTeam", function(dataG){
		//console.log(dataG.name);
		var res = dataG.name.split(",");
		
		var dataS = {
			name: res[0],
			team: dataG.team,
		}
		
		socket.broadcast.to(res[1]).emit("PlayerChangedTeam", dataS)
		
	});
	socket.on('disconnect', function () {
		console.log("Socket: " + socket.id + " Disconnected");
		
		var dataS = {
			playerName: currentClient.name,
		}
		console.log(dataS);
		
		socket.broadcast.emit("PlayerStopped", dataS);
	});
	
	socket.on("StartGame", function(){
		socket.broadcast.emit("OtherStarted");
	});
	
	//INGAME
	socket.on("Move",function(dataG){
		
		var dataS = {
			name: dataG.name,
			x: dataG.x,
			y: dataG.y,
			z: dataG.z,
		}
		//TODO: alleen goede room
		socket.emit("OtherMove", dataS);
	});
});


