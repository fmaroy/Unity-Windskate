using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindGustsBehavior : MonoBehaviour {
	public List<GameObject> WindGustsObjectsList ;
	private float timer;

	//Initial setup wind condition
	public float initWindForce = 30.0f;
	public float initWindOrientation = 0.0f;
	// variable wind condition;
	public float currentWindForce ;
	public float currentWindOrientation ;
	public Vector3 currentWindOrientationVector;


	private float windForceIntensityDirection;

	public float windIntensityThreshold = 0.5f;
	public float windChangeIntensityFactor;
	public float windUpdateTime;

	//Variation of wind direction
	public float windDirectionThreshold = 0.5f;
	public float windDirectionRotation;
	public float windChangeDirectionFactor = 1;
	public float coriolisShiftFactor = 1.0f;

	private bool windIntensityChangedFlag = false; 
	private bool windDirectionChangedFlag = false; 
	private bool windGlobalChangedFlag = false;

	//Handling Gusts
	private int currentGustID = 0;

	//trend to target
	public float targetWindForce = 40f; 
	public float targetWindDirection = 110f;
	public float timeToWindTarget = 0.5f; 
	private float timer_2 = 0.0f;

    public GameObject WindIndicatorObject;
    public UIWindIndicator WindIndicatorData;

    // Use this for initialization
    void Start()
    {
        // Initial registration of Wind gusts
        foreach (Transform child in transform)
        {
            WindGustsObjectsList.Add(child.gameObject);

        }

        currentWindForce = initWindForce;
        currentWindOrientation = initWindOrientation;
        currentWindOrientationVector = new Vector3(Mathf.Cos(currentWindOrientation * Mathf.Deg2Rad), 0.0f, Mathf.Sin(currentWindOrientation * Mathf.Deg2Rad));

        timer = 0.0f;
        timer_2 = 0.0f;
        currentGustID = 0;
        if (GameObject.Find("WindIndicator") != null)
        {
            WindIndicatorObject = GameObject.Find("WindIndicator");
            WindIndicatorData = WindIndicatorObject.GetComponent<UIWindIndicator>();
        }
    }

    // Update is called once per frame
    void Update () 
	{
        //buildGustList(GameObject.Find("Player").GetComponentInChildren<Follow_track>().gameObject.transform.position.x);
        

		timer = timer + Time.deltaTime;
		timer_2 = timer_2 + Time.deltaTime;
		//thresholdChangeIntensity varies from 0 to 1
		float timer_2_dimless = timer_2 / timeToWindTarget;
		if (timer_2_dimless > 1.0f) 
		{
			timer_2_dimless = 1.0f; 
		}

		float thresholdChangeIntensitySide = 0.5f + ((1 - (currentWindForce / targetWindForce))*timer_2_dimless); 
		float thresholdChangeDirectionSide = 0.5f +  ((1 - (currentWindOrientation / targetWindDirection))*timer_2_dimless); 
		// Varies fr
		// 30/40 = 0.75, 30/20 = 1.33 fois time thresholdchnage time  qui change de 0 a 1
		//Debug.Log ("timer dimless " + thresholdChangeTime); 
		//Debug.Log ("Timer_2 " + timer_2_dimless);
		//Debug.Log ("Threshold Change Direction Side " + thresholdChangeDirectionSide); 

		if (timer > windUpdateTime)
		{
			timer = 0.0f;
			if (Random.value > windIntensityThreshold) 
			{
				//Debug.Log ("Wind Intensity Changed!");
				windIntensityChangedFlag = true;
                

                if (Random.value < thresholdChangeIntensitySide) 
				{
					windForceIntensityDirection = Random.value;
				}
				else 
				{
					windForceIntensityDirection = -1 * Random.value;
				}

				currentWindForce = windChangeIntensityFactor * windForceIntensityDirection + currentWindForce;
				currentWindOrientation = currentWindOrientation + coriolisShiftFactor * windForceIntensityDirection;
			}
			else
			{
				//print("Wind Intensity not changed");	
			}
			
			if (Random.value > windDirectionThreshold) 
			{
				//Debug.Log ("Wind direction Changed!");
				windDirectionChangedFlag = true; 

				if (Random.value < thresholdChangeDirectionSide) 
				{
					windDirectionRotation = Random.value;
					//Debug.Log ("Right Shift");
				}
				else 
				{
					windDirectionRotation = -1 * Random.value;
					//Debug.Log ("Left Shift");
				}

				currentWindOrientation = windChangeDirectionFactor * windDirectionRotation + currentWindOrientation;
			} 
			else
			{
				//print ("WindDirection not changed");	
			}
		

			if (currentGustID >= WindGustsObjectsList.Count) 
			{
				currentGustID = 0;
			}
			windGlobalChangedFlag = false; 

			if (windIntensityChangedFlag == true)
			{
				windGlobalChangedFlag = true; 
			}
			if (windDirectionChangedFlag == true)
			{
				windGlobalChangedFlag = true; 
			}
				
			if (windGlobalChangedFlag == true)
			{
                //This changes the orientation of the trigger line, disbaled to chnage to the icon system
                //WindGustsObjectsList[currentGustID].transform.eulerAngles = new Vector3 (0.0f, currentWindOrientation+90, 0.0f) ;

				currentGustProperties currentGustProperties = WindGustsObjectsList[currentGustID].GetComponent<currentGustProperties>();

				currentGustProperties.startGust();
                currentGustProperties.thisGustForce = currentWindForce; 
				currentGustProperties.thisGustOrientation = currentWindOrientation;
                if (WindIndicatorObject != null)
                {
                    WindIndicatorData.WindGustReleasedHandler(WindGustsObjectsList[currentGustID]);
                }
                currentGustID++; 
			}


			windDirectionChangedFlag = false; 
			windIntensityChangedFlag = false; 
			windGlobalChangedFlag = false; 
		}	

	}
}
