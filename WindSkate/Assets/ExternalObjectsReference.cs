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
	
	// Update is called once per frame
	void Update () {
		
	}
}
