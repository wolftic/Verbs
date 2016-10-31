using UnityEngine;
using System.Collections;

public class PinPointLocation : MonoBehaviour {
	public PinPointLocation up, right, down, left;
	public float maxDist;

	void Start() {
		FindOtherPinPoints ();
	}

	void FindOtherPinPoints() {
		GameObject[] pinPoints = GameObject.FindGameObjectsWithTag ("PinPointLocation");
		for (int i = 0; i < pinPoints.Length; i++) {
			float dist = Vector3.Distance (transform.position, pinPoints [i].transform.position);
			if (dist < maxDist + 1) {
				GameObject pinPoint = pinPoints [i];

				float x0 = transform.position.x;
				float x1 = pinPoint.transform.position.x;
				float y0 = transform.position.y;
				float y1 = pinPoint.transform.position.y;

				if (x0 < x1 && y0 == y1) {
					//right
					right = pinPoint.GetComponent<PinPointLocation>();
				} else if (x0 > x1 && y0 == y1) {
					//left
					left = pinPoint.GetComponent<PinPointLocation>();
				} else if (x0 == x1 && y0 < y1) {
					//up
					up = pinPoint.GetComponent<PinPointLocation>();
				} else if (x0 == x1 && y0 > y1) {
					//down
					down = pinPoint.GetComponent<PinPointLocation>();
				}
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, .5f);

		Gizmos.color = Color.green;
		if(up != null)Gizmos.DrawLine (transform.position, up.transform.position);
		if(right != null)Gizmos.DrawLine (transform.position, right.transform.position);
		if(down != null)Gizmos.DrawLine(transform.position, down.transform.position);
		if(left != null)Gizmos.DrawLine (transform.position, left.transform.position);
	}
}
