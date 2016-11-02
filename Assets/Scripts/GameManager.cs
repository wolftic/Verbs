using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using LitJson;

public class Pos
{
    public string name;
    public double x;
    public double y;
    public double z;
}

public class GameManager : MonoBehaviour {
    [SerializeField]
    private SocketIOComponent _socket;
    [SerializeField]
    public List<PlayerData> _playersInGame = new List<PlayerData>();

    [SerializeField]
    public PlayerData pd;

    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _otherPlayerPrefab;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //_socket.On("OtherStarted", OtherPlayerStarted);
        //_socket.On("UpdateOtherPlayer", UpdateOtherPlayer);
    }

    void Update()
    {
        if(_socket == null)
        {
            _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject playerObject = Instantiate(_playerPrefab) as GameObject;
            playerObject.transform.name = _playersInGame[0].name;
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < _playersInGame.Count; i++)
        {
            if(_playersInGame[i].name == pd.name)
            {
                GameObject playerObject = Instantiate(_playerPrefab) as GameObject;
                playerObject.transform.name = _playersInGame[i].name;
            } else
            {
                GameObject playerObject = Instantiate(_otherPlayerPrefab) as GameObject;
                playerObject.transform.name = _playersInGame[i].name;
            }
        }

        //string jsonPlayer = JsonMapper.ToJson(player);
        //_socket.Emit("PlayerStarted", new JSONObject(jsonPlayer));

        //GameObject.Find("Main Camera").GetComponent<TestCam>().SetCam(pd.name);
    }

    void OtherPlayerStarted(SocketIOEvent e)
    {
        Debug.Log(e.data.ToString());
        PlayerData player = JsonMapper.ToObject<PlayerData>(e.data.ToString());
        player.x = transform.position.x;
        player.y = transform.position.y;
        player.z = transform.position.z;
        _playersInGame.Add(player);
        GameObject playerObject = Instantiate(_otherPlayerPrefab) as GameObject;
        playerObject.transform.name = player.name;
    }

    void UpdateOtherPlayer(SocketIOEvent e)
    {
        PlayerData pos = JsonMapper.ToObject<PlayerData>(e.data.ToString());
        GameObject.Find(pos.name).GetComponent<OtherPlayerMovementTest>().Move(new Vector3((float)pos.x, (float)pos.y, (float)pos.z));
    }
}
