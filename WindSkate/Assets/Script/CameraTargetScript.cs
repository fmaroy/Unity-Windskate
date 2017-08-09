using UnityEngine;
using System.Collections;

public class CameraTargetScript : MonoBehaviour {

    public GameObject referenceTransformObjectDirection;
    public Vector3 offsetPosition;
    public Vector3 offsetOrientation;
    public GameObject referenceTransformObjectPosition;
    

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
        transform.position = referenceTransformObjectPosition.transform.position + offsetPosition ;
        transform.eulerAngles = new Vector3(0.0f, referenceTransformObjectDirection.transform.eulerAngles.y, 0.0f)+ offsetOrientation;

    }
}
