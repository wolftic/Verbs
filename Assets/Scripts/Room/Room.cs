using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    [SerializeField] int currentRobbersLength = 0;
    [SerializeField] int currentCopsLength = 0;

    [SerializeField] private GameObject robbersListObject;
    [SerializeField] private GameObject copsListObject;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _otherPlayer;

    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _GameManager;

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        _socket.On("PlayerJoined", PlayerJoined);
        _socket.On("PlayerChangedTeam", PlayerChangedTeam);
        _socket.On("PlayerStopped", PlayerStopped);
        _socket.On("OtherStarted", OtherStarted);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="players"></param>
    /// <param name="localP"></param>
    public void IntializeRoom(int id, List<P> players, P localP)
    {
        _id = id;
        _players = players;

        localPlayer = localP;

        for(int i = 0; i < players.Count; i++)
        {
            GameObject playerObject = Instantiate(_playerPrefab) as GameObject;
            playerObject.transform.name = _players[i].name;
            playerObject.GetComponentInChildren<Text>().text = _players[i].name;
            playerObject.transform.SetParent(GameObject.Find("u").transform);
        }

        GameObject.Find("roomId").GetComponent<Text>().text = "ROOM ID: " + _id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="player"></param>
    public void Join(P player)
    {
        localPlayer = player;

        GameObject playerObject = Instantiate(_playerPrefab) as GameObject;
        playerObject.transform.name = player.name;
        playerObject.GetComponentInChildren<Text>().text = player.name;
        playerObject.transform.SetParent(GameObject.Find("u").transform);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    void PlayerJoined(SocketIOEvent e)
    {
        Data data = JsonMapper.ToObject<Data>(e.data.ToString()); //adnkfalk
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

    /// <summary>
    /// 
    /// </summary>
    public void ChangeTeamCops()
    {
        if (currentCopsLength < 2)
        {
            GameObject playerObject = GameObject.Find(localPlayer.name.ToString());
            playerObject.transform.SetParent(GameObject.Find("c").transform);
            localPlayer.team = "c";

            P player = new P();
            player.name = localPlayer.name + "," + _id;
            player.team = "c";
            string d = JsonMapper.ToJson(player);
            _socket.Emit("ChangeTeam", new JSONObject(d));
        }
        CheckLimit();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangeTeamRobbers()
    {
        if (currentRobbersLength < 2)
        {
            GameObject playerObject = GameObject.Find(localPlayer.name.ToString());
            playerObject.transform.SetParent(GameObject.Find("r").transform);
            localPlayer.team = "r";

            P player = new P();
            player.name = localPlayer.name + "," + _id;
            player.team = "r";
            string d = JsonMapper.ToJson(player);
            _socket.Emit("ChangeTeam", new JSONObject(d));
        }
        CheckLimit();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    void PlayerChangedTeam(SocketIOEvent e)
    {
        P player = JsonMapper.ToObject<P>(e.data.ToString());

        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].name == player.name)
            {
                _players[i].team = player.team;
                GameObject.Find(player.name).transform.SetParent(GameObject.Find(player.team).transform);
                CheckLimit();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void CheckLimit()
    {
        currentCopsLength = 0;
        currentRobbersLength = 0;
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].team == "c")
            {
                currentCopsLength++;
                GameObject.Find("currentC").GetComponent<Text>().text = currentCopsLength + "/2";
                GameObject.Find("currentR").GetComponent<Text>().text = currentRobbersLength + "/2";
            }
            if (_players[i].team == "r")
            {
                currentRobbersLength++;
                GameObject.Find("currentR").GetComponent<Text>().text = currentRobbersLength + "/2";
                GameObject.Find("currentC").GetComponent<Text>().text = currentCopsLength + "/2";
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void StartGame(bool l)
    {
        GameObject gm = Instantiate(_GameManager) as GameObject;
        for (int i = 0; i < _players.Count; i++)
        {
            PlayerData player = new PlayerData();
            player.name = _players[i].name;
            gm.GetComponent<GameManager>()._playersInGame.Add(player);
        }
        PlayerData p = new PlayerData();
        p.name = localPlayer.name;
        gm.GetComponent<GameManager>().pd = p;

        SceneManager.LoadScene("Lorenzo 1");
        gm.GetComponent<GameManager>().StartGame();
        if (l)
        {
            _socket.Emit("StartGame");
        }
    }

    void OtherStarted(SocketIOEvent e)
    {
        StartGame(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    void PlayerStopped(SocketIOEvent e)
    {
        Data data = JsonMapper.ToObject<Data>(e.data.ToString());
        print(data.playerName);
        for (int i = 0; i < _players.Count; i++)
        {
            if(_players[i].name == data.playerName)
            {
                _players.RemoveAt(i);
                Destroy(GameObject.Find(data.playerName));
                CheckLimit();
            }
        }
    }

}
