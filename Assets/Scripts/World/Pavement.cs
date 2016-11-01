using UnityEngine;
using System.Collections;

public class Pavement : MonoBehaviour {
	[SerializeField]
	private Sprite[] _randomSprites;
	private SpriteRenderer _spriteRenderer;

	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer> ();
		int r = Mathf.RoundToInt (Random.Range (0, _randomSprites.Length));
		_spriteRenderer.sprite = _randomSprites [r];
	}

	void Update () {
	
	}
}