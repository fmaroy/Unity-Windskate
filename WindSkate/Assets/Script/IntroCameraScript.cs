using UnityEngine;
using System.Collections;

public class IntroCameraScript : MonoBehaviour {

    public GameObject PlayerObject;
    public Vector3 cameraOffset;
    public Vector3 cameraOrient;
    public float cameraDistance;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = PlayerObject.transform.position - cameraOrient * cameraDistance + cameraOffset;

	}
}
