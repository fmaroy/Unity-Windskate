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
    public InterfaceControl UIControlsData;

	public GameObject starsContainer;
	public Sprite starSprite;
	public Sprite starOutlineSprite;
    public List<GameObject> starsList = new List<GameObject>();
	public GameObject positionFrame;
	public GameObject opponentFrame;
	public GameObject opponentFramePrefab;
	public List<Camera> camPreviewList = new List<Camera>();
	public List<GameObject> opponentInfo = new List<GameObject> ();


    // Use this for initialization
    void Start()
    {
        raceManager = GameObject.Find("RaceManager");
        raceManagerData = raceManager.GetComponent<RaceManagerScript>();
        UIControlsData = GetComponentInParent<InterfaceControl>();

        if (GameObject.Find("Scene_Manager") != null)
        {
            isScene = true;
            persistentSceneData = GameObject.Find("Scene_Manager").GetComponent<PersistentParameters>();
        }
        else
        {
            isScene = false;
        }

        int i = 0;
        foreach (GameObject opponent in raceManagerData.OpponenentObjectsList)
        {
            camPreviewList.Add(opponent.GetComponentInChildren<Camera>());
            Vector2 position = new Vector2(0f, -125f - 50f * (i));
            Quaternion rotation = Quaternion.identity;
            GameObject frame = (GameObject)Instantiate(opponentFramePrefab, position, rotation);
            frame.transform.parent = opponentFrame.transform;
            frame.transform.localPosition = position;
            frame.transform.localScale = new Vector3(1f, 1f, 1f);
            opponentInfo.Add(frame);
            //updateOpponentName (i, opponent);
            i++;
        }

        foreach (Transform t in starsContainer.transform)
        {
            starsList.Add(t.gameObject);

        }

        endOfRace(); //displays arrival page when object is enabled

    }

    public void endOfRace ()
    {
        UIControlsData.controlsVisibiltyHandler(false);
        Time.timeScale = 0.0f;
        // retrieves the player's ranking
        int playerRanking = raceManager.GetComponent<PlayersTrackOnRacetrack>().rankingList[0] + 1;
        int numbStarsToDisplay = 3;
        int i = 0;
        foreach (GameObject starObj in starsList)
        {
            if (i < numbStarsToDisplay)
            {
                ParticleSystem part = starObj.GetComponentInChildren<ParticleSystem>();
                //part.startColor = starObj.GetComponent<Image>().color;
                //starObj.GetComponent<Animator>().Play("animArrivalStars");
            }
            i++;
        }

    }

    /*public void testParticles()
    {
        int playerRanking = raceManager.GetComponent<PlayersTrackOnRacetrack>().rankingList[0] + 1;
        int i = 0;
        foreach (GameObject starObj in starsList)
        {
            ParticleSystem part = starObj.GetComponentInChildren<ParticleSystem>();
            part.Play();
            i++;
        }
    }*/

	// Update is called once per frame
	void Update () {
		
	}
}
