using UnityEngine;
using System.Collections;

public class Bank : Building {
	public float moneyInBank;
	[SerializeField]
	private float minMoneyInBank, maxMoneyInBank;


	void Start () {
		moneyInBank = Random.Range (minMoneyInBank, maxMoneyInBank);
	}

	void Update () {
	
	}
}
