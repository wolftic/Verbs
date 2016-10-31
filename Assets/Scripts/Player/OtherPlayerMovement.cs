using UnityEngine;
using System.Collections;

public class OtherPlayerMovement : MonoBehaviour {
    public string name;
    private float _speed = 1;

    public void GoTo(Vector3 v3)
    {
        transform.position = Vector3.Lerp(transform.position, v3, _speed * Time.deltaTime);
    }
}
