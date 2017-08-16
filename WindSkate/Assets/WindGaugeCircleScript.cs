using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGaugeCircleScript : MonoBehaviour {

	public GameObject windSource;
	public windEffector windData;
	public float currentWindAngle;
	LineRenderer line;
	Gradient lineGradient;
	float[] linePos ;
	public float lineAngularLength = 90f;
	public float lineAngularResolution = 1f;
	public GameObject player;
	public float heightPosition = 0.0f;
	public float radius = 60f;
	public int gradientResolution = 4;

	//public Color[] WindAnglesColors = new Color[3];
	//public List<WindAnglesClass> listOfWindAngles = new List<WindAnglesClass> ();



	// Use this for initialization
	void Start () {
		windData = windSource.GetComponent<windEffector>();
		line = this.GetComponent<LineRenderer> ();
		lineGradient = line.colorGradient;
		currentWindAngle = player.GetComponent<Follow_track>().angleBoardToWind;
		//linePos = new float[Mathf.RoundToInt(lineAngularLength / lineAngularResolution)];
	}




	LineRenderer linePosDefinition(LineRenderer l, float centerAngle, float length, int segmentCount, float rad, float height)
	{
		float currentMinAngle = centerAngle - length / 2;
		float currentMaxAngle = centerAngle + length / 2;
		//Debug.Log ("min : " + currentMinAngle);
		//Debug.Log ("max : " + currentMaxAngle);

		for (int i = 0; i < segmentCount + 1; i ++)
		{
			float lerpValue = (float)i /(float)segmentCount;
			//Debug.Log ("i : " + i + ", Lerp : " + lerpValue);
			float currentAngle = Mathf.LerpAngle (currentMinAngle, currentMaxAngle, lerpValue);
			//Debug.Log (currentAngle);
			float x = Mathf.Sin (Mathf.Deg2Rad * currentAngle) * rad;
			float y = Mathf.Cos (Mathf.Deg2Rad * currentAngle) * rad;
			l.SetPosition (i, new Vector3 (x, y, height));
		}
		return l;
	}

	GradientColorKey[] lineGradientDefinition (float centerAngle, float length, int resolution)
	{
		
		List<GradientColorKey> keyConstruction = new List<GradientColorKey> ();
		float currentMinAngle = centerAngle - length / 2;
		float currentMaxAngle = centerAngle + length / 2;
		int coursetype = transform.parent.GetComponent<CircleIndicators> ().typeOfCourse;

		GradientColorKey currentKey;
		currentKey.color = transform.parent.GetComponent<CircleIndicators> ().getColorForWindAngle (coursetype, currentMinAngle);
		currentKey.time = 0;
		keyConstruction.Add (currentKey);

		currentKey.color = transform.parent.GetComponent<CircleIndicators> ().getColorForWindAngle (coursetype, currentMinAngle);
		currentKey.time = 1;
		keyConstruction.Add (currentKey);

		foreach (float angle in transform.parent.GetComponent<CircleIndicators>().listOfWindAngles[coursetype].bestAngles) {
			if (Mathf.Abs (Mathf.DeltaAngle (centerAngle, angle)) < length / 2) {
				currentKey.color = transform.parent.GetComponent<CircleIndicators> ().getColorForWindAngle (coursetype, angle);
				currentKey.time = Mathf.InverseLerp (currentMinAngle, currentMaxAngle, angle);
				keyConstruction.Add (currentKey);
			}
		}
		foreach (float angle in transform.parent.GetComponent<CircleIndicators>().listOfWindAngles[coursetype].worstAngles) {
			if (Mathf.Abs (Mathf.DeltaAngle (centerAngle, angle)) < length / 2) 
			{
				currentKey.color = transform.parent.GetComponent<CircleIndicators> ().getColorForWindAngle (coursetype, angle);
				currentKey.time = Mathf.InverseLerp(currentMinAngle, currentMaxAngle, angle);
				keyConstruction.Add (currentKey);
			}
		}

		GradientColorKey[] currentColorGradient = new GradientColorKey [keyConstruction.Count];
		int i = 0;
		foreach (GradientColorKey key in keyConstruction) {
			currentColorGradient [i] = key;
			i++;
		}
		return currentColorGradient;
	}

	// Update is called once per frame
	void Update () {
		currentWindAngle = player.GetComponent<Follow_track>().angleBoardToWind;
		int segments = Mathf.RoundToInt (lineAngularLength / lineAngularResolution);
		line.SetVertexCount (segments + 1);
		// following Makes sure each wraps around 360
		//currentMinAngle = Mathf.MoveTowardsAngle(currentWindAngle, currentMinAngle, lineAngularLength /2);


		//currentMaxAngle = Mathf.MoveTowardsAngle(currentWindAngle, currentMaxAngle, lineAngularLength /2);

		//line.numCapVertices = 5;

		line = linePosDefinition (line, 0, lineAngularLength, segments, radius, heightPosition);

		lineGradient.colorKeys = lineGradientDefinition (currentWindAngle, lineAngularLength, gradientResolution);
		line.colorGradient = lineGradient;

		transform.eulerAngles = new Vector3 (90.0f,0.0f,-1*windData.localWindDirection - 90);
	}
}
