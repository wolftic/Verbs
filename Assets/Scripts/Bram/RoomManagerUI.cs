using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class RoomManagerUI : MonoBehaviour {

    private InputField inputFieldName;
    private Text currentName;

    private GameObject CreateRoomPanel;
    private GameObject JoinRoomPanel;
    private GameObject ChooseNamePanel;

    void Start()
    {
        inputFieldName = GameObject.Find("EnterName").GetComponent<InputField>();
        currentName = GameObject.Find("CurrentName").GetComponent<Text>();
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

    public void ChooseName()
    {
        if (inputFieldName.text != "")
        {
            currentName.text = "Current name: " + inputFieldName.text;
            //GetComponent<RoomManager>().localPlayerName = inputFieldName.text;
        }
    }
}
