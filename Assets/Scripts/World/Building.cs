using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

	private List<GameObject> peopleInBuilding = new List<GameObject> ();

	[SerializeField]
	private SpriteRenderer _roof;

	private float _roofFadeLevel = 1f;
	private bool _roofVisible = true;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			peopleInBuilding.Add (other.gameObject);
		}

		if (peopleInBuilding.Count != 0) {
			_roofVisible = false;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			peopleInBuilding.Remove (other.gameObject);
		}

		if (peopleInBuilding.Count == 0) {
			_roofVisible = true;
		}
	}

	void Update() {
		if (_roofVisible) {
			_roofFadeLevel = Mathf.SmoothStep (_roofFadeLevel, 1f, 10f * Time.deltaTime);
		} else {
			_roofFadeLevel = Mathf.SmoothStep (_roofFadeLevel, 0f, 10f * Time.deltaTime);
		}

		Color roofColor = _roof.color;
		roofColor.a = _roofFadeLevel;
		_roof.color = roofColor;
	}
}
