using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdaptativeCanvenas : MonoBehaviour {

	public string deviceType;
	public RectTransform thisRect;

	// Use this for initialization
	void Start () {
		deviceType = SystemInfo.deviceModel;
		thisRect = this.GetComponent<RectTransform> ();
		Debug.Log ("AnchorMin: ");
		Debug.Log (thisRect.anchoredPosition3D);
		Debug.Log (thisRect.anchorMax);
		Debug.Log (deviceType);
		if (deviceType == "iPhone10,6") {
			thisRect.offsetMin = new Vector2 (50f, 0f);
			thisRect.offsetMax = new Vector2 (0f, 0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
