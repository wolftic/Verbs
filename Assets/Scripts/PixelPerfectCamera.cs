using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour {
	public int PPU = 32;
	public float PPUscale = 1;
	public bool customScale = false;

	public int verticalResolution = 144;

	void Start () {
		if (customScale) {
			PPUscale = Screen.height / verticalResolution;
		}

		Apply ();
	}

	[ContextMenu("Apply changes!")]
	public void Apply () {
		GetComponent<Camera> ().orthographicSize = ((verticalResolution) / (PPUscale * PPU)) * .5f;
	}
}
