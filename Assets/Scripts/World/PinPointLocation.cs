using UnityEngine;
using System.Collections;

public class PinPointLocation : MonoBehaviour {
	public GameObject up, right, down, left;

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, .5f);

		Gizmos.color = Color.green;
		if(up != null)Gizmos.DrawRay (transform.position, up.transform.position);
		if(right != null)Gizmos.DrawRay (transform.position, right.transform.position);
		if(down != null)Gizmos.DrawRay (transform.position, down.transform.position);
		if(left != null)Gizmos.DrawRay (transform.position, left.transform.position);
	}
}
