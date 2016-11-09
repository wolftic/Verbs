using UnityEngine;
using UnityEngine.Events;
//using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CitizenSpawner))]
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
	[SerializeField]
	private GameObject _traffic_light;
	[SerializeField]
	private GameObject _light;
	[SerializeField]
	private GameObject _busStop;
	[SerializeField]
	private GameObject[] _bins;
	[SerializeField]
	private GameObject[] _props;
	[SerializeField]
	private GameObject[] _trees;
	[SerializeField]
	private GameObject _verticalBlockage;
	[SerializeField]
	private GameObject _horizontalBlockage;
	[SerializeField]
	private GameObject[] _cornerBlockage;

	[Header("Settings")]
	[SerializeField]
	private Settings _settings;

	[Header("Events")]
	[SerializeField]
	public UnityEvent OnMapLoadDone;

	private GameObject _map;

	private string[][] _placeholderBlocks;
	private GameObject[][] _pinPointLocationGameobjects;
	private int _multiplier;
	private GameObject _pinPointLocations;
	private GameObject _propObjects;
	private GameObject _blockages;
	private CitizenSpawner _citizenSpawner;

	void Start () {
		_map = new GameObject ("MAP");

		_pinPointLocations = new GameObject ("PinPointLocations");
		_propObjects = new GameObject ("Props");
		_blockages = new GameObject ("Blockages");

		_pinPointLocations.transform.SetParent (_map.transform);
		_propObjects.transform.SetParent (_map.transform);
		_blockages.transform.SetParent (_map.transform);

		_citizenSpawner = GetComponent<CitizenSpawner> ();
		_multiplier = Mathf.RoundToInt (_settings.blockSize + _settings.pavementWidth * 2 + _settings.roadWidth * 2);

		int x = Mathf.RoundToInt(_multiplier * _settings.blocks.x);
		int y = Mathf.RoundToInt(_multiplier * _settings.blocks.y);

		_placeholderBlocks = new string[x][];
		for (int i = 0; i < _placeholderBlocks.Length; i++) {
			_placeholderBlocks [i] = new string[y];
		}
			
		_pinPointLocationGameobjects = new GameObject[Mathf.RoundToInt(_settings.blocks.x * 2)][];
		for (int i = 0; i < _pinPointLocationGameobjects.Length; i++) {
			_pinPointLocationGameobjects [i] = new GameObject[Mathf.RoundToInt(_settings.blocks.y * 2)];
		}

		OnMapLoadDone.AddListener (_citizenSpawner.GetPinPointLocations);
		OnMapLoadDone.AddListener (_citizenSpawner.SpawnCitizens);
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
						placeBlockage (x, y, blockX, blockY);

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
						placeProp (x, y, blockX, blockY);

						if (((y >= _settings.roadWidth + _settings.pavementWidth)
						    && (y < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2) - _settings.pavementWidth))
						    && ((x >= _settings.roadWidth + _settings.pavementWidth)
						    && (x < _settings.roadWidth + _settings.blockSize + (_settings.pavementWidth * 2) - _settings.pavementWidth)))
							continue;

						/*float r = Random.Range (0, 100);
						if (r <= _settings._propspawnPercentage) {
							int rInt = Mathf.RoundToInt (Random.Range (0, _props.Length));
							GameObject prop = Instantiate(_props[rInt]);
							prop.transform.position = new Vector2 (x + (_multiplier * blockX), y + (_multiplier * blockY));
						}*/

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
	/// Places the props on the map.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="blockX">Block x.</param>
	/// <param name="blockY">Block y.</param>
	void placeProp(int x, int y, int blockX, int blockY) {
		GameObject prop = null;
		if (x == Mathf.RoundToInt (_multiplier / 2) && y == _settings.pavementWidth) {
			prop = _busStop;
		}

		if (x == 0 && y == Mathf.RoundToInt (_multiplier / 2) - _settings.pavementWidth) {
			prop = _light;
		}

		if (x == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2
		    && y == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2) {
			prop = _traffic_light;
		}

		if (x == 0 && y == 0) {
			int rInt = Mathf.RoundToInt (Random.Range (0, _props.Length));
			prop = _props [rInt];
		}

		if (x == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2 && y == 0) {
			int rInt = Mathf.RoundToInt (Random.Range (0, _bins.Length));
			prop = _bins [rInt];
		}

		if (x == 0 && y == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2) {
			int rInt = Mathf.RoundToInt (Random.Range (0, _trees.Length));
			prop = _trees [rInt];
		}

		if (prop == null) return;
		prop = Instantiate (prop);
		prop.transform.position = new Vector2 (x + (_multiplier * blockX) + _settings.pavementWidth, y + (_multiplier * blockY) + _settings.pavementWidth);
		prop.transform.SetParent (_propObjects.transform);
	}

	/// <summary>
	/// Places the blockages.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="blockX">Block x.</param>
	/// <param name="blockY">Block y.</param>
	void placeBlockage(int x, int y, int blockX, int blockY) {
		GameObject blockage = null;

		if (blockX == 0) {
			if (x == 0) {
				blockage = _verticalBlockage;
			}
		} 
		if (blockY == 0) {
			if (y == 0) {
				blockage = _horizontalBlockage;
			}
		} 
		if (blockX == _settings.blocks.x - 1) {
			if (x == _multiplier-1) {
				blockage = _verticalBlockage;
			}
		} 
		if (blockY == _settings.blocks.y - 1) {
			if (y == _multiplier-1) {
				blockage = _horizontalBlockage;
			}
		}

		if (x == 0 && y == 0 && blockX == 0 && blockY == 0) {
			blockage = _cornerBlockage [0];
		} else if (x == 0 && y == _multiplier - 1 && blockX == 0 && blockY == _settings.blocks.y - 1) {
			blockage = _cornerBlockage [1];
		} else if(x == _multiplier - 1 && y == 0 && blockX == _settings.blocks.x - 1 && blockY == 0) {
			blockage = _cornerBlockage [2];
		} else if(x == _multiplier - 1 && y == _multiplier - 1 && blockX == _settings.blocks.x - 1 && blockY == _settings.blocks.y - 1) {
			blockage = _cornerBlockage [3];
		}

		if (blockage == null)
			return;

		blockage = Instantiate (blockage);
		blockage.transform.position = new Vector2 (x + (_multiplier * blockX), y + (_multiplier * blockY));
		blockage.transform.SetParent (_blockages.transform);
	}

	/// <summary>
	/// Checks first if should place a pinpoint, if so then places one.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="blockX">Block x.</param>
	/// <param name="blockY">Block y.</param>
	void placePinPoint(int x, int y, int blockX, int blockY) {
		if (x == 1 && y == 1
			|| x == 1 && y == _multiplier
			|| x == _multiplier && y == 1
			|| x == _multiplier && y == _multiplier
			|| x == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2 - 1 && y == 1
			|| x == 1 && y == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2 - 1
			|| x == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2 - 1 && y == _multiplier - _settings.pavementWidth - _settings.roadWidth - _settings.pavementWidth / 2 - 1) {
			PinPointLocation pinPoint = Instantiate (_pinPointLocation) as PinPointLocation;
			pinPoint.gameObject.transform.position = new Vector2 (x + (_multiplier * blockX) + _settings.pavementWidth, y + (_multiplier * blockY) + _settings.pavementWidth);
			pinPoint.gameObject.transform.SetParent (_pinPointLocations.transform);
			pinPoint.maxDist = _multiplier - _settings.roadWidth * 2;
		}
	}

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
		OnMapLoadDone.Invoke ();

		yield break;
	}

	[System.Serializable]
	struct Settings {
		public Vector2 blocks;
		public int blockSize;
		public int roadWidth;
		public int pavementWidth;
		public float _propspawnPercentage;
	}
}
