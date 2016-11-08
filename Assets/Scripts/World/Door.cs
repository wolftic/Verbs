using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	private SpriteRenderer sprite;
	[SerializeField]
	private Sprite open, closed;
	private Vector3 startPos;

	void Start () {
		startPos = transform.position;
		sprite = GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.transform.tag == "Player") {
			sprite.sprite = open;
			Vector3 pos = startPos;
			pos.x -= transform.localScale.x * 0.4f;
			transform.position = pos;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.transform.tag == "Player") {
			sprite.sprite = closed;
			transform.position = startPos;
		}
	}
}
