using UnityEngine;
using System.Collections;

public class currentGustProperties : MonoBehaviour {
	public float thisGustForce;
	public float thisGustOrientation;
	public bool thisGustenabled = false;
	public float thisGustVelocity = 60.0f ; 
	private float initTime;
	private Vector3 initPosition = new Vector3 (0.0f, 0.0f, 0.0f);
	private float time; 

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (thisGustenabled) 
		{
			time = Time.time - initTime;
			transform.localPosition = new Vector3 (0.0f, 0.0f, thisGustVelocity * time);
		}

	}

	public void startGust ()
	{
		initTime = Time.time;
		thisGustenabled = true;
		transform.localPosition = initPosition;
        gameObject.SetActive(true);


    }
	public void stopGust ()
	{
		thisGustenabled = false;
		transform.localPosition = initPosition;
        gameObject.SetActive(false);
    }


}
