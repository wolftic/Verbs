using UnityEngine;
using System.Collections;

public class CitizenSpawner : MonoBehaviour {
	[SerializeField]
	private CitizenMovement _citizen;

	[SerializeField]
	private int _amountOfCitizens = 1;

	private CitizenMovement[] _citizens;
	private GameObject _citizenMap;

	[HideInInspector]
	public PinPointLocation[] pinPointLocations;

	void Start () {
		_citizenMap = new GameObject ("Citizens");
		_citizens = new CitizenMovement[_amountOfCitizens];
	}

	public void GetPinPointLocations() {
		GameObject[] pinPoints = GameObject.FindGameObjectsWithTag ("PinPointLocation");
		pinPointLocations = new PinPointLocation[pinPoints.Length];
		for (int i = 0; i < pinPoints.Length; i++) {
			pinPointLocations [i] = pinPoints [i].GetComponent<PinPointLocation> ();
		}
	}

	public void SpawnCitizens() {
		for (int i = 0; i < _amountOfCitizens; i++) {
			SpawnCitizen (i);
		}
	}

	private void SpawnCitizen(int id) {
		CitizenMovement citizen = Instantiate (_citizen) as CitizenMovement;
		int r = Mathf.RoundToInt(Random.Range (0, pinPointLocations.Length));

		citizen.goToLocation = pinPointLocations [r];
		citizen.name = "Citizen " + id;	
		citizen.transform.SetParent (_citizenMap.transform);

		_citizens [id] = citizen;
	}
}
