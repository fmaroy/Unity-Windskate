using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceIntroScript : MonoBehaviour {

	public GameObject raceManager;
	public RaceManagerScript raceManagerData;
	public GameObject weatherPannel;
	public GameObject opponentPannel;
    public GameObject opponentFramePrefab;
    public Vector2 initWrtingPosition;
	public WindGustsBehavior windData;


	// Use this for initialization
	void Start () {
        raceManager = GameObject.Find("RaceManager");
        raceManagerData = raceManager.GetComponent<RaceManagerScript>();
        windData = raceManagerData.WindData;
        int i = 0;
        foreach (GameObject opponent in raceManagerData.OpponenentObjectsList)
        {
            writeOpponentPannel(i, opponent, opponentFramePrefab);
            i++;
        }
    }

    void writeOpponentPannel(int positionID , GameObject opponent, GameObject prefab)
    {
        //GameObject frame = new GameObject("Opponent_info"+positionID.ToString());
        //frame.transform.parent = opponentPannel.transform;
        Vector2 position = new Vector2(0f, -120f -40f * (positionID));
        Quaternion rotation = Quaternion.identity;
        GameObject frame = (GameObject)Instantiate(prefab, position , rotation);
        frame.transform.parent = opponentPannel.transform;
        frame.transform.localPosition =  position;
        //Debug.Log(opponent.transform.Find("ImageFace").gameObject.name);
        //RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        Camera cam = opponent.GetComponentInChildren<Camera>();
        RenderTexture rt = cam.GetComponent<Camera>().targetTexture ;
        frame.GetComponent<RawImage>().texture = rt;
        //frame.GetComponent<RawImage>().texture = opponent.transform.Find("ImageFace").gameObject.GetComponent<PlayerfaceImageHandler>().rt;
    }


	// Update is called once per frame
	void Update () {
		
	}
}
