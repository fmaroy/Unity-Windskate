using UnityEngine;
using System.Collections;

public class PositionShadowPlane : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(0.0f, this.gameObject.transform.parent.gameObject.transform.eulerAngles.y-90, 0.0f);

    }
}
