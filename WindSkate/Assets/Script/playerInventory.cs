using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerInventory : MonoBehaviour {
    //public GameObject RaceDataObject;
    //private UserPreferenceScript RaceData;
    public List<GameObject> sailsList = new List<GameObject>();
    public int currentSail = 0;
    public List<GameObject> boardList = new List<GameObject>();
    public List<AxleList> boardAxleList = new List<AxleList>();
    
    public int currentBoard = 0;
    public List<GameObject> characterList = new List<GameObject>();


    // Use this for initialization
    void Start () {
        /*RaceData = RaceDataObject.GetComponent<UserPreferenceScript>();
        setPlayerProps(RaceData);*/

    }

    /*void setPlayerProps(UserPreferenceScript racedata)
    {
        int i = 0;
        foreach (GameObject sail in sailsList)
        {
            Debug.Log(racedata.PersistentParameterData.PlayerConfig.sail);
            if (i == racedata.PersistentParameterData.PlayerConfig.sail)
            {
                sail.SetActive(true);
            }
            else
            {
                sail.SetActive(false);
            }
            i++;
        }
        
    }*/
	// Update is called once per frame
	void Update () {
	
	}
}
[System.Serializable]
public class AxleList
{
    public string name;
    public List<GameObject> axleList;

    public AxleList(string n, List<GameObject> g)
    {
        name = n;
        axleList = g;
    }
}
