using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
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

	void Start () {
		rigidbody2d = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		Vector2 movement = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		rigidbody2d.velocity = movement * _speed;
	}
}
