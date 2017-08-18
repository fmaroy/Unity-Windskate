using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGaugeScript : MonoBehaviour {
	public GameObject player;
	public float windAngle;
	public float globalWindAngle;
	public GameObject windSource;
	private windEffector windData;
	public float radius = 8f;
	public float sectorLength = 90f;
	public int lineSegments = 30;
	private LineRenderer line;

	// Use this for initialization
	void Start () {
		windSource = player.transform.GetComponentInChildren<windEffector> ().gameObject;
		windData = windSource.GetComponent<windEffector>();
		windAngle = player.GetComponentInChildren<Follow_track>().angleBoardToWind;
		globalWindAngle = windData.localWindDirection;
		//WindToBoard = player.GetComponentInChildren<Follow_track> ().angleBoardToWind;
		line = this.GetComponent<LineRenderer> ();
		drawCircle (windAngle, sectorLength, lineSegments, radius);
	}

	void drawCircle(float angle, float length, int segments, float r)
	{
		float minAngleBound = angle -1 * length / 2;
		float maxAngleBound = angle + length / 2;
		float angleStep = length / (1f*segments);
		line.positionCount = segments;
		for (int i = 0; i < segments; i++) 
		{
			float a = minAngleBound + i * angleStep;
			float x = Mathf.Sin (Mathf.Deg2Rad * a) * r;
			float y = Mathf.Cos (Mathf.Deg2Rad * a) * r;
			line.SetPosition (i,new Vector3(x, y , 0f));
		}
	}

	public Gradient colorCircle(float length, float angle)
	{
		float minAngleBound = angle - (1 * length / 2);
		float maxAngleBound = angle + (1 * length / 2);

		int courseType = transform.parent.GetComponent<CircleIndicators> ().typeOfCourse;

		Gradient lineGradient= line.colorGradient;
		Gradient grad = new Gradient ();
		grad.alphaKeys = lineGradient.alphaKeys;

		List<float> keyGoodAngles = new List<float> ();
		List<float> keyBadAngles = new List<float> ();
		int currentCourse = transform.parent.GetComponent<CircleIndicators> ().typeOfCourse;
		WindAnglesClass currentAnglesData = transform.parent.GetComponent<CircleIndicators>().listOfWindAngles[currentCourse];
		foreach (float bestangle in currentAnglesData.bestAngles) {
			if (Mathf.Abs (Mathf.DeltaAngle (angle, bestangle)) < length / 2) {
				keyGoodAngles.Add (bestangle);
			}
		}
		foreach (float worstangle in currentAnglesData.worstAngles) {
			if (Mathf.Abs (Mathf.DeltaAngle (angle, worstangle)) < length / 2) {
				keyBadAngles.Add (worstangle);
			}
		}
			
		GradientColorKey[] colorKeys = new GradientColorKey[keyBadAngles.Count + keyGoodAngles.Count + 3];

		//GradientColorKey[] colorKeys = new GradientColorKey[3];

		int i = 0;
		foreach (float a in keyGoodAngles) {
			colorKeys.SetValue (convertColorToGradientColorKey(transform.parent.GetComponent<CircleIndicators> ().WindAnglesColors [1], dimlessAngleCalc(a, minAngleBound, maxAngleBound)), i);
			i++;
		}
		foreach (float a in keyBadAngles) {
			colorKeys.SetValue (convertColorToGradientColorKey(transform.parent.GetComponent<CircleIndicators>().WindAnglesColors [0], dimlessAngleCalc(a, minAngleBound, maxAngleBound)), i);
			i++;
		}

		colorKeys.SetValue(convertColorToGradientColorKey( transform.parent.GetComponent<CircleIndicators>().getColorForWindAngle(courseType, minAngleBound), 0), i);
		Debug.Log("minAngle : " + minAngleBound);
		i++;
		colorKeys.SetValue(convertColorToGradientColorKey( transform.parent.GetComponent<CircleIndicators>().getColorForWindAngle(courseType, maxAngleBound), 1), i);
		i++;

		colorKeys.SetValue(convertColorToGradientColorKey( transform.parent.GetComponent<CircleIndicators>().getColorForWindAngle(courseType, angle), 0.5f), i);

		grad.colorKeys = colorKeys;
		return grad;

	}

	float dimlessAngleCalc(float angle, float  minangle, float  maxangle)
	{
		float val = Mathf.InverseLerp (minangle, maxangle, angle);
		return val;
	}


	GradientColorKey convertColorToGradientColorKey(Color c, float pos)
	{
		GradientColorKey key = new GradientColorKey();
		key.color = c;
		key.time = pos;
		return key;
	}


	// Update is called once per frame
	void Update () {
		windAngle = player.GetComponentInChildren<Follow_track>().angleBoardToWind;

		line.colorGradient = colorCircle(sectorLength, windAngle);
		drawCircle (globalWindAngle, sectorLength, lineSegments, radius);

		//this.GetComponent<LineRenderer> () = line;

		//transform.eulerAngles = new Vector3(90, windAngle +90f, 0f);

	}
}
