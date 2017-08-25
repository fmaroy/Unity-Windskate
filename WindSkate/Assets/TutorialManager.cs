using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public List<tutorialItem> tutorialList = new List<tutorialItem>();
	public bool timerToTutorialFlag;
	public tutorialItem currentTutorialItem;
	public float currentRealTime;

	// Use this for initialization
	void Start () {
		enableTutorial ("");

	}

	public void disableTutorial()
	{
		//Time.timeScale = 1.0f;
		//Time.fixedDeltaTime = 0.01f * Time.timeScale;
		timerToTutorialFlag = false;
		foreach (tutorialItem tuto in tutorialList) {
			if (tuto.itemObject.name == name) {
				tuto.itemObject.SetActive (true);
			}
			else
			{
				tuto.itemObject.SetActive(false);
			}
		}
	}

	public void enableTutorial(string name)
	{
		Debug.Log ("Enabled Tutorial : " + name);
		foreach (tutorialItem tuto in tutorialList) {
			if (tuto.name == name) {
				Debug.Log ("enabling tutorial");
				StartCoroutine (timeBeforeShowinfTutorial(tuto.itemObject, tuto.timer));
				currentTutorialItem = tuto;
				timerToTutorialFlag = true;
				currentRealTime = 0;
				//tuto.itemObject.SetActive (true);
			}
			else
			{
				tuto.itemObject.SetActive(false);
			}
		}
	}

	IEnumerator timeBeforeShowinfTutorial(GameObject obj, float t)
	{
		yield return new WaitForSeconds (t);
		obj.SetActive (true);
		Time.timeScale = 0.0f;
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
	public GameObject trigger ;
	public GameObject obj;
	public int timer ;
	public GameObject itemObject;

	public tutorialItem (string n, GameObject item, GameObject t, GameObject o, int time)
	{
		name = n;
		itemObject = item;
		trigger = t;
		obj = o;
		timer = time;
	}
}