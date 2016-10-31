using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class CitizenMovement : MonoBehaviour {
	[SerializeField]
	private float _speed = 2f;

	private Rigidbody2D _rigidbody2d;

	[SerializeField]
	private GameObject[] _pinPointLocations;

<<<<<<< HEAD
	void Start () {
		_pinPointLocations = GameObject.FindGameObjectsWithTag ("PinPointLocation");
=======
	public PinPointLocation goToLocation;

	void Start () {
		_pinPointLocations = GameObject.FindGameObjectsWithTag ("PinPointLocation");
		if (goToLocation == null) {
			goToLocation = _pinPointLocations [0].GetComponent<PinPointLocation> ();
		}

		transform.position = goToLocation.transform.position;
>>>>>>> origin/master
		_rigidbody2d = GetComponent<Rigidbody2D> ();
	}

	void Update () {
<<<<<<< HEAD
	
=======
		WalkTo (goToLocation);
	}

	void WalkTo(PinPointLocation loc) {
		float dist = Vector3.Distance (transform.position, loc.transform.position);
		if (dist < 1f) {
			ChooseNextPinPoint ();
			return;
		} else {
			Vector3	movement = (goToLocation.transform.position - transform.position).normalized * _speed;
			_rigidbody2d.MovePosition (transform.position + movement * Time.deltaTime);
		}
		goToLocation = loc;
	}

	void ChooseNextPinPoint() {
		int rand = Mathf.RoundToInt (Random.Range (0, 4));
		PinPointLocation ppl = goToLocation;

		if (rand == 0) {
			ppl = goToLocation.left;		
		} else if (rand == 1) {
			ppl = goToLocation.right;		
		} else if (rand == 2) {
			ppl = goToLocation.up;
		} else {
			ppl = goToLocation.down;		
		}

		if (ppl == null || goToLocation == ppl) {
			ChooseNextPinPoint ();
			return;
		} else {
			goToLocation = ppl;
		}
>>>>>>> origin/master
	}
}
