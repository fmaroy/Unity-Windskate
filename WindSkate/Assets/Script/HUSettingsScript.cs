using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class HUSettingsScript : MonoBehaviour
{
    private GameObject SceneManagerObject;
    private PersistentParameters PersistentParameterData;

    // Use this for initialization
    void Start()
    {
        SceneManagerObject = GameObject.Find("Scene_Manager");
        PersistentParameterData = SceneManagerObject.GetComponent<PersistentParameters>();
        
        //Lists all avaiable toogles gameobjects to be used in scene
        int i = 0;
        foreach (Transform toogleObject in transform)
        {
            if (toogleObject.gameObject.name.Contains("Toggle") == true)
            {
                if (i < PersistentParameterData.HUDSettingsNames.Count) 
                {
                    Debug.Log(PersistentParameterData.HUDSettingsNames[i] + " : " + PersistentParameterData.HUDSettingsBool[i] + i);
                    toogleObject.gameObject.GetComponent<Toggle>().isOn = PersistentParameterData.HUDSettingsBool[i];
                    toogleObject.gameObject.SetActive(true);
                    
                    foreach (Transform label in toogleObject)
                    {
                        if (label.gameObject.name == "Label")
                        {
                            label.gameObject.GetComponent<Text>().text = PersistentParameterData.HUDSettingsNames[i];
                        }
                    }
                }
                else
                {
                    toogleObject.gameObject.SetActive(false);
                }
                i++;
            }
        }
    }

    public void UpdatePreference()
    {
        int i = 0;
        foreach (Transform toogleObject in transform)
        {
            if (toogleObject.gameObject.name.Contains("Toggle") == true)
            {
                if (i < PersistentParameterData.HUDSettingsNames.Count)
                {
                    PersistentParameterData.HUDSettingsBool[i] = toogleObject.gameObject.GetComponent<Toggle>().isOn;
                }

                i++;
            }
        }
    }
	// Update is called once per frame
	void Update (){
        

    }
}
