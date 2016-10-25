using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
	[SerializeField]
	private float speed = 2f;
	[SerializeField]
	private float runSpeed = 4f;

	private float curSpeed {
		set {
			curSpeed = Mathf.Clamp (value, speed, runSpeed);
		}
		get {
			return curSpeed;
		}
	}

	private Rigidbody2D rigidbody2d;

	void Start () {
		rigidbody2d = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		Vector2 movement = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		rigidbody2d.velocity = movement * speed;
	}
}
