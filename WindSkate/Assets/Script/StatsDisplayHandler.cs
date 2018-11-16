using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplayHandler : MonoBehaviour {


	public PersistentParameters param;
	public GameObject StarsStatsObject;
	public GameObject CoinsStatsObject;
	public int numbStars;
	public int numbCoins;

	// Use this for initialization
	void Start () {
		GameObject raceManagerObject = GameObject.Find ("RaceManager");
		param = raceManagerObject.GetComponent<UserPreferenceScript> ().PersistentParameterData;

	}
	
	// Update is called once per frame
	void Update () {
		numbStars = param.NumberOfStars;
		numbCoins = param.NumberOfCoins;
		StarsStatsObject.GetComponentInChildren<TextMeshProUGUI> ().SetText (numbStars.ToString());
		CoinsStatsObject.GetComponentInChildren<TextMeshProUGUI> ().SetText (numbCoins.ToString());
	}
}
