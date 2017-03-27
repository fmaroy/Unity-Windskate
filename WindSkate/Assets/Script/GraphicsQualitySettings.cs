using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GraphicsQualitySettings : MonoBehaviour {

    public GameObject Camera;
    private List<string> qualitySetttingsNames = new List<string>();
    public GameObject SliderObject; 
    public GameObject toogleOcclusionObject;
    private List<bool> QualitySettingsBool = new List<bool>();
    
    public GameObject wheelsPhysicsSlider;
    private GameObject RaceManager;

    private int currentQualitySettings; 
    //private bool toggleImg = false;

    public void updateQualitySettings(int toto)
    {
        int i = 0;
        foreach (Transform toogleObject in transform)
        {
            if (toogleObject.gameObject.name.Contains("Toggle") == true)
            {
                if (i < qualitySetttingsNames.Count)
                {
                    if (toogleObject.gameObject.GetComponent<Toggle>().isOn)
                    {
                        QualitySettings.SetQualityLevel(i);
                        RaceManager.GetComponent<PersistentParameters>().qualityLevel = i;
                    }
                }
            }
            i++;
        }
    }
    
    // Use this for initialization
    void Start () {
        RaceManager=  GameObject.Find("Scene_Manager");
        foreach (string name in QualitySettings.names)
        {
            qualitySetttingsNames.Add(name);
            QualitySettingsBool.Add(false);
        }

        currentQualitySettings = RaceManager.GetComponent<PersistentParameters>().qualityLevel;

        //Debug.Log(qualitySetttingsNames);
        //Debug.Log(QualitySettings.names);

        SliderObject.GetComponent<Slider>().maxValue = qualitySetttingsNames.Count-1;

        SliderObject.GetComponent<Slider>().value = currentQualitySettings;

        if (PlayerPrefs.GetInt("CamScreenOcclusion") == 1)
        {
            toogleOcclusionObject.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            toogleOcclusionObject.GetComponent<Toggle>().isOn = false;
        }
        
        wheelsPhysicsSlider.GetComponent<Slider>().maxValue = RaceManager.GetComponent<PersistentParameters>().playerWheelsSettingsList.Count -1;
        wheelsPhysicsSlider.GetComponent<Slider>().value = RaceManager.GetComponent<PersistentParameters>().wheelPhysicsLevel;

        foreach (Transform child in wheelsPhysicsSlider.transform)
        {
            if (child.gameObject.name == "Text")
            {
                child.gameObject.GetComponent<Text>().text = "Wheels Physics : " + RaceManager.GetComponent<PersistentParameters>().playerWheelsSettingsList[RaceManager.GetComponent<PersistentParameters>().wheelPhysicsLevel].name;
            }
        }
    }

    public void wheelSliderChange()
    {
        RaceManager.GetComponent<PersistentParameters>().wheelPhysicsLevel = Mathf.FloorToInt(wheelsPhysicsSlider.GetComponent<Slider>().value);
        foreach (Transform child in wheelsPhysicsSlider.transform)
        {
            if (child.gameObject.name == "Text")
            {
                child.gameObject.GetComponent<Text>().text = "Wheels Physics : " + RaceManager.GetComponent<PersistentParameters>().playerWheelsSettingsList[RaceManager.GetComponent<PersistentParameters>().wheelPhysicsLevel].name;
            }
        }
    }

    public void sliderChange()
    {
        Debug.Log("QualitySetting set to : " + qualitySetttingsNames[Mathf.FloorToInt(SliderObject.GetComponent<Slider>().value)]);
        currentQualitySettings = Mathf.FloorToInt(SliderObject.GetComponent<Slider>().value);
        foreach (Transform child  in SliderObject.transform)
        {
            if (child.gameObject.name == "Text")
            {
                child.gameObject.GetComponent<Text>().text = "Quality: " + qualitySetttingsNames[Mathf.FloorToInt(SliderObject.GetComponent<Slider>().value)];
            }
        }
        PlayerPrefs.SetInt("CamQualitySettings", currentQualitySettings);
        GameObject.Find("RaceManager").GetComponent<UserPreferenceScript>().updateGraphicSettings();
        //setQualitySetting();
    }

    public void toggleChange(int toogleId)
    {
        int i = 0;
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.GetComponent<Toggle>() != null)
            {
                if (toogleId == 0)
                {
                    if (child.gameObject.GetComponent<Toggle>().isOn == true)
                    {
                        //Debug.Log("enable");
                        //Camera.GetComponent<ScreenSpaceAmbientOcclusion>().enabled = true;
                        PlayerPrefs.SetInt("CamScreenOcclusion", 1);
                    }
                    else
                    {
                        //Debug.Log("disable");
                        //Camera.GetComponent<ScreenSpaceAmbientOcclusion>().enabled = false;
                        PlayerPrefs.SetInt("CamScreenOcclusion", 0);
                    }
                    i++;
                }
            }
        }
        GameObject.Find("RaceManager").GetComponent<UserPreferenceScript>().updateGraphicSettings();
        //setQualitySetting();
    }
    public void setCameraFilters()
    {
        if (PlayerPrefs.GetInt("CamScreenOcclusion")== 1 )
        {
            Camera.GetComponent<ScreenSpaceAmbientOcclusion>().enabled = true;
        }
        else
        {
            Camera.GetComponent<ScreenSpaceAmbientOcclusion>().enabled = false;
        }
        GameObject.Find("RaceManager").GetComponent<UserPreferenceScript>().updateGraphicSettings();
    }

    public void setQualitySetting()
    {
        currentQualitySettings = PlayerPrefs.GetInt("CamQualitySettings");
        //RaceManager.GetComponent<PersistentParameters>().qualityLevel = currentQualitySettings;
        QualitySettings.SetQualityLevel(currentQualitySettings);
    }

    /*public void toogleChange()
    {
        int i = 0;
        foreach (Transform toogleObject in transform)
        {
            if (toogleObject.gameObject.name.Contains("Toggle") == true)
            {
                if (i < qualitySetttingsNames.Count)
                {
                    Debug.Log("Found");
                    if (toogleObject.gameObject.GetComponent<Toggle>().isOn == true)
                    {
                        Debug.Log("QualitySetting set to : " + i);
                        int currentQualitySettings = i;
                        RaceManager.GetComponent<PersistentParameters>().qualityLevel = currentQualitySettings;
                        QualitySettings.SetQualityLevel(currentQualitySettings);
                    }
                }
            }
            i++;
        }
    }
    */
    void OnGUI()
    {

    }

    // Update is called once per frame
    void Update () {
	
	}
}
