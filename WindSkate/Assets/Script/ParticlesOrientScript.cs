using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesOrientScript : MonoBehaviour {
    public GameObject RefObject;
    
    public Vector3 ShiftOrient;
    public Vector3 ShiftPos;
	// Use this for initialization
	void Start () {
        }
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = ShiftOrient + RefObject.GetComponent<Transform>().eulerAngles;
        //transform.localPosition = new Vector3 (RefObject.GetComponent<Transform>().position.xShiftPos.x + RefObject.GetComponent<Transform>().localPosition;
    }
}
