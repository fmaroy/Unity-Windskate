using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	List<GameObject> tutorialList = new List<GameObject>();
	// Use this for initialization
	void Start () {
		enableTutorial ("");
	}



	public void disableTutorial()
	{
		Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.01f * Time.timeScale;

		foreach (GameObject tuto in tutorialList) {
			if (tuto.name == name) {
				tuto.SetActive (true);
			}
			else
			{
				tuto.SetActive(false);
			}
		}
	}

	public void enableTutorial(string name)
	{
		foreach (GameObject tuto in tutorialList) {
			if (tuto.name == name) {
				tuto.SetActive (true);
			}
			else
			{
				tuto.SetActive(false);
			}
		}
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = 0.01f * Time.timeScale;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
