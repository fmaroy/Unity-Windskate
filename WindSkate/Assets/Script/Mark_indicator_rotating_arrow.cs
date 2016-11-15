using UnityEngine;
using System.Collections;

public class Mark_indicator_rotating_arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, 0, 1) * Time.deltaTime*150);
	}
}
