using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mark : MonoBehaviour {
	public Vector3 firstPassValidation;
	public Vector3 finalPassValidation;
    public bool singleMark;
    public bool isWaypoint = false;
    public bool PreviousBoundingBoxOverwrite = false;
    public Vector3 BoundingBoxMin;
    public Vector3 BoundingBoxMax;
    public GameObject ResetPositionParent;
    public List<GameObject> ResetPositionsList;

    public List<GameObject> Children;

	// Use this for initialization
	void Start () 
	{	
		if (singleMark == false)
		{
			foreach (Transform child in transform)
            { 
				Children.Add(child.gameObject);
			}
		}
        ResetPositionsList = new List<GameObject>();

        foreach (Transform position in ResetPositionParent.transform)
        {
            ResetPositionsList.Add(position.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
