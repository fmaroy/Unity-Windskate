using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrivalImageScript : MonoBehaviour {

	public GameObject raceManager;
	public bool isScene;
	public RaceManagerScript raceManagerData;
	public PersistentParameters persistentSceneData;

	public GameObject starsContainer;
	public Sprite starSprite;
	public Sprite starOutlineSprite;
	public GameObject positionFrame;
	public GameObject opponentFrame;
	public GameObject opponentFramePrefab;
	public List<Camera> camPreviewList = new List<Camera>();
	public List<GameObject> opponentInfo = new List<GameObject> ();


	// Use this for initialization
	void Start () {
		raceManager = GameObject.Find("RaceManager");
		raceManagerData = raceManager.GetComponent<RaceManagerScript>();

		if (GameObject.Find ("Scene_Manager") != null) {
			isScene = true;
			persistentSceneData = GameObject.Find ("Scene_Manager").GetComponent<PersistentParameters> ();
		} else {
			isScene = false;
		}

		int i = 0;
		foreach (GameObject opponent in raceManagerData.OpponenentObjectsList)
		{
			camPreviewList.Add (opponent.GetComponentInChildren<Camera>());
			Vector2 position = new Vector2(0f, -125f -50f * (i));
			Quaternion rotation = Quaternion.identity;
			GameObject frame = (GameObject)Instantiate(opponentFramePrefab, position , rotation);
			frame.transform.parent = opponentFrame.transform;
			frame.transform.localPosition =  position;
			frame.transform.localScale =  new Vector3 (1f, 1f, 1f);
			opponentInfo.Add (frame);
			//updateOpponentName (i, opponent);
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
