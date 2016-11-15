using UnityEngine;
using System.Collections;

public class NavAgent_script : MonoBehaviour {

    public GameObject ParentObject;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        transform.position = ParentObject.transform.position;
    }
}
