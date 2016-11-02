using UnityEngine;
using System.Collections;

public class TestCam : MonoBehaviour
{

    Transform player;

    public void SetCam(string name)
    {   
        player = GameObject.Find(name).transform;
    }

    void Update()
    { 
        transform.position = new Vector3(player.position.x,player.position.y,player.position.z-10);
    }
}
