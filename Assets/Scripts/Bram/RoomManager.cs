using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using LitJson;

public class CreatedRoom
{
    public int id;
}

public class RoomManager : MonoBehaviour {

    private Player localPlayer;

    [SerializeField]
    private GameObject room;

    private InputField inputFieldName;
    private Text currentName;

    private SocketIOComponent _socket;
    private InputField inputFieldRoomId;

    private GameObject ChooseTeamPanel;
    private GameObject CreateRoomPanel;
    private GameObject JoinRoomPanel;
    private GameObject ChooseNamePanel;

    [SerializeField]
    private List<Room> rooms = new List<Room>();  

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        inputFieldRoomId = GameObject.Find("RoomId").GetComponent<InputField>();

        inputFieldName = GameObject.Find("EnterName").GetComponent<InputField>();
        currentName = GameObject.Find("CurrentName").GetComponent<Text>();

        ChooseTeamPanel = GameObject.Find("ChooseTeam");
        CreateRoomPanel = GameObject.Find("CreateRoom");
        JoinRoomPanel = GameObject.Find("JoinRoom");
        ChooseNamePanel = GameObject.Find("Name");

        ChooseTeamPanel.SetActive(false);
    }

    public void ChooseName()
    {
        if(inputFieldName.text != "")
        {
            currentName.text = "Current Name: " + localPlayer;
            localPlayer.name = inputFieldName.text;
        }
    }

    public void CreateRoom()
    {
        GameObject createdRoomObject = Instantiate(room) as GameObject;
        Room createdRoom = createdRoomObject.GetComponent<Room>();
        createdRoom.Intialize(Random.Range(0, 100), localPlayer);
        createdRoomObject.transform.name = createdRoom.id.ToString();
        rooms.Add(createdRoom);

        CreatedRoom createdRoomJson = new CreatedRoom();
        createdRoomJson.id = createdRoom.id;

        string jsonRoom = JsonMapper.ToJson(createdRoomJson);
        _socket.Emit("CreateRoom", new JSONObject(jsonRoom));

        CreateRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        ChooseNamePanel.SetActive(false);
        ChooseTeamPanel.SetActive(true);
    }

    public void JoinRoom()
    {
        /*
        for (int i = 0; i < rooms.Count; i++) {
            if (int.Parse(inputFieldRoomId.text) == rooms[i].id)
            {
                currentRoom = rooms[i].id;
                CreateRoomPanel.SetActive(false);
                JoinRoomPanel.SetActive(false);
                ChooseNamePanel.SetActive(false);
                ChooseTeamPanel.SetActive(true);
            }   
        }
        player.id = currentRoom;
        player.name = localPlayerName;
        string jsonRoom = JsonMapper.ToJson(player);
        _socket.Emit("JoinRoom", new JSONObject(jsonRoom));
        localPlayer.transform.SetParent(GameObject.Find("UnAssigned").transform);
        playersInRooms.Add(player);

        for (int i = 0; i < playersInRooms.Count; i++)
        {
            GameObject pl = Instantiate(otherPlayer) as GameObject;
            pl.transform.SetParent(GameObject.Find("UnAssigned").transform);
        }
        */
    }
}
//https://lbv.github.io/litjson/docs/quickstart.html
//litjson doc