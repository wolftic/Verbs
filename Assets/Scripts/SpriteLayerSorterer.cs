using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteLayerSorterer : MonoBehaviour {
	private SpriteRenderer sprite;

	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		sprite.sortingOrder = -Mathf.RoundToInt(transform.position.y * 2f);
	}
}
