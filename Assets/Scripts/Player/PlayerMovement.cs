using UnityEngine;
using System.Collections;
using SocketIO;
using LitJson;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    private SocketIOComponent _socket;

	[SerializeField]
	private float _speed = 2f;
	[SerializeField]
	private float _runSpeed = 4f;
    public string name;

    private float _curSpeed {
		set {
			_curSpeed = Mathf.Clamp (value, _speed, _runSpeed);
		}
		get {
			return _curSpeed;
		}
	}

	private Rigidbody2D rigidbody2d;

    Pos pos = new Pos();

    void Start () {
		rigidbody2d = GetComponent<Rigidbody2D> ();
        pos.name = name;
        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }
	
	void Update () {
		Vector2 movement = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		rigidbody2d.velocity = movement * _speed;

        PlayerData playerPosition = new PlayerData();
        playerPosition.name = name;
        playerPosition.x = transform.position.x;
        playerPosition.y = transform.position.y;
        playerPosition.z = transform.position.z;

        string jsonPos = JsonMapper.ToJson(playerPosition);

        _socket.Emit("SendPlayerData", new JSONObject(jsonPos));
    }
}
