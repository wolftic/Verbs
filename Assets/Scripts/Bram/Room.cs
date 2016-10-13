using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
    public string name;
    public bool team;
}

public class Room : MonoBehaviour {
    public int id;
    private List<Player> players = new List<Player>();

    public void Intialize(int id, Player pl)
    {
        this.id = id;
        players.Add(pl);
    }

    /*
    void PlayerJoined(SocketIOEvent e)
    {
        Player otherPlayerP = JsonMapper.ToObject<Player>(e.data.ToString());
        //Debug.Log(otherPlayerP);
        //playersInRooms.Add(otherPlayerP);
        GameObject other = Instantiate(otherPlayer) as GameObject;
        other.transform.SetParent(GameObject.Find("UnAssigned").transform);
    }

    public void JoinRobbers()
    {
        localPlayer.transform.SetParent(GameObject.Find("CurrentRobbers").transform);
        player.team = "Robbers";
        string jsonPlayer = JsonMapper.ToJson(player);
        _socket.Emit("JoinRobbers", new JSONObject(jsonPlayer));
    }

    public void JoinCops()
    {
        localPlayer.transform.SetParent(GameObject.Find("CurrentCops").transform);
        player.team = "Cops";
        string jsonPlayer = JsonMapper.ToJson(player);
        _socket.Emit("JoinCops", new JSONObject(jsonPlayer));
    }
    */
}
