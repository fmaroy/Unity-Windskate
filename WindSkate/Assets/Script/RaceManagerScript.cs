using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RaceManagerScript : MonoBehaviour {
    public GameObject Wind;
    private WindGustsBehavior WindData;
    public int typeOfWindDirection;
    public int typeOfWindForce;
    public float highOscillationRange = 20.0f;
    public float smallOscillationRange = 10.0f;
    public float bigShift = 30.0f;
    public float littleShift = 10.0f;
    private float initial_wind_direction = 30.0f;
    private int right_shift;
    public GameObject thisLevelTerrain;
    public List<GameObject> OpponenentObjectsList = new List<GameObject>();
    public GameObject OpponentContainerObject;
    public GameObject PlayerObject;

    // Use this for initialization
    void Start()
    {
        thisLevelTerrain = GameObject.Find("Track").GetComponentInChildren<Terrain>().gameObject;
        foreach (Transform opponents in OpponentContainerObject.transform)
        {
            OpponenentObjectsList.Add(opponents.gameObject);
            opponents.gameObject.GetComponentInChildren<windEffector>().terrainWindEffects = thisLevelTerrain;
        }
        PlayerObject.GetComponentInChildren<windEffector>().terrainWindEffects = thisLevelTerrain;
        WindData = Wind.GetComponent<WindGustsBehavior>();
        initial_wind_direction = WindData.initWindOrientation;

        if (GameObject.Find("Scene_Manager")==null)
        {
            setWindOlderbehavior();
        }
    }

    public void setWindOlderbehavior()
    {
        //Wind Shift handling
        if (Random.value < 0.3)
        {
            Debug.Log("no Shift planned");
            //no Shift planned
            WindData.targetWindDirection = initial_wind_direction;
        }
        else
        {
            if (Random.value < 0.5)
            {
                Debug.Log("wind will turn right");
                //wind will turn right
                right_shift = 1;
            }
            else
            {
                //wind will turn left
                Debug.Log("wind will turn left");
                right_shift = -1;
            }
            if (Random.value < 0.5)
            {
                //little shift
                Debug.Log("wind will shift a little");
                WindData.targetWindDirection = initial_wind_direction + right_shift * Random.value * littleShift;
            }
            else
            {
                //Big shift
                Debug.Log("wind will shift a lot");
                WindData.targetWindDirection = initial_wind_direction + right_shift * Random.value * bigShift;
            }
        }
        //Wind oscilation handling
        if (Random.value < 0.5)
        {
            Debug.Log("little Oscillatons planned");
            //no Shift planned
            WindData.windChangeDirectionFactor = smallOscillationRange;
        }
        else
        {
            Debug.Log("Big Oscillatons planned");
            //no Shift planned
            WindData.windChangeDirectionFactor = highOscillationRange;
        }
    }

    public void setWindBehavior(WindType Windprops)
    {
        
        Debug.Log("TypeOdWind : " + Windprops.name);
        if (Random.value < 0.5)
        {
            Debug.Log("wind will turn right");
            //wind will turn right
            right_shift = 1;
        }
        else
        {
            //wind will turn left
            Debug.Log("wind will turn left");
            right_shift = -1;
        }
        WindData.targetWindDirection = initial_wind_direction + right_shift * Random.value * Windprops.shiftRange;
        WindData.windChangeDirectionFactor = Windprops.oscillationRange;
        WindData.targetWindForce = Windprops.targetWindForce;
    }
    // Update is called once per frame
    void Update () {
	    ///Is wind Ocilating or Shifting?
        
        
    }
}
