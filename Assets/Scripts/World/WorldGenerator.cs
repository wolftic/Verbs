using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {
	[Header("Prefabs")]
	[SerializeField]
	private Bank[] _banks;
	[SerializeField]
	private Building[] _buildings;
	[SerializeField]
	private Hideout[] _hideouts;
	[SerializeField]
	private PoliceStation[] _policeStations;
	[SerializeField]
	private Grass _grassTile;
	[SerializeField]
	private Road _roadTile;
	[SerializeField]
	private Pavement _pavementTile;
	[SerializeField]
	private Pavement _pavementUnderTile;

	[Header("Settings")]
	[SerializeField]
	private Settings _settings;

	private GameObject _map;

	private string[][] _placeholderBlocks;
	private int _multiplier;

	void Start () {
		_map = new GameObject ("__map");
		_multiplier = Mathf.RoundToInt (_settings.blockSize + _settings.pavementWidth * 2 + _settings.roadWidth * 2);
		int x = Mathf.RoundToInt(_multiplier * _settings.blocks.x);
		int y = Mathf.RoundToInt(_multiplier * _settings.blocks.y);

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
		for (int blockX = 0; blockX < _settings.blocks.x; blockX++) {
			for (int blockY = 0; blockY < _settings.blocks.y; blockY++) {
				//Debug.Log("Roads generated at: [" + blockX + ", " + blockY + "]");
				
				for (int x = 0; x < _multiplier; x++) {
					for (int y = 0; y < _multiplier; y++) {
						if (((y >= _settings.roadWidth) && (y < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2))) && ((x >= _settings.roadWidth) && (x < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2))))
							continue;

						//Road road = Instantiate (_roadTile) as Road;

						//road.transform.position = new Vector2 (x + (_multiplier * blockX), y + (_multiplier * blockY));

						//road.transform.SetParent (roads.transform);

						_placeholderBlocks[x + (_multiplier * blockX)][y + (_multiplier * blockY)]  = "ROAD";
					}
				}

			}
		}
	}

	void GeneratePavements() {
		for (int blockX = 0; blockX < _settings.blocks.x; blockX++) {
			for (int blockY = 0; blockY < _settings.blocks.y; blockY++) {
				//Debug.Log("Pavements generated at: [" + blockX + ", " + blockY + "]");

				for (int x = 0; x < _multiplier; x++) {
					for (int y = 0; y < _multiplier; y++) {
						if (((y >= _settings.roadWidth + _settings.pavementWidth)
						    && (y < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2) - _settings.pavementWidth))
						    && ((x >= _settings.roadWidth + _settings.pavementWidth)
						    && (x < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2) - _settings.pavementWidth)))
							continue;

						//Pavement pavement = Instantiate (_pavementTile) as Pavement;

						//new Vector2 (x + (_multiplier * blockX), y + (_multiplier * blockY));

						//pavement.transform.SetParent (pavements.transform);

						if (y != _settings.roadWidth) { 
							_placeholderBlocks [x + (_multiplier * blockX)] [y + (_multiplier * blockY)] = "PAVEMENT"; 
						} 
						else { 
							_placeholderBlocks [x + (_multiplier * blockX)] [y + (_multiplier * blockY)] = "PAVEMENT_UNDER"; 
							Debug.Log (y);
						}
					}
				}

			}
		}
	}

	/*void Layout_map() {
		GameObject pavements = new GameObject ("Pavement");
		pavements.transform.SetParent (_map.transform);

		GameObject roads = new GameObject ("Roads");
		roads.transform.SetParent (_map.transform);

		for (int x = 0; x < _placeholderBlocks.Length; x++) {
			for (int y = 0; y < _placeholderBlocks [x].Length; y++) {
				switch (_placeholderBlocks [x] [y]) {
				case "ROAD":
					Road road = Instantiate (_roadTile) as Road;
					road.transform.position = new Vector2 (x, y);
					road.transform.SetParent (roads.transform);
					break;
				case "PAVEMENT":
					Pavement pavement = Instantiate (_pavementTile) as Pavement;
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
		pavements.transform.SetParent (_map.transform);

		GameObject roads = new GameObject ("Roads");
		roads.transform.SetParent (_map.transform);

		GameObject grasses = new GameObject ("Grasses");
		grasses.transform.SetParent (_map.transform);

		GameObject _buildings_map = new GameObject ("_buildings");
		_buildings_map.transform.SetParent (_map.transform);

		float startTime = Time.time;

		for (int x = 0; x < _placeholderBlocks.Length; x++) {
			for (int y = 0; y < _placeholderBlocks [x].Length; y++) {
				switch (_placeholderBlocks [x] [y]) {
				case "ROAD":
					Road road = Instantiate (_roadTile) as Road;
					road.transform.position = new Vector2 (x, y);
					road.transform.SetParent (roads.transform);
					break;
				case "PAVEMENT":
					Pavement pavement = Instantiate (_pavementTile) as Pavement;
					pavement.transform.position = new Vector2 (x, y);
					pavement.transform.SetParent (pavements.transform);
					break;
				case "PAVEMENT_UNDER":
					Pavement pavementUnder = Instantiate (_pavementUnderTile) as Pavement;
					pavementUnder.transform.position = new Vector2 (x, y);
					pavementUnder.transform.SetParent (pavements.transform);
					break;
				default:
					Grass grass = Instantiate (_grassTile) as Grass;
					grass.transform.position = new Vector2 (x, y);
					grass.transform.SetParent (grasses.transform);
					break;
				}
			}

			yield return new WaitForEndOfFrame ();
		}

		for (int blockX = 0; blockX < _settings.blocks.x; blockX++) {
			for (int blockY = 0; blockY < _settings.blocks.y; blockY++) {
				int rand = Mathf.RoundToInt ( UnityEngine.Random.Range(0, _buildings.Length) );

				Building building = Instantiate (_buildings [rand]) as Building;

				float x = (blockX + 1) * (_multiplier / 2) - 0.5f + (_multiplier * blockX / 2);
				float y = (blockY + 1) * (_multiplier / 2) - 0.5f + (_multiplier * blockY / 2);

				building.transform.position = new Vector2 (x, y);
				building.transform.SetParent (_buildings_map.transform);
			}
		}

		Debug.Log (Time.time - startTime);

		yield break;
	}

	[System.Serializable]
	struct Settings {
		public Vector2 blocks;
		public int blockSize;
		public int roadWidth;
		public int pavementWidth;
	}
}
