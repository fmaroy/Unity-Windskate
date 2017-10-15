using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Timeline;

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
	public int playerPosition = 0;



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
		foreach (int opponentId in raceManager.GetComponent<PlayersTrackOnRacetrack>().rankingList)
		{
			GameObject opponent = raceManager.GetComponent<PlayersTrackOnRacetrack> ().PlayersList[opponentId];
			camPreviewList.Add (opponent.GetComponentInChildren<Camera>());
			Vector2 position = new Vector2(50f, -125f -50f * (i));
			Quaternion rotation = Quaternion.identity;
			GameObject frame = (GameObject)Instantiate(opponentFramePrefab, position , rotation);
			frame.transform.parent = opponentFrame.transform;
			frame.transform.localPosition =  position;
			frame.transform.localScale =  new Vector3 (1f, 1f, 1f);
			opponentInfo.Add (frame);
			//updateOpponentName (i, opponent);
			i++;
		}
		playerPosition = GetPosition (raceManagerData.PlayerObject);
		displayManager ();
	}

	public void displayManager ()
	{
		string positionLabel = "";
		switch (playerPosition) {
		case 1:
			positionLabel = "1st !!!";
			break;
		case 2:
			positionLabel = "2nd";
			break;
		
		case 3:
			positionLabel = "3rd";
			break;
		default :
			positionLabel = "Better next time!";
			break;
		}

		positionFrame.GetComponentInChildren<TextMeshProUGUI> ().SetText (positionLabel);
		animateStars (playerPosition);
	}

	public void animateStars(int i)
	{
		List<GameObject> starsObjList = new List<GameObject> ();


		foreach (Transform star in starsContainer.transform) {
			starsObjList.Add (star.gameObject);
		}
		switch (playerPosition) {
		case 1:
			break;
		case 2:
			starsObjList [2].GetComponent<Animator> ().enabled = false;
			break;
		case 3:
			starsObjList [1].GetComponent<Animator> ().enabled = false;
			starsObjList [2].GetComponent<Animator> ().enabled = false;
			break;
		default:
			starsObjList [0].GetComponent<Animator> ().enabled = false;
			starsObjList [1].GetComponent<Animator> ().enabled = false;
			starsObjList [2].GetComponent<Animator> ().enabled = false;
			break;
			
		}

		starsContainer.GetComponent<PlayableDirector> ().Play ();

	}

	/// <summary>
	/// Gets the race position of the requested player/opponent
	/// </summary>
	/// <returns>The position.</returns>
	/// <param name="id">GameObject of the resquested player / Opponnent.</param>
	public int GetPosition (GameObject player)
	{
		int pos = 0;
		int playerId = raceManager.GetComponent<PlayersTrackOnRacetrack> ().PlayersList.IndexOf (player);//Id of the current player in the ranking tracking object
		pos = raceManager.GetComponent<PlayersTrackOnRacetrack>().rankingList[playerId] + 1;

		return pos;
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
		TextMeshProUGUI rankingMesh = opponentInfo [id].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI> ();
		Debug.Log (rankingMesh.text);
		rankingMesh.text = (id.ToString() + ".");
	}

	// Update is called once per frame
	void Update () {
		int i = 0;
		foreach (int opponentId in raceManager.GetComponent<PlayersTrackOnRacetrack>().rankingList)
		{
			GameObject opponent = raceManager.GetComponent<PlayersTrackOnRacetrack> ().PlayersList[opponentId];
			updatePreviewImage(i, opponent);
			updateOpponentName (i, opponent);
			i++;
		}
	}
}
