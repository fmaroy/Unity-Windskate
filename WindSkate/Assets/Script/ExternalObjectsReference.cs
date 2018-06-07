using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalObjectsReference : MonoBehaviour {

	public GameObject raceManagerObject;
	public UserPreferenceScript UserPrefs;
	public GameObject SceneManagerObject;
	public PersistentParameters SceneData;
	public GameObject WindGust;
	public GameObject UIControls;
	public InterfaceControl UIControlData;
	public GameObject Windindicator;
	public GameObject FinishImage;
	public GameObject PauseButton;
	public GameObject currentCamera;

	// Use this for initialization
	void Start () {
		initPlayer ();
		currentCamera = Camera.main.gameObject;
	}

	public bool initPlayer()
	{
		bool worked = false;
		raceManagerObject = GameObject.Find ("RaceManager");
		UserPrefs = raceManagerObject.GetComponent<UserPreferenceScript> ();
		SceneManagerObject = UserPrefs.SceneManagerObject;
		SceneData = UserPrefs.PersistentParameterData;
		if (SceneData != null) {
			this.GetComponent<tricksHandlingScript> ().localTackList = SceneData.tackList;
			this.GetComponent<tricksHandlingScript> ().localJibeList = SceneData.jibeList;
		}
		else
		{
			this.GetComponent<tricksHandlingScript> ().localTackList = UserPrefs.localTackManoeuvres;
			this.GetComponent<tricksHandlingScript> ().localJibeList = UserPrefs.localJibeManoeuvres;
		}

		WindGust = GameObject.Find ("WindGusts");
		if (GetComponent<PlayerCollision> ().isPlayer == true) {
            UIControls = GameObject.Find ("OnScreenButtons");
			if (UIControls != null) {
				UIControlData = UIControls.GetComponent<InterfaceControl> ();
				Windindicator = UIControlData.WindIndicator;
				FinishImage = UIControlData.FinishImage;
				PauseButton = UIControlData.PauseButton;
			}
		}


		if (UserPrefs != null) {
			worked = true;
		}

		return worked;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
