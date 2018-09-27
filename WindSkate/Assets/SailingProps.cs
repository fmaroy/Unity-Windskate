using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailingProps : MonoBehaviour {

    
    //Weather props
    public float WindAngle = 0f;
    public float upwindAngle = 45f;
    public float downwindAngle = 135f;
    public List<float> upwindBoundaryAngles = new List<float>();
    public List<float> downwindBoundaryAngles = new List<float>();

    //Player props
    public float playerAngle;
    public float playerSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
