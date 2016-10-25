using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class RoomManagerUI : MonoBehaviour {
    private GameObject CreateRoomPanel;
    private GameObject JoinRoomPanel;
    private GameObject ChooseNamePanel;

    void Start()
    {
        CreateRoomPanel = GameObject.Find("CreateRoom");
        JoinRoomPanel = GameObject.Find("JoinRoom");
        ChooseNamePanel = GameObject.Find("Name");
    }

    public void ToRoom()
    {
        CreateRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        ChooseNamePanel.SetActive(false);
    }

    public void ToMain()
    {
        CreateRoomPanel.SetActive(true);
        JoinRoomPanel.SetActive(true);
        ChooseNamePanel.SetActive(true);
    }

}
