using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceIntroScript : MonoBehaviour {

	public GameObject raceManager;
	public bool isScene;
	public RaceManagerScript raceManagerData;
	public PersistentParameters persistentSceneData;
	public RaceType raceTypeData;
	public GameObject weatherPannel;
	public GameObject opponentPannel;
    public GameObject opponentFramePrefab;
    public Vector2 initWritingPosition;
	public WindGustsBehavior windData;
	public List<Camera> camPreviewList = new List<Camera>();
	public List<GameObject> opponentInfo = new List<GameObject> ();

	// Use this for initialization
	void Start () {
        raceManager = GameObject.Find("RaceManager");
        raceManagerData = raceManager.GetComponent<RaceManagerScript>();
        windData = raceManagerData.WindData;
		if (GameObject.Find ("Scene_Manager") != null) {
			isScene = true;
			persistentSceneData = GameObject.Find ("Scene_Manager").GetComponent<PersistentParameters> ();
			raceTypeData = GameObject.Find ("Scene_Manager").GetComponent<RaceType> ();
		} else {
			isScene = false;
		}

		if (isScene) {
			Debug.Log(persistentSceneData.trackList[persistentSceneData.currentSingleRaceDefinition.raceId].trackName);
			opponentPannel.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText("Track : " + persistentSceneData.trackList[persistentSceneData.currentSingleRaceDefinition.raceId].trackName + "\nType of Race : " + persistentSceneData.trackList[persistentSceneData.currentSingleRaceDefinition.raceId].typeOfRace);
		}


		int i = 0;
		foreach (GameObject opponent in raceManagerData.OpponenentObjectsList)
		{
			camPreviewList.Add (opponent.GetComponentInChildren<Camera>());
			Vector2 position = new Vector2(0f, -125f -50f * (i));
			Quaternion rotation = Quaternion.identity;
			GameObject frame = (GameObject)Instantiate(opponentFramePrefab, position , rotation);
			frame.transform.parent = opponentPannel.transform;
			frame.transform.localPosition =  position;
			frame.transform.localScale =  new Vector3 (1f, 1f, 1f);
			opponentInfo.Add (frame);
			//updateOpponentName (i, opponent);
			i++;
		}




    }

    void writeOpponentPannel(int positionID , GameObject opponent, GameObject prefab)
    {
        //Debug.Log(opponent.transform.Find("ImageFace").gameObject.name);
		//frame.GetComponent<RawImage>().texture = opponent.GetComponentInChildren<Camera>().targetTexture;
        //frame.GetComponent<RawImage>().texture = opponent.transform.Find("ImageFace").gameObject.GetComponent<PlayerfaceImageHandler>().rt;
    }

	void updatePreviewImage(int id, GameObject opponent)
	{
		opponentInfo[id].GetComponent<RawImage>().texture = opponent.GetComponentInChildren<Camera>().targetTexture;
	}
	void updateOpponentName(int id, GameObject opponent)
	{
		Debug.Log (opponent.GetComponent<playerInventory> ().PlayerName);
		Debug.Log (opponentInfo [id].name);
		Debug.Log (opponentInfo [id].transform.GetChild(0).gameObject.name);
		TextMeshProUGUI textMesh = opponentInfo [id].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI> ();
		Debug.Log (textMesh.text);
		textMesh.text = opponent.GetComponent<playerInventory> ().PlayerName;
	}


	// Update is called once per frame
	void Update () {


		int i = 0;
		foreach (GameObject opponent in raceManagerData.OpponenentObjectsList)
		{
			updatePreviewImage(i, opponent);
			updateOpponentName (i, opponent);
			i++;
		}
	}
}
