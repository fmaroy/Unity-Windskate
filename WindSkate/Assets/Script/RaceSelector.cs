using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RaceSelector : MonoBehaviour {

	public string typeOfPage = "single"; //can be single or career
    private GameObject SceneManagerObject;
    private PersistentParameters PersistentParameterData;
    private GameObject RaceManagerObject;
    private UserPreferenceScript RaceManagerData;

    public int currentObjId = 0;
	public List<CareerSeasonProps> currentSeasonList = new List<CareerSeasonProps> ();
    public List<TrackList> currentTrackList = new List<TrackList>();
	public List<GameObject> seasonRaceObj = new List<GameObject> ();
    public GameObject trackNameObject;
    public GameObject RaceImageObject;

    public int numberOpponents = 3;
    public int opponentLevel = 3;
    public int windType = 0;
    public GameObject SliderObjectNumberOpponents;
    public GameObject SliderObjectOpponentsLevel;
    public GameObject SliderWindType;

	public GameObject seasonPannel;
	public GameObject trackTemplate;
	public float raceItemMargins = 15f;
	public int numbColumn = 4;

	public int currentSeasonId = 0;
	public int currentSelectedCarreerRaceId = 0;

    // Use this for initialization
    void Start () {
        SceneManagerObject = GameObject.Find("Scene_Manager");
        PersistentParameterData = SceneManagerObject.GetComponent<PersistentParameters>();
        RaceManagerObject = GameObject.Find("RaceManager");
        RaceManagerData = RaceManagerObject.GetComponent<UserPreferenceScript>();

		foreach (TrackList track in PersistentParameterData.trackList) {
			currentTrackList.Add (track);
		}

		if (typeOfPage == "career") {
			foreach (CareerSeasonProps season in PersistentParameterData.seasonList) {
				currentSeasonList.Add (season);
			}
			updateSeason (currentSeasonId);
		}


		if (typeOfPage == "single") {
			updateCurrentTrack (0);
			getOpponentsLevel ();
			getNumberOpponents ();
			SliderWindType.GetComponent<Slider> ().maxValue = PersistentParameterData.ListOfWinds.Count - 1;
			getWindType ();

		}
    }

	public void selectedCarreerRace(int id)
	{
		int i = 0;
		foreach (SingleRaceProps race in currentSeasonList[currentSeasonId].raceList) {
			if (id == i) {
				currentSelectedCarreerRaceId = i;
			} else {
				seasonRaceObj[i].GetComponent<buttonCarreerRaceSelected>().thisRaceDeselected();
			}
			i++;
		}
	}

	public void updateSeason(int seasonId)
	{
		int raceId = 0;
		int columnId = 0;
		int rowId = 0;
		seasonRaceObj = new List<GameObject> ();
		Vector2 itemDimensions = trackTemplate.GetComponent<RectTransform> ().sizeDelta;
		Debug.Log (itemDimensions);
		foreach (SingleRaceProps race in currentSeasonList[seasonId].raceList) {
			int currentTrackID = race.raceId;
			//List<TrackList> currentListOfTrack = PersistentParameterData.trackList;

			GameObject temp = (GameObject)Instantiate (trackTemplate, new Vector3 (0f, 0f, 0f), seasonPannel.transform.rotation);
			//temp.transform.parent = seasonPannel.transform;
			temp.transform.SetParent (seasonPannel.transform, false);
			float currentColumnMargins = (raceItemMargins + itemDimensions [0]) * columnId;
			float currentRowMargins = (raceItemMargins + itemDimensions [1]) * rowId;
			Debug.Log ("row : " + rowId + ", ColumnPos : " + currentColumnMargins + ", RowPos : " + currentRowMargins);
			//temp.transform.localPosition = 
			temp.GetComponent<RectTransform> ().anchoredPosition = new Vector3 (raceItemMargins + currentColumnMargins, -1 * raceItemMargins - currentRowMargins, 0);

			//trackNameObject.GetComponent<Text>().text = currentTrackList[trackid].trackName;
			temp.GetComponent<RawImage>().texture = currentTrackList[currentTrackID].RacePreview;
			temp.GetComponent<buttonCarreerRaceSelected> ().raceId = raceId;
			temp.GetComponent<buttonCarreerRaceSelected> ().carreerRaceManager = this.gameObject;
			seasonRaceObj.Add (temp);

			columnId++;
			if (columnId >= numbColumn-1) {
				columnId = 0;
				Debug.Log ("updating rowId");
				rowId++;
			}
			raceId++;


		}
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
		SceneManagerObject.GetComponent<PersistentParameters>().currentSingleRaceDefinition = new SingleRaceProps(numberOpponents, opponentLevel, windType, currentObjId);
        SceneManagerObject.GetComponent<SceneManagerScript>().LoadScene(currentTrackList[currentObjId].sceneName);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
