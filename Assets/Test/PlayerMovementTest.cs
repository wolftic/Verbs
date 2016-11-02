using UnityEngine;
using System.Collections;
using LitJson;
using SocketIO;

public class PlayerMovementTest : MonoBehaviour {

    [SerializeField] private bool _multiPlayer;
    [SerializeField] private SocketIOComponent _socket;

    [SerializeField] private Rigidbody2D _rigidbody2d;

    [SerializeField] private float _speed = 5;
    [SerializeField] private bool _facingRight;

    void Start()
    {
        _multiPlayer = true;
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        _rigidbody2d.velocity = new Vector2(x * _speed, y * _speed);

        if (_multiPlayer)
        {
            PlayerData playerPosition = new PlayerData();
            playerPosition.name = "bram";
            playerPosition.x = transform.position.x;
            playerPosition.y = transform.position.y;
            playerPosition.z = transform.position.z;

            string jsonPos = JsonMapper.ToJson(playerPosition);

            _socket.Emit("SendPlayerData", new JSONObject(jsonPos));

            Debug.Log(playerPosition.name);
        }
    }

    private void FlipPlayer()
    {
        _facingRight = !_facingRight;

        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
