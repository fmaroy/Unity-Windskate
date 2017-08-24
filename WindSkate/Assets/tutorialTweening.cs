using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialTweening : MonoBehaviour {
	public float timer = 1f;
	public GameObject pulseobj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		pulseobj = GetComponentInParent<TutorialObjectScript> ().pulsingObject;
		System.Collections.Hashtable hash = new System.Collections.Hashtable();
		hash.Add("amount", new Vector3(0.15f, 0.15f, 0.15f));
		hash.Add("time", timer);
		iTween.PunchScale(gameObject, hash);
		iTween.PunchScale (pulseobj, hash);
	}
}
