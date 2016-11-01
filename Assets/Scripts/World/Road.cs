using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {
	[SerializeField]
	private Sprite[] _randomSprites;
	private SpriteRenderer _spriteRenderer;

	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer> ();
		int r = Mathf.RoundToInt (Random.Range (0, _randomSprites.Length));
		_spriteRenderer.sprite = _randomSprites [r];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
