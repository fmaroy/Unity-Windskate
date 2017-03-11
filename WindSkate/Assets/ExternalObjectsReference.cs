using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalObjectsReference : MonoBehaviour {

	public GameObject raceManagerObject;
	public UserPreferenceScript UserPrefs;
	// Use this for initialization
	void Start () {
		raceManagerObject = GameObject.Find ("RaceManager");
		UserPrefs = raceManagerObject.GetComponent<UserPreferenceScript> ();
	}

	public bool initPlayer()
	{
		bool worked = false;
		raceManagerObject = GameObject.Find ("RaceManager");
		UserPrefs = raceManagerObject.GetComponent<UserPreferenceScript> ();
		if (UserPrefs != null) {
			worked = true;
		}
		return worked;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
