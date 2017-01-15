using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RaceSelector : MonoBehaviour {

    private GameObject SceneManagerObject;
    private PersistentParameters PersistentParameterData;
    private GameObject RaceManagerObject;
    private UserPreferenceScript RaceManagerData;

    public int currentObjId = 0;
    public List<TrackList> currentTrackList = new List<TrackList>();
    public GameObject trackNameObject;
    public GameObject RaceImageObject;

    public int numberOpponents = 3;
    public int opponentLevel = 3;
    public int windType = 0;
    public GameObject SliderObjectNumberOpponents;
    public GameObject SliderObjectOpponentsLevel;
    public GameObject SliderWindType;

    // Use this for initialization
    void Start () {
        SceneManagerObject = GameObject.Find("Scene_Manager");
        PersistentParameterData = SceneManagerObject.GetComponent<PersistentParameters>();
        RaceManagerObject = GameObject.Find("RaceManager");
        RaceManagerData = RaceManagerObject.GetComponent<UserPreferenceScript>();
        foreach (TrackList track in PersistentParameterData.trackList)
        {
            currentTrackList.Add(track);
        }
        //Debug.Log(trackName.GetComponent<Text>().text);
        updateCurrentTrack(0);
        //currentTrackList = PersistentParameterData.trackList;
        getOpponentsLevel();
        getNumberOpponents();
        SliderWindType.GetComponent<Slider>().maxValue = PersistentParameterData.ListOfWinds.Count-1;
        getWindType();
    }

    public void getWindType()
    {
        Slider currentSlider = SliderWindType.GetComponent<Slider>();
        windType = Mathf.FloorToInt(currentSlider.value);
        string currentSliderText;
        currentSliderText = "Wind: \n" + PersistentParameterData.ListOfWinds[windType].name + "\n" + PersistentParameterData.ListOfWinds[windType].label;
        SliderWindType.GetComponentInChildren<Text>().text = currentSliderText;

    }

    public void getOpponentsLevel()
    {
        Slider currentSlider = SliderObjectOpponentsLevel.GetComponent<Slider>();
        string currentSliderText;
        opponentLevel = Mathf.FloorToInt(currentSlider.value);
        currentSliderText = "Level: " + opponentLevel.ToString();
        SliderObjectOpponentsLevel.GetComponentInChildren<Text>().text = currentSliderText;

    }
    public void getNumberOpponents()
    {
        Slider currentSlider = SliderObjectNumberOpponents.GetComponent<Slider>();
        string currentSliderText;
        numberOpponents = Mathf.FloorToInt( currentSlider.value);
        currentSliderText = "Players: " + numberOpponents.ToString();
        SliderObjectNumberOpponents.GetComponentInChildren<Text>().text = currentSliderText;

    }

    public void updateCurrentTrack(int trackid)
    {
        trackNameObject.GetComponent<Text>().text = currentTrackList[trackid].trackName;
        RaceImageObject.GetComponent<RawImage>().texture = currentTrackList[trackid].RacePreview;
        currentObjId = trackid;
    }
    public void nextTrack()
    {
        currentObjId = currentObjId + 1;
        currentObjId = Mathf.Clamp(currentObjId, 0, currentTrackList.Count - 1);
        updateCurrentTrack(currentObjId);
    }
    public void prevTrack()
    {
        currentObjId = currentObjId - 1;
        currentObjId = Mathf.Clamp(currentObjId, 0, currentTrackList.Count - 1);
        updateCurrentTrack(currentObjId);
    }
    public void loadTrack()
    {
        List <int> tempList = new List<int>();
        for (int i = 0; i < SceneManagerObject.GetComponent<PersistentParameters>().OpponentConfigList.Capacity; i++)
        {
            tempList.Add(i);
            
        }
        // Next randomizes "tempList"
        for (int i = 0; i < tempList.Count; i++)
        {
            int temp = tempList[i];
            int randomIndex = Random.Range(i, tempList.Count);
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;
        }
        SceneManagerObject.GetComponent<PersistentParameters>().currentRaceOpponentsListIds = tempList;
        SceneManagerObject.GetComponent<PersistentParameters>().currentSingleRaceDefinition = new SingleRaceProps(numberOpponents, opponentLevel, windType);
        SceneManagerObject.GetComponent<SceneManagerScript>().LoadScene(currentTrackList[currentObjId].sceneName);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
