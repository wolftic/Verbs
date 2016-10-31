using UnityEngine;
using System.Collections;
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
    private SocketIOComponent _socket;

    void Start()
    {
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();

        _socket.On("OtherMove", Move);
    }

    void Move(SocketIOEvent e)
    {
        Pos pos = JsonMapper.ToObject<Pos>(e.data.ToString());
        Vector3 v3 = new Vector3((float)pos.x, (float)pos.y, (float)pos.z);
        GameObject.Find(pos.name).GetComponent<OtherPlayerMovement>().GoTo(v3);
    }
}
