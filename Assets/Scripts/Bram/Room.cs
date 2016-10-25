using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SocketIO;
using LitJson;

public class Room : MonoBehaviour {

    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private int _id;
    [SerializeField] private List<P> _players = new List<P>();

    private SocketIOComponent _socket;

    [SerializeField] private P localPlayer;

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        _socket.On("PlayerJoined", PlayerJoined);
        _socket.On("PlayerChangedTeam", PlayerChangedTeam);
    }

    public void IntializeRoom(int id, List<P> players, P localP)
    {
        _id = id;
        _players = players;

        localPlayer = localP;

        GameObject playerObject = Instantiate(_playerPrefab) as GameObject;
        playerObject.transform.name = _players[0].name;
        playerObject.GetComponentInChildren<Text>().text = _players[0].name;
        playerObject.transform.SetParent(GameObject.Find("u").transform);
    }

    public void Join(P player)
    {
        localPlayer = player;

        GameObject playerObject = Instantiate(_playerPrefab) as GameObject;
        playerObject.transform.name = player.name;
        playerObject.GetComponentInChildren<Text>().text = player.name;
        playerObject.transform.SetParent(GameObject.Find("u").transform);
    }

    void PlayerJoined(SocketIOEvent e)
    {
        Data data = JsonMapper.ToObject<Data>(e.data.ToString());
        P player = new P();

        for (int i = 0; i < data.playersInRoom.Count; i++)
        {
            if(data.playersInRoom[i].name == data.playerName)
            {
                player = data.playersInRoom[i];
            }
        }

        _players.Add(player);

        GameObject playerObject = Instantiate(_playerPrefab) as GameObject;
        playerObject.transform.name = player.name;
        playerObject.GetComponentInChildren<Text>().text = player.name;
        playerObject.transform.SetParent(GameObject.Find("u").transform);
    }

    public void ChangeTeam(string team)
    {
        GameObject playerObject = GameObject.Find(localPlayer.name.ToString());
        playerObject.transform.SetParent(GameObject.Find(team.ToString()).transform);
        localPlayer.team = team;

        P player = new P();
        player.name = localPlayer.name + "," + _id;
        player.team = team;
        string d = JsonMapper.ToJson(player);
        _socket.Emit("ChangeTeam", new JSONObject(d));
    }

    void PlayerChangedTeam(SocketIOEvent e)
    {
        P player = JsonMapper.ToObject<P>(e.data.ToString());

        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].name == player.name)
            {
                _players[i].team = player.team;
                GameObject.Find(player.name).transform.SetParent(GameObject.Find(player.team).transform);
            }
        }
    }
}
