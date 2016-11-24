using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class windEffector : MonoBehaviour {

    public GameObject raceManagerObject;
	public float targetLocalWindForce ;
	public float targetLocalWindDirection ; 
	public float localWindForce ; 
	public float localWindDirection ;
    public Vector3 localWindDirectionVector ; 
	private float deltaLocalWindForce;
	private float deltaLocalWindDirection;

	private float timeToValue = 2.0f;
	private float timer ;

	private GameObject WindGusts; 
	public WindGustsBehavior WindGustsBehavior;

    public GameObject terrainWindEffects; 
    
    private float TurbulentWindModifier;
    private float previousTurbulentWindModifier;
    public float currentTurbulentWindModifier;
    private float TurbulentWindModifierTimer = 0.0f;
    public float TurbulentWindModifierTimieToChange = 0.5f;

    // this is used by Sail_System_Control, UIWindIndicator, UI_True_Wind_Direction, UI_Apparent_Wind
    public float effectiveLocalWindForce;

    //public bool isPlayer = false;

    private bool SelfIntersection = true;

    //public GameObject UIArrows;

    public List<GameObject> WindModifierObjectListDisplay = new List<GameObject>();

    public GameObject WindIdicatorObject;
    

    // Use this for initialization
    void Start()
    {
        WindGusts = GameObject.Find("WindGusts");
        WindGustsBehavior = WindGusts.GetComponent<WindGustsBehavior>();
        raceManagerObject = GameObject.Find("RaceManager");

        terrainWindEffects = raceManagerObject.GetComponent<RaceManagerScript>().thisLevelTerrain;

        localWindForce = WindGustsBehavior.initWindForce;
        localWindDirection = WindGustsBehavior.initWindOrientation;
        localWindDirectionVector = new Vector3(-1 * Mathf.Cos(localWindDirection * Mathf.Deg2Rad), 0.0f, Mathf.Sin(localWindDirection * Mathf.Deg2Rad));
        targetLocalWindForce = localWindForce;
        targetLocalWindDirection = localWindDirection;
        timer = timeToValue;
        effectiveLocalWindForce = localWindForce;
        TurbulentWindModifier = 0.0f;
        previousTurbulentWindModifier = 0.0f;
        currentTurbulentWindModifier = 0.0f;
        WindIdicatorObject = GameObject.Find("WindIndicator");
    }

    
    public List<GameObject> getSubChildren(GameObject currentObject)
    {
        List<GameObject> currentObj = new List<GameObject>();
        foreach (Transform child in currentObject.transform)
        {
            currentObj.Add(child.gameObject);
        }
        return currentObj;
    }


    // Update is called once per frame
    void Update()
    {
        if (TurbulentWindModifier != previousTurbulentWindModifier)
        {
            TurbulentWindModifierTimer = 0.0f;
            //timer = 0.0f;
        }
        if (timer < timeToValue)
        {
            timer = timer + Time.deltaTime;
            localWindForce = localWindForce + (deltaLocalWindForce * Time.deltaTime / timeToValue);
            localWindDirection = localWindDirection + (deltaLocalWindDirection * Time.deltaTime / timeToValue);
            localWindDirectionVector = new Vector3(-1 * Mathf.Cos(localWindDirection * Mathf.Deg2Rad), 0.0f, Mathf.Sin(localWindDirection * Mathf.Deg2Rad));
            //UIArrows.GetComponent< UIWindIndicator>().updateLocalWindArrow(localWindDirection, localWindForce);

        }
        TurbulentWindModifierTimer = TurbulentWindModifierTimer + Time.deltaTime;

        if (TurbulentWindModifierTimer < TurbulentWindModifierTimieToChange)
        {
            currentTurbulentWindModifier = TurbulentWindModifierTimer * TurbulentWindModifier / TurbulentWindModifierTimieToChange;
        }
        effectiveLocalWindForce = localWindForce - currentTurbulentWindModifier;
        previousTurbulentWindModifier = TurbulentWindModifier;
    }
    
    public GameObject recursiveParentSearch(GameObject searchObject)
    {
        GameObject parentObject = searchObject;
        if (searchObject.transform.parent != null)
        {
            parentObject = searchObject.transform.parent.gameObject;
            //Debug.Log("ParentObject: " + parentObject.name);
            return parentObject;
        }
        else
        {
            return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject rootObject = other.gameObject;
        SelfIntersection = true;

        if (other.gameObject.CompareTag("WindGust"))
        {
            //Debug.Log("Wind Received");
            currentGustProperties receivedGust = other.gameObject.GetComponent<currentGustProperties>();

            targetLocalWindForce = receivedGust.thisGustForce;
            targetLocalWindDirection = receivedGust.thisGustOrientation;

            deltaLocalWindForce = targetLocalWindForce - localWindForce;
            deltaLocalWindDirection = targetLocalWindDirection - localWindDirection;
            timer = 0.0f;
            if (this.gameObject.transform.parent.gameObject.GetComponent<PlayerCollision>().isPlayer == true)
            {
                WindIdicatorObject.GetComponent<UIWindIndicator>().WindGustExitHandler(other.gameObject);
            }  
        }

        if (other.gameObject.CompareTag("WindEffects"))
        {
           //Debug.Log("WindEffects");
            while (rootObject != null)
            {
                if (rootObject.GetComponent<PlayerCollision>() != null)
                {
                    //Debug.Log("Root Player found");
                    if (rootObject == this.gameObject.transform.parent.gameObject)
                    {
                        SelfIntersection = true;
                        //Debug.Log("Self intersection on trigger");
                        break;
                    }
                    else
                    {
                        SelfIntersection = false;
                        break;
                    }
                }

                //Debug.Log("Looking for Parent of :" + rootObject.name);

                rootObject = recursiveParentSearch(rootObject);

                if (rootObject.CompareTag("Tree") == true)
                {
                    //Debug.Log("Terrain Wind Effects");
                    SelfIntersection = false;
                    break;
                }
            }


            if ((other.gameObject.name.Contains("Light Effect")) && (SelfIntersection == false))
            {
                TurbulentWindModifier = 10.0f;
                TurbulentWindModifierTimer = 0.0f;
            }
            if ((other.gameObject.name.Contains("Heavy Effect")) && (SelfIntersection == false))
            {
                TurbulentWindModifier = 20.0f;
                TurbulentWindModifierTimer = 0.0f;
            }
            if ((other.gameObject.name.Contains("Positive Effect")) && (SelfIntersection == false))
            {
                TurbulentWindModifier = -10.0f;
                TurbulentWindModifierTimer = 0.0f;
            }
            if ((SelfIntersection == false) & (this.gameObject.transform.parent.GetComponent<PlayerCollision>().isPlayer == true))
            {
                //Debug.Log("Cover, but no MeshRenderer : " + other.gameObject.name);
                
                if (other.gameObject.GetComponent<MeshRenderer>() != null)
                {
                    //Debug.Log("Covered!");
                    other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                    WindModifierObjectListDisplay.Add(other.gameObject);
                }
                else
                {
                    //Debug.Log("no Meshrenderer");
                }
            }
        }
    }
    
    

    void OnTriggerExit(Collider other)
    {
        GameObject rootObject = other.gameObject;
        SelfIntersection = true;

        if (other.gameObject.CompareTag("WindEffects"))
        {
            while (rootObject != null)
            {
                if (rootObject.GetComponent<PlayerCollision>() != null)
                {
                    //Debug.Log("Root Player found");
                    if (rootObject == this.gameObject.transform.parent.gameObject)
                    {
                        SelfIntersection = true;
                        //Debug.Log("Self intersection on trigger");
                        break;
                    }
                    else
                    {
                        SelfIntersection = false;
                        break;
                    }

                }

               //Debug.Log("Looking for Parent of :" + rootObject.name);
                rootObject = recursiveParentSearch(rootObject);

                if (rootObject.CompareTag("Tree") == true)
                {
                    //Debug.Log("Leaving Terrain Wind Effects");
                    SelfIntersection = false;
                    break;
                }
            }
            if ((other.gameObject.name.Contains("Heavy Effect")) && (SelfIntersection == false))
            {
                
                TurbulentWindModifier = 0.0f;
            }

            if ((other.gameObject.name.Contains("Light Effect")) && (SelfIntersection == false))
            {
                //Debug.Log("LEaving Light Effect Area");
                TurbulentWindModifier = 0.0f;
            }
            if ((other.gameObject.name.Contains("Positive Effect")) && (SelfIntersection == false))
            {
               
                TurbulentWindModifier = 0.0f;
            }
            if ((SelfIntersection == false)&(this.gameObject.transform.parent.GetComponent<PlayerCollision>().isPlayer == true))
            {
                if (other.gameObject.GetComponent<MeshRenderer>() != null)
                {
                    other.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    WindModifierObjectListDisplay.Remove(other.gameObject);
                }
            }
        }
    }
}
