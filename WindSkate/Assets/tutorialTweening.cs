using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialTweening : MonoBehaviour {
	public float timer = 1f;
	public GameObject pulseobj1 ;
	public GameObject pulseobj2 ;
	public float pulseIntensity1 = 0.3f;
	public float pulseIntensity2 = 0.3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pulseobj1 = GetComponentInParent<TutorialObjectScript> ().pulsingObject1;
		pulseobj2 = GetComponentInParent<TutorialObjectScript> ().pulsingObject2;
		if (pulseobj1 != null) {
			System.Collections.Hashtable hash1 = new System.Collections.Hashtable();
			hash1.Add("amount", new Vector3(pulseIntensity1, pulseIntensity1, pulseIntensity1));
			hash1.Add("time", timer);
			hash1.Add ("ignoretimescale", true);
			iTween.PunchScale (pulseobj1, hash1);
		}
		if (pulseobj2 != null) {
			System.Collections.Hashtable hash2 = new System.Collections.Hashtable ();
			hash2.Add ("amount", new Vector3 (pulseIntensity2, pulseIntensity2, pulseIntensity2));
			hash2.Add ("time", timer);
			hash2.Add ("ignoretimescale", true);
			iTween.PunchScale (pulseobj2, hash2);
		}

	}
}
