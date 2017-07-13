using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceIntroScript : MonoBehaviour {

	public GameObject raceManager;
	public RaceManagerScript raceManagerData;
	public GameObject weatherPannel;
	public GameObject opponenentPannel;
	public WindGustsBehavior windData;


	// Use this for initialization
	void Start () {
		raceManager = GameObject.Find ("RaceManager");
		raceManagerData = raceManager.GetComponent<RaceManagerScript> ();
		windData = raceManagerData.WindData;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
