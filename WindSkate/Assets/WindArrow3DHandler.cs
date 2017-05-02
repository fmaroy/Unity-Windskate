using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArrow3DHandler : MonoBehaviour {

	public float windAngle;
	public CircleIndicators CircleData;

	// Use this for initialization
	void Start () {
		CircleData = transform.parent.gameObject.GetComponent<CircleIndicators> ();
		windAngle = CircleData.windAngle;
	}
	
	// Update is called once per frame
	void Update () {
		CircleData.placeOnCircle (this.gameObject, 8f, windAngle +90, 0);
	}
	void LateUpdate(){
		
	}
}
