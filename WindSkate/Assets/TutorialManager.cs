using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TutorialManager : MonoBehaviour {

	public GameObject raceObject;
	public List<tutorialItem> tutorialList = new List<tutorialItem>();
	public bool timerToTutorialFlag;
	public tutorialItem currentTutorialItem;
	public float currentRealTime;
	// followinf is a list of objects that tuturials needs as reference
	public GameObject player;
	public GameObject opponentContainer;
	public GameObject windCircle;

	// Use this for initialization
	void Start () {
		raceObject = GameObject.Find ("RaceManager");
		raceObject.GetComponent<RaceManagerScript>().tutorialObj = this.gameObject;
		player = raceObject.GetComponent<RaceManagerScript> ().PlayerObject;
		opponentContainer = raceObject.GetComponent<RaceManagerScript> ().OpponentContainerObject;
		windCircle = player.GetComponentInChildren<CircleIndicators> ().gameObject;

		enableTutorial ("");
	}
	
	public void tutorialObjectAssignments()
	{
		foreach (tutorialItem item in tutorialList) {
			if (item.name == "Trace tutorial") {
				//item.camPos = raceObject.GetComponent<RaceManagerScript>().IntroductionObj.transform.GetChild(0).gameObject;
				//item.camOrient = raceObject.GetComponent<RaceManagerScript>().IntroductionObj.transform.GetChild(0).gameObject;
				item.tweeningObj1 = raceObject.GetComponent<RaceManagerScript>().IntroductionObj.transform.GetChild(0).gameObject;
				//item.tweeningObj2 = windCircle.GetComponentInChildren<WindGaugeScript> ().gameObject;
			}
			if (item.name == "Basic Controls tutorial") {
				item.tweeningObj1 = player.GetComponent<ExternalObjectsReference>().UIControlData.TurnLeftButton;
				item.tweeningObj2 = player.GetComponent<ExternalObjectsReference>().UIControlData.TurnRightButton;
			}
			if (item.name == "Wind Circle tutorial") {
				item.tweeningObj1 = windCircle.GetComponent<CircleIndicators> ().trueWindArrow;
				item.tweeningObj2 = windCircle.GetComponentInChildren<WindGaugeScript> ().gameObject;
			}
			if (item.name == "Track Arrow tutorial") {
				item.tweeningObj1 = windCircle.GetComponent<CircleIndicators> ().trackDirectionTickArrow;
				item.tweeningObj2 = windCircle.GetComponent<CircleIndicators> ().trackDirectionIndicator;
				item.camPos = windCircle.GetComponent<CircleIndicators> ().trackDirectionTickArrow;
				item.camOrient = player.GetComponentInChildren<Follow_track> ().gameObject;
			}
			if (item.name == "Track Mark tutorial") {
				item.tweeningObj1 = player.GetComponentInChildren<Follow_track> ().currentMark;
				item.tweeningObj2 = windCircle.GetComponent<CircleIndicators> ().trackDirectionIndicator;
				item.camPos = player.GetComponentInChildren<Follow_track> ().currentMark;
				item.camOrient = player.GetComponentInChildren<Follow_track> ().currentMark;
			}
		}
	}

	public void disableTutorial()
	{
		//Time.timeScale = 1.0f;
		//Time.fixedDeltaTime = 0.01f * Time.timeScale;
		timerToTutorialFlag = false;
		string tutoToRun = "";
		foreach (tutorialItem tuto in tutorialList) {
			if (tuto.itemObject.activeSelf == true) {
				tuto.itemObject.SetActive (false);
				if (tuto.nextTutorial != "") // check if an other tutorial needs to be run
				{
					tutoToRun = tuto.nextTutorial;
				}
			}
			else
			{
				tuto.itemObject.SetActive(false);
			}
		}
		Time.timeScale = 1.0f;
		if (tutoToRun != "") {
			enableTutorial (tutoToRun);
		}
		//Time.fixedDeltaTime = 0.01f * Time.timeScale;
	}
	
	public void enableTutorial(string name)
	{
		
		Debug.Log ("Enabled Tutorial : " + name);
		foreach (tutorialItem tuto in tutorialList) {
			if (tuto.name == name) {
				if (tuto.isEnabled) {
				Debug.Log ("enabling tutorial");
				tutorialObjectAssignments ();
				StartCoroutine (timeBeforeShowingTutorial(tuto));
				currentTutorialItem = tuto;
				timerToTutorialFlag = true;
				currentRealTime = 0;
				
				//tuto.itemObject.SetActive (true);
				}
			}
			else
			{
				tuto.itemObject.SetActive(false);
			}
		}
	}

	public void updateTutorialCam(tutorialItem t)
	{
		//t.itemObject.GetComponentInChildren<CinemachineVirtualCamera>().
	}

	IEnumerator timeBeforeShowingTutorial(tutorialItem t)
	{
		yield return new WaitForSeconds (t.timer);
		t.itemObject.SetActive (true);
		t.itemObject.GetComponent<TutorialObjectScript>().pulsingObject1 = t.tweeningObj1;
		t.itemObject.GetComponent<TutorialObjectScript>().pulsingObject2 = t.tweeningObj2;
		t.itemObject.GetComponentInChildren<tutorialTweening> ().pulseIntensity1 = t.tweenIntensity1;
		t.itemObject.GetComponentInChildren<tutorialTweening> ().pulseIntensity2 = t.tweenIntensity2;
		if (t.camPos != null) {
		if (t.itemObject.GetComponentInChildren<CinemachineVirtualCamera> ().m_LookAt == null) {
			Debug.Log ("found virtual camera attached on object : " + t.itemObject.GetComponentInChildren<CinemachineVirtualCamera> ().gameObject.name);
			t.itemObject.GetComponentInChildren<CinemachineVirtualCamera> ().m_LookAt = t.camPos.transform;
			}
		}
		if (t.camOrient != null) {
			if (t.itemObject.GetComponentInChildren<CinemachineVirtualCamera> ().m_Follow == null) {
				t.itemObject.GetComponentInChildren<CinemachineVirtualCamera> ().m_Follow = t.camOrient.transform;
			}
		}

		Time.timeScale = 0.0f;
		t.itemObject.SetActive (false);
		t.itemObject.SetActive (true);
		//Time.fixedDeltaTime = 0.01f * Time.timeScale;
		Debug.Log ("enabled tutorial");
	}

	// Update is called once per frame
	/*void Update () {
		if (timerToTutorialFlag) {
			currentRealTime = currentRealTime + Time.realtimeSinceStartup;
			if (currentRealTime > currentTutorialItem.timer) {
				currentTutorialItem.obj.SetActive (true);

				currentRealTime = 0f;
			}
		}
	}*/
}

[System.Serializable]
public class tutorialItem
{
	public string name;
	public bool isEnabled;
	public GameObject trigger ;
	public GameObject obj;
	public int timer ;
	public GameObject itemObject;
	public GameObject tweeningObj1;
	public GameObject tweeningObj2;
	public float tweenIntensity1;
	public float tweenIntensity2;
	public GameObject camPos;
	public GameObject camOrient;
	public string nextTutorial;

	public tutorialItem (string n, GameObject item, GameObject t, GameObject o, int time)
	{
		name = n;
		itemObject = item;
		trigger = t;
		obj = o;
		timer = time;
	}
}