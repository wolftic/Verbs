using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using LitJson;

[System.Serializable]
public class Player {
    public string name;
    public string team;
}

public class Room : MonoBehaviour {
    public int id;

    [SerializeField] private GameObject localPlayerObject;
    [SerializeField] private Player localPlayer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] public List<Player> players = new List<Player>();

    private SocketIOComponent _socket;

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        _socket.On("PlayerJoined", OnJoin);
        _socket.On("OnChangeTeam", OnChangeTeam);
    }

    public void Join(Player player)
    {
        players.Add(player);
        
        localPlayer = player;
        GameObject p = Instantiate(playerPrefab) as GameObject;
        localPlayerObject = p;
        p.GetComponentInChildren<Text>().text = player.name;
        p.transform.name = player.name;
        p.transform.SetParent(GameObject.Find("UnAssigned").transform);
    }

    void OnJoin(SocketIOEvent e)
    {
        Player player = JsonMapper.ToObject<Player>(e.data.ToString());
        players.Add(player);

        GameObject p = Instantiate(playerPrefab) as GameObject;
        p.GetComponentInChildren<Text>().text = player.name;
        p.transform.name = player.name;
        p.transform.SetParent(GameObject.Find("UnAssigned").transform);
    }

    void OnChangeTeam(SocketIOEvent e)
    {
        
    }

    public void JoinRobbers()
    {
        localPlayer.team = "r";
        string jsonTeam = JsonMapper.ToJson(localPlayer);

        _socket.Emit("ChangeTeam", new JSONObject(jsonTeam));

        localPlayerObject.transform.SetParent(GameObject.Find("CurrentRobbers").transform);
    }

    public void JoinCops()
    {
        localPlayer.team = "c";
        string jsonTeam = JsonMapper.ToJson(localPlayer);

        _socket.Emit("ChangeTeam", new JSONObject(jsonTeam));

        localPlayerObject.transform.SetParent(GameObject.Find("CurrentCops").transform);
    }
    
}
