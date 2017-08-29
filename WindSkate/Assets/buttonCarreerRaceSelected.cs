using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonCarreerRaceSelected : MonoBehaviour {

	public int raceId;
	public GameObject carreerRaceManager;
	public GameObject highlightObj;

	// Use this for initialization
	void Start () {
		
	}

	public void thisRaceDeselected()
	{
		highlightObj.SetActive (false);
	}

	public void thisRaceSelected()
	{
		highlightObj.SetActive (true);
		carreerRaceManager.GetComponentInParent<RaceSelector> ().selectedCarreerRace (raceId);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
