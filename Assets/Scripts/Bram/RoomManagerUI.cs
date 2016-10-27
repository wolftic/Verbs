using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class RoomManagerUI : MonoBehaviour {

    [SerializeField] GameObject[] thingsToTurnOffToRoom;

    public void ToRoom()
    {
        for (int i = 0; i < thingsToTurnOffToRoom.Length; i++)
        {
            thingsToTurnOffToRoom[i].SetActive(false);
        }
    }
}
