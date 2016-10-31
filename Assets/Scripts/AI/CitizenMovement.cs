using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CitizenMovement : MonoBehaviour {
	[SerializeField]
	private float _speed = 2f;

	private Rigidbody2D _rigidbody2d;

	[SerializeField]
	private GameObject[] _pinPointLocations;

	void Start () {
		_pinPointLocations = GameObject.FindGameObjectsWithTag ("PinPointLocation");
		_rigidbody2d = GetComponent<Rigidbody2D> ();
	}

	void Update () {
	
	}
}
