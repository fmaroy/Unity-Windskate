using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Setup_track : MonoBehaviour {

    /*public float leftSideBoundaryMax = -90.0f;
    public float rightSideBoundaryMax = 90.0f;*/
    public Vector3 BoundaryMin = new Vector3(-90.0f, 0.0f, -250.0f);
    public Vector3 BoundaryMax = new Vector3(90.0f, 0.0f, 250.0f);
    
    public bool cyclicTrack = false;

    /* public List<GameObject> markList;
	public static List<int> trackList = new List<int>();
	*/
    public List<string> markSequence = new List<string>()
    {
        "1st_Mark", "Downwind", "Upwind","Downwind"
    };

    // Use this for initialization
    void Start () 
	{
		//Debug.Log(markSequence[1]);
	}
	

	// Update is called once per frame
	void Update ()
    {
	

	}
}
