using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSequence : MonoBehaviour {
	private InterfaceControl UIItemsData;
	public RaceManagerScript raceData;
	private GameObject IntroImage;
	private Animator IntroImageAnim;
	public int IntroductionStage = 0;
	public GameObject tutorialManager;

	// Use this for initialization
	void Start () {
		UIItemsData = GameObject.FindObjectOfType<InterfaceControl> ();
		IntroImage = UIItemsData.IntroImage;
		StartCoroutine(introductionManager (IntroductionStage));
		raceData = GameObject.Find ("RaceManager").GetComponent<RaceManagerScript> ();
		tutorialManager = raceData.gameObject.GetComponent<UserPreferenceScript> ().tutorialObj;
	}

	/*public void initiateCamera ()
	{
		//Camera.main.GetComponent<CameraControlScript> ().initCamera ();
		//Camera.main.GetComponent<CameraControlScript> ().setViewpoint (0);
	}*/

	IEnumerator animateStartbutton(float time)
	{
		float i = 0;
		float rate = 1 / time;
		yield return new WaitForSeconds(0.5f);
		foreach (Transform child in IntroImage.transform) {
			child.gameObject.SetActive (false);
		}
		while (i < 1) {
			IntroImage.transform.localScale = new Vector3 (0f, 1f, 0f);
			i += Time.deltaTime * rate;
			yield return 0;
		}

		foreach (Transform child in IntroImage.transform) {
			child.gameObject.SetActive (true);
		}
		IntroImage.SetActive(false);

		this.GetComponent<TrackIndicatorMotion> ().initTrackTrace();
		UIItemsData.SkipIntoButton.SetActive (true);
	}

	public IEnumerator disableObjectTimer (float time, GameObject obj, bool targetStatus)
	{
		float i = 0;
		float rate = 1 / time;
		while (i < 1) {
			i += Time.deltaTime * rate;
			yield return 0;
		}
		obj.SetActive (targetStatus);
	}

	public void startButton()
	{
		StartCoroutine (introductionManager (1));
	}

	public IEnumerator introductionManager (int stage)
	{
		if (stage == 0) {
			IntroImage.SetActive(true);
			UIItemsData.controlsVisibiltyHandler (false);
		}
		if (stage == 1) {
			StartCoroutine (animateStartbutton (0.5f));
			Camera.main.GetComponent<CinemachineControls> ().setTraceCamera (true);
			UIItemsData.controlsVisibiltyHandler (false);
			if (raceData.tutorialObj != null){
				raceData.tutorialObj.GetComponent<TutorialManager> ().enableTutorial("Trace tutorial");
			}
		}
		if (stage == 2) {
			Camera.main.GetComponent<CinemachineControls> ().setTraceCamera (false);
			UIItemsData.StartLights.SetActive (false);
			UIItemsData.controlsVisibiltyHandler (true);
			Debug.Log ("Race is Starting...");
			//initiateCamera ();
			this.GetComponent <TrackIndicatorMotion> ().enabled = false;
			UIItemsData.StartLights.SetActive (true);
			UIItemsData.SkipIntoButton.SetActive (false);

			yield return StartCoroutine (disableObjectTimer(5.5f, UIItemsData.StartLights, false));

			StartCoroutine(raceData.PlayerObject.GetComponent<PlayerStart>().PlayerStartSequence ());
			raceData.PlayerObject.GetComponentInChildren<Follow_track> ().updateTrackIndicator (); // required to force the update display of the projectors after the indication arrow
			//raceData.PlayerObject.GetComponent<PlayerStart>().tempPlayerStartSequence();

			foreach (GameObject opponentObj in raceData.OpponenentObjectsList) {
				StartCoroutine (opponentObj.GetComponent<PlayerStart> ().PlayerStartSequence ());
				//opponentObj.GetComponent<PlayerStart> ().tempPlayerStartSequence();
			}
			if (raceData.tutorialObj != null){
				raceData.tutorialObj.GetComponent<TutorialManager> ().enableTutorial("Basic Controls tutorial");
				raceData.tutorialObj.GetComponent<TutorialManager> ().enableTutorial("Wind Circle tutorial");
				raceData.tutorialObj.GetComponent<TutorialManager> ().enableTutorial("Track Mark tutorial");


			}

		}
		/*if (stage < 2) {
			UIItemsData.controlsVisibiltyHandler (false);
		} else {
			UIItemsData.controlsVisibiltyHandler (true);
		}*/
		yield return 0;
	}

	// Update is called once per frame
	void Update () {
		
		
	}
}
