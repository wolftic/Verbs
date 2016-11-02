using UnityEngine;
using System.Collections;

public class OtherPlayerMovementTest : MonoBehaviour {

    [SerializeField] private Vector3 currentPos;

    public void Move(Vector3 pos)
    {
        currentPos = pos;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentPos, 3 * Time.deltaTime);
    }
}
