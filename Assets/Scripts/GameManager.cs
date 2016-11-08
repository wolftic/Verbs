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
    private GameObject _copsPrefab;
    [SerializeField]
    private GameObject _robberPrefab;

    [SerializeField]
    private GameObject _otherCopsPrefab;
    [SerializeField]
    private GameObject _otherRobberPrefab;

    public float gameStartTime;
    bool gameStarted;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //_socket.On("OtherStarted", OtherPlayerStarted);
    }

    void Update()
    {
        if(_socket == null)
        {
            _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
            _socket.On("OtherMove", UpdateOtherPlayer);
        }

        if(Time.time <= gameStartTime + 3 && !gameStarted)
        {
            StartGame();
            gameStarted = true;
        }
    }

    void StartGame()
    {
        for (int i = 0; i < _playersInGame.Count; i++)
        {
            if(_playersInGame[i].name == pd.name)
            {
                if (pd.team == "r")
                {
                    GameObject playerObject = Instantiate(_robberPrefab) as GameObject;
                    playerObject.transform.name = _playersInGame[i].name;
                    playerObject.GetComponent<PlayerMovement>().name = pd.name;
                    Camera.main.gameObject.GetComponent<TestCam>().SetCam(_playersInGame[i].name);
                } else if (pd.team == "c")
                {
                    GameObject playerObject = Instantiate(_copsPrefab) as GameObject;
                    playerObject.transform.name = _playersInGame[i].name;
                    playerObject.GetComponent<PlayerMovement>().name = pd.name;
                    Camera.main.gameObject.GetComponent<TestCam>().SetCam(_playersInGame[i].name);
                }
                
            } else
            {
                if (_playersInGame[i].team == "r")
                {
                    GameObject playerObject = Instantiate(_otherRobberPrefab) as GameObject;
                    playerObject.transform.name = _playersInGame[i].name;
                } else if(_playersInGame[i].team == "c")
                {
                    GameObject playerObject = Instantiate(_otherCopsPrefab) as GameObject;
                    playerObject.transform.name = _playersInGame[i].name;
                }
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
        if (player.team == "r")
        {
            GameObject playerObject = Instantiate(_otherRobberPrefab) as GameObject;
            playerObject.transform.name = player.name;
        }
        else if (player.team == "c")
        {
            GameObject playerObject = Instantiate(_otherCopsPrefab) as GameObject;
            playerObject.transform.name = player.name;
        }
    }

    void UpdateOtherPlayer(SocketIOEvent e)
    {
        PlayerData pos = JsonMapper.ToObject<PlayerData>(e.data.ToString());
        Debug.Log(pos.name + pos.x + pos.y + pos.z);
        GameObject.Find(pos.name).GetComponent<OtherPlayerMovementTest>().Move(new Vector3((float)pos.x, (float)pos.y, (float)pos.z));
    }
}
