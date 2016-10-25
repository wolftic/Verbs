using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using LitJson;

[System.Serializable]
public class P
{
    public string name;
    public string team;
}

[System.Serializable]
public class R
{
    public int id;
    public List<P> players = new List<P>();
}

public class Data
{
    public string playerName;
    public int id;
    public List<P> playersInRoom = new List<P>();
}

public class RoomManager : MonoBehaviour {
    [SerializeField] private GameObject _roomPrefab;

    private SocketIOComponent _socket;
    private RoomManagerUI _UI;

    [SerializeField] private List<R> _rooms = new List<R>();
    [SerializeField] private GameObject roomsObject;

    [SerializeField] private InputField _idField;

    void Start()
    {
        _socket = _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        _UI = GetComponent<RoomManagerUI>();

        _socket.On("RoomCreated", RoomCreated);
    }

    void CreateRoom()
    {
        _UI.ToRoom();

        P player = new P();
        R room = new R();

        player.name = "bram"; //TODO: set player name
        player.team = "u";

        room.id = Random.Range(0, 100);
        room.players.Add(player);

        Data data = new Data();
        data.playerName = player.name;
        data.id = room.id;
        data.playersInRoom = room.players;

        GameObject roomObject = Instantiate(_roomPrefab) as GameObject;
        roomObject.transform.name = room.id.ToString();
        roomObject.transform.SetParent(roomsObject.transform);
        roomObject.GetComponent<Room>().IntializeRoom(room.id, room.players);

        string d = JsonMapper.ToJson(data);
        _socket.Emit("CreateRoom", new JSONObject(d));
    }

    void RoomCreated(SocketIOEvent e)
    {
        R room = JsonMapper.ToObject<R>(e.data.ToString());
        _rooms.Add(room);

        GameObject roomObject = Instantiate(_roomPrefab) as GameObject;
        roomObject.transform.name = room.id.ToString();
        roomObject.transform.SetParent(roomsObject.transform);
        roomObject.GetComponent<Room>().IntializeRoom(room.id, room.players);
        roomObject.SetActive(false);
    }

    void JoinRoom()
    {
        for (int i = 0; i < _rooms.Count; i++)
        {
            if(int.Parse(_idField.text) == _rooms[i].id)
            {
                _UI.ToRoom();

                roomsObject.transform.Find(_idField.text).gameObject.SetActive(true);

                P player = new P();

                player.name = "maarten";
                player.team = "u";

                _rooms[i].players.Add(player);

                Data data = new Data();
                data.playerName = player.name;
                data.id = _rooms[i].id;
                data.playersInRoom = _rooms[i].players;

                string d = JsonMapper.ToJson(data);
                _socket.Emit("JoinRoom", new JSONObject(d));
            }
        }
    }

}
//https://lbv.github.io/litjson/docs/quickstart.html
//litjson doc