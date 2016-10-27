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
	[SerializeField]
	private PinPointLocation _pinPointLocation;

	[Header("Settings")]
	[SerializeField]
	private Settings _settings;

	private GameObject _map;

	private string[][] _placeholderBlocks;
	private GameObject[][] _pinPointLocationGameobjects;
	private int _multiplier;
	private GameObject _pinPointLocations;

	void Start () {
		_map = new GameObject ("MAP");
		_pinPointLocations = new GameObject ("PinPointLocations");
		_pinPointLocations.transform.SetParent (_map.transform);
		_multiplier = Mathf.RoundToInt (_settings.blockSize + _settings.pavementWidth * 2 + _settings.roadWidth * 2);

		int x = Mathf.RoundToInt(_multiplier * _settings.blocks.x);
		int y = Mathf.RoundToInt(_multiplier * _settings.blocks.y);

		_placeholderBlocks = new string[x][];
		for (int i = 0; i < _placeholderBlocks.Length; i++) {
			_placeholderBlocks [i] = new string[y];
		}
			
		_pinPointLocationGameobjects = new string[_settings.blocks.x * 2][];
		for (int i = 0; i < _placeholderBlocks.Length; i++) {
			_pinPointLocationGameobjects [i] = new string[_settings.blocks.y * 2];
		}

		GeneratePavements ();
		GenerateRoads ();
		StartCoroutine( LayoutMap ());
	}

	/// <summary>
	/// Generates the roads.
	/// </summary>
	void GenerateRoads() {
		for (int blockX = 0; blockX < _settings.blocks.x; blockX++) {
			for (int blockY = 0; blockY < _settings.blocks.y; blockY++) {
				for (int x = 0; x < _multiplier; x++) {
					for (int y = 0; y < _multiplier; y++) {
						if (((y >= _settings.roadWidth) && (y < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2))) && ((x >= _settings.roadWidth) && (x < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2))))
							continue;
						_placeholderBlocks[x + (_multiplier * blockX)][y + (_multiplier * blockY)]  = "ROAD";
					}
				}

			}
		}
	}

	/// <summary>
	/// Generates the pavements.
	/// </summary>
	void GeneratePavements() {
		for (int blockX = 0; blockX < _settings.blocks.x; blockX++) {
			for (int blockY = 0; blockY < _settings.blocks.y; blockY++) {
				for (int x = 0; x < _multiplier; x++) {
					for (int y = 0; y < _multiplier; y++) {
						placePinPoint (x, y, blockX, blockY);

						if (((y >= _settings.roadWidth + _settings.pavementWidth)
						    && (y < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2) - _settings.pavementWidth))
						    && ((x >= _settings.roadWidth + _settings.pavementWidth)
						    && (x < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2) - _settings.pavementWidth)))
							continue;

						if (y != _settings.roadWidth) { 
							_placeholderBlocks [x + (_multiplier * blockX)] [y + (_multiplier * blockY)] = "PAVEMENT"; 
						} 
						else { 
							_placeholderBlocks [x + (_multiplier * blockX)] [y + (_multiplier * blockY)] = "PAVEMENT_UNDER"; 
						}
					}
				}

			}
		}
	}

	/// <summary>
	/// Checks first if should place a pinpoint, if so then places one.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="blockX">Block x.</param>
	/// <param name="blockY">Block y.</param>
	void placePinPoint(int x, int y, int blockX, int blockY) {
		if (x == 0 && y == 0
			|| x == 0 && y == _multiplier
			|| x == _multiplier && y == 0
			|| x == _multiplier && y == _multiplier
			|| x == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2 && y == 0
			|| x == 0 && y == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2
			|| x == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2 && y == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2) {
			PinPointLocation pinPoint = Instantiate (_pinPointLocation) as PinPointLocation;
			pinPoint.gameObject.transform.position = new Vector2 (x + (_multiplier * blockX) + _settings.pavementWidth, y + (_multiplier * blockY) + _settings.pavementWidth);
			pinPoint.gameObject.transform.SetParent (_pinPointLocations.transform);
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

	/// <summary>
	/// Layouts the map.
	/// </summary>
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
