using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {
	[Header("Prefabs")]
	[SerializeField]
	private Bank[] banks;
	[SerializeField]
	private Building[] buildings;
	[SerializeField]
	private Hideout[] hideouts;
	[SerializeField]
	private PoliceStation[] policeStations;
	[SerializeField]
	private Grass grassTile;
	[SerializeField]
	private Road roadTile;
	[SerializeField]
	private Pavement pavementTile;

	[Header("Settings")]
	[SerializeField]
	private Settings settings;

	private GameObject map;

	private string[][] _placeholderBlocks;
	private int multiplier;

	void Start () {
		map = new GameObject ("_map");
		multiplier = Mathf.RoundToInt (settings.blockSize + settings.pavementWidth * 2 + settings.roadWidth * 2);
		int x = Mathf.RoundToInt(multiplier * settings.blocks.x);
		int y = Mathf.RoundToInt(multiplier * settings.blocks.y);

		_placeholderBlocks = new string[x][];
		for (int i = 0; i < _placeholderBlocks.Length; i++) {
			_placeholderBlocks [i] = new string[y];
		}

		GeneratePavements ();
		GenerateRoads ();
		StartCoroutine( LayoutMap ());
	}
		
	void Update () {
	
	}

	void GenerateRoads() {
		for (int blockX = 0; blockX < settings.blocks.x; blockX++) {
			for (int blockY = 0; blockY < settings.blocks.y; blockY++) {
				//Debug.Log("Roads generated at: [" + blockX + ", " + blockY + "]");
				
				for (int x = 0; x < multiplier; x++) {
					for (int y = 0; y < multiplier; y++) {
						if (((y >= settings.roadWidth) && (y < settings.roadWidth + settings.blockSize + (settings.pavementWidth * 2))) && ((x >= settings.roadWidth) && (x < settings.roadWidth + settings.blockSize + (settings.pavementWidth * 2))))
							continue;

						//Road road = Instantiate (roadTile) as Road;

						//road.transform.position = new Vector2 (x + (multiplier * blockX), y + (multiplier * blockY));

						//road.transform.SetParent (roads.transform);

						_placeholderBlocks[x + (multiplier * blockX)][y + (multiplier * blockY)]  = "ROAD";
					}
				}

			}
		}
	}

	void GeneratePavements() {
		for (int blockX = 0; blockX < settings.blocks.x; blockX++) {
			for (int blockY = 0; blockY < settings.blocks.y; blockY++) {
				//Debug.Log("Pavements generated at: [" + blockX + ", " + blockY + "]");

				for (int x = 0; x < multiplier; x++) {
					for (int y = 0; y < multiplier; y++) {
						if (((y >= settings.roadWidth + settings.pavementWidth) 
							&& (y < settings.roadWidth + settings.blockSize + (settings.pavementWidth * 2) - settings.pavementWidth)) 
							&& ((x >= settings.roadWidth + settings.pavementWidth) 
								&& (x < settings.roadWidth + settings.blockSize + (settings.pavementWidth * 2) - settings.pavementWidth)))
							continue;

						//Pavement pavement = Instantiate (pavementTile) as Pavement;

						//new Vector2 (x + (multiplier * blockX), y + (multiplier * blockY));

						//pavement.transform.SetParent (pavements.transform);
						_placeholderBlocks[x + (multiplier * blockX)][y + (multiplier * blockY)] = "PAVEMENT";
					}
				}

			}
		}
	}

	/*void LayoutMap() {
		GameObject pavements = new GameObject ("Pavement");
		pavements.transform.SetParent (map.transform);

		GameObject roads = new GameObject ("Roads");
		roads.transform.SetParent (map.transform);

		for (int x = 0; x < _placeholderBlocks.Length; x++) {
			for (int y = 0; y < _placeholderBlocks [x].Length; y++) {
				switch (_placeholderBlocks [x] [y]) {
				case "ROAD":
					Road road = Instantiate (roadTile) as Road;
					road.transform.position = new Vector2 (x, y);
					road.transform.SetParent (roads.transform);
					break;
				case "PAVEMENT":
					Pavement pavement = Instantiate (pavementTile) as Pavement;
					pavement.transform.position = new Vector2 (x, y);
					pavement.transform.SetParent (pavements.transform);
					break;
				default:
					Debug.Log ("Block not found or empty.");
					break;
				}
			}
		}
	}*/

	IEnumerator LayoutMap () {
		GameObject pavements = new GameObject ("Pavement");
		pavements.transform.SetParent (map.transform);

		GameObject roads = new GameObject ("Roads");
		roads.transform.SetParent (map.transform);

		float startTime = Time.time;

		for (int x = 0; x < _placeholderBlocks.Length; x++) {
			for (int y = 0; y < _placeholderBlocks [x].Length; y++) {
				switch (_placeholderBlocks [x] [y]) {
				case "ROAD":
					Road road = Instantiate (roadTile) as Road;
					road.transform.position = new Vector2 (x, y);
					road.transform.SetParent (roads.transform);
					break;
				case "PAVEMENT":
					Pavement pavement = Instantiate (pavementTile) as Pavement;
					pavement.transform.position = new Vector2 (x, y);
					pavement.transform.SetParent (pavements.transform);
					break;
				default:
					//Debug.Log ("Block not found or empty.");
					break;
				}
			}

			yield return new WaitForEndOfFrame ();
		}

		Debug.Log (Time.time - startTime);

		yield break;
	}

	[Serializable]
	struct Settings {
		public Vector2 blocks;
		public int blockSize;
		public int roadWidth;
		public int pavementWidth;
	}
}
