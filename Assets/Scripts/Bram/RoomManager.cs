using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using LitJson;

[System.Serializable]
public class nRoom
{
    public int id;
    public List<Player> players = new List<Player>();
}

public class RoomManager : MonoBehaviour {

    public string localPlayerName;

    private SocketIOComponent _socket;

    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private List<nRoom> rooms = new List<nRoom>();
    [SerializeField] private GameObject roomsObject;

    private InputField inputFieldRoomId;
    private RoomManagerUI ui;

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        _socket.On("RoomCreated", NewRoomCreated);

        ui = GetComponent<RoomManagerUI>();
        inputFieldRoomId = GameObject.Find("RoomId").GetComponent<InputField>();
    }

    public void CreateRoom()
    {
        ui.ToRoom();

        nRoom newRoom = new nRoom();
        int createdRoomId = Random.Range(0, 100);
        newRoom.id = createdRoomId;
        
        rooms.Add(newRoom);

        GameObject roomObject = Instantiate(roomPrefab) as GameObject;
        roomObject.GetComponent<Room>().id = newRoom.id;    

        roomObject.transform.name = newRoom.id.ToString();

        Player player = new Player();
        player.name = localPlayerName;
        player.team = "u";

        newRoom.players.Add(player);
        GameObject.Find(createdRoomId.ToString()).GetComponent<Room>().Join(player);

        string jsonRoom = JsonMapper.ToJson(newRoom);
        _socket.Emit("CreateRoom", new JSONObject(jsonRoom));
    }

    public void JoinRoom()
    {
        int id = int.Parse(inputFieldRoomId.text);
        for(int i = 0; i < rooms.Count; i++)
        {
            if(rooms[i].id == id)
            {
                ui.ToRoom();

                roomsObject.transform.Find(rooms[i].id.ToString()).gameObject.SetActive(true);

                Player player = new Player();
                player.name = localPlayerName;
                player.team = "u";

                rooms[i].players.Add(player);

                string jsonRoom = JsonMapper.ToJson(rooms[i]);
                _socket.Emit("JoinRoom", new JSONObject(jsonRoom));

                GameObject.Find(rooms[i].id.ToString()).GetComponent<Room>().Join(player);
            }
        }
    }

    void NewRoomCreated(SocketIOEvent e)
    {
        nRoom newRoom = JsonMapper.ToObject<nRoom>(e.data.ToString());
        Debug.Log(e.data.ToString());
        
        rooms.Add(newRoom);
        GameObject roomObject = Instantiate(roomPrefab) as GameObject;
        roomObject.GetComponent<Room>().id = newRoom.id;

        roomObject.transform.name = newRoom.id.ToString();

        roomObject.transform.SetParent(roomsObject.transform);

        roomObject.SetActive(false);
    }
}
//https://lbv.github.io/litjson/docs/quickstart.html
//litjson doc