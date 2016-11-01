using UnityEngine;
using System.Collections;

public class CitizenBody : MonoBehaviour {
	[Header("Body")]
	[SerializeField]
	private Sprite _left, _up, _down;
	[Header("Head")]
	[SerializeField]
	private Sprite _left_head, _up_head, _down_head;

	private SpriteRenderer _spriteRendererHead;
	private SpriteRenderer _spriteRendererBody;
	private int zValue = 0;

	public int direction {
		get{return direction;}
		set {
			ChangeSpriteWithDirection (value);
		}
	}

	void Start() {
		_spriteRendererBody = GetComponent<SpriteRenderer> ();
		_spriteRendererHead = transform.FindChild("Head").GetComponent<SpriteRenderer> ();
	}

	void Update() {
		_spriteRendererHead.sortingOrder = _spriteRendererBody.sortingOrder + zValue;
	}

	void ChangeSpriteWithDirection(int dir) {
		Sprite newSprite = _left;
		Sprite newSpriteHead = _left_head;
		zValue = 1;

		switch (dir) {
		case 0: //left
			newSprite = _left;
			newSpriteHead = _left_head;
			transform.localScale = new Vector3 (1, 1);
			_spriteRendererHead.transform.localPosition = new Vector3 (-0.15f, 0.6f); 
			break;
		case 1: //right
			newSprite = _left;
			newSpriteHead = _left_head;
			transform.localScale = new Vector3 (-1, 1);
			_spriteRendererHead.transform.localPosition = new Vector3 (-0.15f, 0.6f); 
			break;
		case 2: //up
			newSprite = _up;
			newSpriteHead = _up_head;
			_spriteRendererHead.transform.localPosition = new Vector3 (0f, 0.6f); 
			zValue = 0;
			break;
		default: //down
			newSprite = _down;
			newSpriteHead = _down_head;
			_spriteRendererHead.transform.localPosition = new Vector3 (0f, 0.6f); 
			break;
		}

		_spriteRendererBody.sprite = newSprite;
		_spriteRendererHead.sprite = newSpriteHead;
	}
}
