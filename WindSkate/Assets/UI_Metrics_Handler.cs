using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Metrics_Handler : MonoBehaviour {

	public float speed;
	public float angle;
	public GameObject textObj; 
	private string[] angleStringColors = { "white","orange","red"};

	// Use this for initialization
	void Start () {
		//textObj = this.transform.GetComponentInChildren<TextMeshProUGUI> ();
		speed = 0f;
		angle = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		//string text = "Speed : "+ String.Format({0:0.0}, speed) + " Angle : " + String.Format({0:0.0}, angle) + "°";
		int anglecolorID = 0;
		if ((Mathf.Abs(angle) < 40) || (Mathf.Abs(angle) > 140)){anglecolorID = 1;}
		if ((Mathf.Abs(angle) < 20) || (Mathf.Abs(angle) > 160)){anglecolorID = 2;}

		string text = "Speed         : "+ speed.ToString("###") + " km/h\nWind Angle : <color=\"" +angleStringColors[anglecolorID]+"\">" + Mathf.Abs(angle).ToString("###") + "</color>°";
		textObj.GetComponent<TextMeshProUGUI> ().SetText(text);
	}
}
