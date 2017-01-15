using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour {
    private GameObject SceneManagerObject;
    private PersistentParameters PersistentParameterData;
    private GameObject RaceManagerObject;
    private UserPreferenceScript RaceManagerData;
    public int tabId;
    public SideMenuHandler sideMenu;
    public GameObject PlayerUICamera;
    public string ObjType = "Character";
    public GameObject ObjInStoreRoot;
    public List<StoreObject> ObjInStoreList;
    
    public int currentObjId = 0;
    public int currentActiveObjLookId = 0;

    public GameObject ObjTypeButtonBoy;
    public GameObject ObjTypeButtonGirl;
    public GameObject PlayerNameText;
    public List<Vector3> UICameraPos = new List<Vector3>();
    public List<float> UICameraSize = new List<float>();

    public GameObject faceShapeSliderContainer;
    public List<GameObject> faceShapeSliderList;
    public List<GameObject> characterMeshList = new List<GameObject>();

    public GameObject SkinColorSlider;
    public GameObject HairColorSlider;

    private int skinColorId;
    private int hairColorId;

    // Use this for initialization
    void Start () {
        UICameraPos.Add(new Vector3(0.38f,57.55f,-104.0f));
        UICameraPos.Add(new Vector3(0.38f, 60.0f, -104.0f));
        UICameraSize.Add(5.0f);
        UICameraSize.Add(2.5f);
        //sideMenu = transform.parent.gameObject.transform.parent.gameObject.GetComponentInChildren<SideMenuHandler>();
        SceneManagerObject = GameObject.Find("Scene_Manager");
        PersistentParameterData = SceneManagerObject.GetComponent<PersistentParameters>();
        RaceManagerObject = GameObject.Find("RaceManager");
        RaceManagerData = RaceManagerObject.GetComponent<UserPreferenceScript>();
        currentObjId = PersistentParameterData.PlayerConfig.gender;
        updateObjType(currentObjId);
        characterMeshList = new List<GameObject>();
        foreach (StoreObject obj in ObjInStoreList)
        {
            foreach (Transform child in obj.Object.transform)
            {
                Debug.Log("ObjName : " + child.gameObject.name);
                if (child.gameObject.name == "Character")
                {
                    Debug.Log("found");
                    characterMeshList.Add(child.gameObject);
                }
            }
        }
        if (tabId == 0)
        {
            PlayerNameText.GetComponent<InputField>().text = PlayerPrefs.GetString("PlayerName");
            
        }
        if (tabId == 1)
        {
            int i = 0;
            foreach (Transform child in faceShapeSliderContainer.transform)
            {
                if (child.gameObject.name == "Slider")
                {
                    Debug.Log("Slider found");
                    faceShapeSliderList.Add(child.gameObject);
                    Debug.Log(i);
                    Debug.Log(PersistentParameterData.PlayerConfig.featuresList[i].name);
                    child.gameObject.GetComponent<Slider>().value = PersistentParameterData.PlayerConfig.featuresList[i].value;
                    Debug.Log("test");
                    i++;
                }
            }
            SkinColorSlider.GetComponent<Slider>().maxValue = SceneManagerObject.GetComponent<DataHolder>().SkinColorsList.Count - 1;
            SkinColorSlider.GetComponent<Slider>().value = PersistentParameterData.PlayerConfig.skinColor;
            HairColorSlider.GetComponent<Slider>().maxValue = SceneManagerObject.GetComponent<DataHolder>().HairColorsList.Count - 1;
            HairColorSlider.GetComponent<Slider>().value = PersistentParameterData.PlayerConfig.hairColor;
        }
        
        Debug.Log("CurrentTab : " + tabId);
        updateTab();
    }
    void OnEnable()
    {
        PlayerUICamera.SetActive(true);
    }
    void OnDisable()
    {
        PlayerUICamera.SetActive(false);
    }
    public void updateSkinColor(int i)
    {
        skinColorId = i;
        Color skinColor = SceneManagerObject.GetComponent<DataHolder>().SkinColorsList[i];
        foreach (GameObject characterMesh in characterMeshList)
        {
            characterMesh.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", skinColor);
        }
    }
    public void skinColorSliderChange()
    {
        int i = Mathf.RoundToInt( SkinColorSlider.GetComponent<Slider>().value);
        updateSkinColor(i);
    }
    public void updateHairColor(int i)
    {
        hairColorId = i;
        Color skinColor = SceneManagerObject.GetComponent<DataHolder>().HairColorsList[i];
        foreach (GameObject characterMesh in characterMeshList)
        {
            characterMesh.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor("_Color", skinColor);
        }
        foreach (StoreObject obj in ObjInStoreList)
        {
            foreach (Cloths hairObject in PersistentParameterData.hairList)
            {
                foreach (Transform child in obj.Object.transform)
                {
                    if (child.gameObject.name == hairObject.typeName)
                    {
                        child.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", skinColor);
                    }
                }
            }
        }
    }
    public void hairColorSliderChange()
    {
        int i = Mathf.RoundToInt(HairColorSlider.GetComponent<Slider>().value);
        updateHairColor(i);
    }
    public void updateCharacter()
    {
        int k = 0;
        foreach (CharacterFeature feature in PersistentParameterData.PlayerConfig.featuresList)
        {
            Debug.Log("updating Face values : " + feature.value + ", with k: " + k);
            float value = feature.value;
            characterMeshList[0].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(k, value);
            characterMeshList[1].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(k, value);
            k++;
        }
        updateHairColor(PersistentParameterData.PlayerConfig.hairColor);
        updateSkinColor(PersistentParameterData.PlayerConfig.skinColor);
    }
    public void faceValueChanged()
    {
        int k = 0;
        foreach (GameObject sliderObject in faceShapeSliderList)
        {
            Debug.Log("updateing Face values : " + sliderObject.GetComponent<Slider>().value + ", with k: " + k);
            float slidervalue = sliderObject.GetComponent<Slider>().value;
            characterMeshList[0].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(k, slidervalue);
            characterMeshList[1].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(k, slidervalue);
            k++;
        }
    }

    public void updateTab()
    {
        Debug.Log("Update Tab");
        currentObjId = PersistentParameterData.PlayerConfig.gender;
        updateObjType(currentObjId);
        Debug.Log("CurrentTab : " + tabId);
        PlayerUICamera.GetComponent<Camera>().transform.localPosition = UICameraPos[tabId];
        PlayerUICamera.GetComponent<Camera>().orthographicSize = UICameraSize[tabId];
        updateCharacter();
        updateObjType(currentObjId);
        PlayerUICamera.GetComponent<Camera>().transform.localPosition = UICameraPos[tabId];
        PlayerUICamera.GetComponent<Camera>().orthographicSize = UICameraSize[tabId];
    }
   
    public void updateObjType(int sailid)
    {
        //SailTypeImage.GetComponent<Image>().sprite = SailTypeImages[sailid];
        int z = 0;
        foreach (StoreObject obj in ObjInStoreList)
        {
            if (sailid == z)
            {
                obj.Object.SetActive(true);
            }
            else
            {
                obj.Object.SetActive(false);
            }
            z++;
        }
        currentObjId = sailid;
    }
    
    public void updatePlayerName()
    {
        PersistentParameterData.PlayerConfig.name = PlayerNameText.GetComponent<InputField>().text;
        PlayerPrefs.SetString("PlayerName", PersistentParameterData.PlayerConfig.name);
    }

    public void assignCharacter()
    {
        Debug.Log(PersistentParameterData.PlayerConfig.gender);
        Debug.Log("currentObjId: " + currentObjId);
        PersistentParameterData.PlayerConfig.gender = currentObjId;
        RaceManagerData.updatePlayerPropsCharacter(GameObject.Find("Player"));
        PlayerPrefs.SetInt("PlayerGender", currentObjId);
    }

    public void assignCharacterProps()
    {
        Debug.Log(PersistentParameterData.PlayerConfig.gender);
        int i = 0;
        foreach (GameObject sliderobject in faceShapeSliderList)
        {
            //string featureName = PersistentParameterData.PlayerConfig.featuresList[i].name;
            string featureName = PersistentParameterData.PlayerConfig.featuresList[i].name;
            PersistentParameterData.PlayerConfig.featuresList[i] = new CharacterFeature (featureName,sliderobject.GetComponent<Slider>().value);
            PlayerPrefs.SetFloat(featureName, sliderobject.GetComponent<Slider>().value);
            i++;
        }
        PersistentParameterData.PlayerConfig.skinColor = skinColorId;
        PersistentParameterData.PlayerConfig.hairColor = hairColorId;
        PlayerPrefs.SetInt("HairColor", hairColorId);
        PlayerPrefs.SetInt("SkinColor", skinColorId);
        RaceManagerData.updatePlayerPropsCharacter(GameObject.Find("Player"));
    }
    
    // Update is called once per frame
    void Update () {
	
	}
}
