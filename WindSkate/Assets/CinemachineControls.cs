using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineControls : MonoBehaviour {

	int numberOfFollowCameras = 2;
	public bool isIntroScene = false;
	private RaceManagerScript raceData;
	// referenced to the player object elements to be tracked;
	public GameObject playerOrientTarget;
	public GameObject playerPosTarget;
	//placeholder objects following the player motions
	public GameObject playerRefPos;
	public GameObject playerRefOrient;
	// following can be gameplay, trackdemo,  or intro
	//public int cameraStateID = 0;
	public List<GameObject> GameCameraList = new List<GameObject>();
	public int gameViewId = 0;
	public List<GameObject> CrashCameraList = new List<GameObject>();
	public GameObject OrbitCamera;
	public CinemachineFreeLook OrbitCameraData;
	public GameObject camFollowTraceObj;
	public GameObject cameraContainers;
	public Animator cameraAnimator;
	public float introSceneCamTimer = 5; 
	private float camtimer;


	// Use this for initialization
	void Start () {
		cameraAnimator = this.GetComponent<Animator> ();
		raceData = GameObject.Find ("RaceManager").GetComponent<RaceManagerScript>();
		isIntroScene = raceData.gameObject.GetComponent<UserPreferenceScript> ().IntroScene;
		camtimer = 0;
		OrbitCameraData = OrbitCamera.GetComponent<CinemachineFreeLook> ();
		initCamera ();
	}
		
	public void initCamera ()
	{
		CrashCameraHandler (false, 0);
		GameCameraHandler (0);
		camFollowTraceObj.SetActive (false);
	}
		
	public void CrashCameraHandler (bool isCrashed, int CrashId)
	{
		//Debug.Log ("Camera crash " + isCrashed);
		if (isCrashed) {
			// enables the crash camera #i
			int i = 0;
			foreach (GameObject camObj in CrashCameraList) {
				if (CrashId == i) {
					//Debug.Log ("enables camera : " + camObj.name);
					camObj.SetActive (true);
				} else {
					//Debug.Log ("disables camera : " + camObj.name);
					camObj.SetActive (false);
				}
				i++;
			}
		} else {
			// disables the crash camera
			//Debug.Log("disables all cameras");
			foreach (GameObject camObj in CrashCameraList) {
				//Debug.Log ("disables camera : " + camObj.name);
				camObj.SetActive (false);
			}
		}
	}

	public void disableOrbitCamera()
	{
		OrbitCamera.SetActive(false);
		OrbitCameraData.m_XAxis.Value = 90f;

	}

	public void orbitCameraHandler(float CamShiftX, float CamShiftY)
	{
		// enbales roation camera
		OrbitCamera.SetActive(true);
		OrbitCameraData.m_XAxis.Value = CamShiftX;
		OrbitCameraData.m_YAxis.Value = CamShiftY;



	}

	public void GameCameraHandler(int id)
	{
		Debug.Log ("load game camera : " + id);
		//CrashCameraHandler (false, 0);
		int i = 0;
		foreach (GameObject camObj in GameCameraList) {
			if (id == i) {
				Debug.Log ("enables camera : " + camObj.name);
				camObj.SetActive (true);
			} else {
				Debug.Log ("disables camera : " + camObj.name);
				camObj.SetActive (false);
			}
			i++;
		}
	}

	public void nextView() {
		int maximumNumberOfViewpoints = GameCameraList.Count;
		gameViewId = gameViewId + 1;
		if (gameViewId >= maximumNumberOfViewpoints) {
			gameViewId = 0;
		}
		GameCameraHandler (gameViewId);
	}

	// Update is called once per frame

	void FixedUpdate(){
		playerRefPos.transform.position = playerPosTarget.transform.position;

		playerRefOrient.transform.position = playerOrientTarget.transform.position;
		playerRefOrient.transform.eulerAngles = new Vector3(0f, playerOrientTarget.transform.eulerAngles.y, 0f);
	}

	void Update () {
		
		if (isIntroScene) 
		{
			camtimer = camtimer + Time.deltaTime;
			if (camtimer > introSceneCamTimer) {
				camtimer = 0;
				nextView ();
			}
		}
	}
}
