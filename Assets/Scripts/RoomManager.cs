using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using LitJson;

public class Room
{
    public int id;
}

public class RoomManager : MonoBehaviour {

    private SocketIOComponent _socket;
    private InputField _inputFieldRoomId;

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        _inputFieldRoomId = GameObject.Find("RoomId").GetComponent<InputField>();
    }

    public void CreateRoom()
    {
        Room room = new Room();
        room.id = Random.Range(0, 100);
        string jsonRoom = JsonMapper.ToJson(room);
        _socket.Emit("CreateRoom", new JSONObject(jsonRoom));
    }

    public void JoinRoom()
    {

    }
}
