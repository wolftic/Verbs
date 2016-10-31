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

	private float _curSpeed {
		set {
			_curSpeed = Mathf.Clamp (value, _speed, _runSpeed);
		}
		get {
			return _curSpeed;
		}
	}

	private Rigidbody2D rigidbody2d;

    public string name;

    Pos pos = new Pos();

    void Start () {
		rigidbody2d = GetComponent<Rigidbody2D> ();

        _socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
    }
	
	void Update () {
		Vector2 movement = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		rigidbody2d.velocity = movement * _speed;

        pos.x = rigidbody2d.transform.position.x;
        pos.y = rigidbody2d.transform.position.y;
        pos.z = rigidbody2d.transform.position.z;

        string jsonPos = JsonMapper.ToJson(pos);
        _socket.Emit("Move", new JSONObject(jsonPos));
	}
}
