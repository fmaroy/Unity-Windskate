using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ObjStoreSelector : MonoBehaviour {
    private GameObject SceneManagerObject;
    private PersistentParameters PersistentParameterData;
    private GameObject RaceManagerObject;
    private UserPreferenceScript RaceManagerData;

    public GameObject CameraObject;
    public string ObjType = "Board";
    public GameObject ObjInStoreRoot;
    public List<StoreObject> ObjInStoreList;

    public int currentObjId = 0;
    public int currentActiveObjLookId = 0;

    public GameObject ObjTypeButtonNext;
    public GameObject ObjTypeButtonPrev;
    public GameObject ObjTypeText;
    
    public GameObject ObjLookButtonNext;
    public GameObject ObjLookButtonPrev;
    // Use this for initialization
    void Start () {
        SceneManagerObject = GameObject.Find("Scene_Manager");
        PersistentParameterData = SceneManagerObject.GetComponent<PersistentParameters>();
        RaceManagerObject = GameObject.Find("RaceManager");
        RaceManagerData = RaceManagerObject.GetComponent<UserPreferenceScript>();
        if (ObjType != "Character")
        {
            ObjInStoreList = new List<StoreObject>();
            int i = 0;
            foreach (Transform child in ObjInStoreRoot.transform)
            {
                foreach (Transform Object in child.transform)
                {
                    if (ObjType == "Board")
                    {
                        //Debug.Log(PersistentParameterData.boardList[i].name);
                        ObjInStoreList.Add(new StoreObject(PersistentParameterData.boardList[i].name, Object.gameObject, PersistentParameterData.boardList[i].looks, PersistentParameterData.boardList[i].activeLook));
                    }
                    if (ObjType == "Sail")
                    {
                        //Debug.Log(PersistentParameterData.sailList[i].name);
                        ObjInStoreList.Add(new StoreObject(PersistentParameterData.sailList[i].name, Object.gameObject, PersistentParameterData.sailList[i].looks, PersistentParameterData.sailList[i].activeLook));
                    }

                    i++;
                }

            }
        }
        if (ObjType == "Board")
        {
            currentObjId = PlayerPrefs.GetInt("Board");
            currentActiveObjLookId = PlayerPrefs.GetInt("BoardLooks");
        }
        if (ObjType == "Sail")
        {
            currentObjId = PlayerPrefs.GetInt("Sail");
            currentActiveObjLookId = PlayerPrefs.GetInt("SailLooks");
        }
        if (ObjType != "Character")
        {
            updateAllObjMat();
        }
        updateObjType(currentObjId);

    }
    
    void OnEnable()
    {
        CameraObject.SetActive(true);
    }
    void OnDisable()
    {
        CameraObject.SetActive(false);
    }

    public void updateAllObjMat()
    {
        int i = 0;
        foreach (StoreObject Obj in ObjInStoreList)
        {
            updateCurrentObjMat(i, ObjInStoreList[i].activeLookId);
            /*ObjectLookSet matSet = Obj.ObjectLooksList[i];
            // for board the looks has to be appplyied on the sub element
            Material[] mats = Obj.Object.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().materials;

            for (int j = 0; j < mats.Length; j++)
            {
                // for board, the first material slot is for the mast base. The if checks the first slot and aplies it's own material
                if (j==0) { mats[j] = Obj.Object.GetComponent<SkinnedMeshRenderer>().materials[0]; }
                else { mats[j] = ObjInStoreList[i].ObjectLooksList[j].lookList[j - 1]; }
                
            }
            // for board the looks has to be appplyied on the sub element
            Obj.Object.transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().materials = mats;
            */
            i++;
        }
    }

    public void updateCurrentObjMat(int objid, int matid)
    {
        //Debug.Log("check!");
        int i = 0;
        foreach (StoreObject Obj in ObjInStoreList)
        {
            if (objid == i)
            {
                //Debug.Log(i);

                ObjectLookSet matSet = Obj.ObjectLooksList[matid];
                // for board the looks has to be appplyied on the sub element
                if (ObjType == "Board")
                {
                    Material[] mats = Obj.Object.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;

                    for (int j = 0; j < mats.Length; j++)
                    {
                        // for board, the first material slot is for the mast base. The if checks the first slot and aplies it's own material
                        if (j == 0) { mats[j] = Obj.Object.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0]; }
                        else
                        {
                            mats[j] = ObjInStoreList[i].ObjectLooksList[matid].lookList[j - 1];
                        }

                    }
                    // for board the looks has to be appplyied on the sub element
                    Obj.Object.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = mats;
                }
                if (ObjType == "Sail")
                {
                    RaceManagerData.updateSailsLooks(Obj.Object, Obj.ObjectLooksList[matid]);
                }
            }
            i++;
        }   
    }

    public void updateObjType(int sailid)
    {
        //SailTypeImage.GetComponent<Image>().sprite = SailTypeImages[sailid];

        int i = 0;
        foreach (StoreObject obj in ObjInStoreList)
        {
            if (sailid == i)
            {
                obj.Object.SetActive(true);
            }
            else
            {
                obj.Object.SetActive(false);
            }
            i++;
        }
        //SailTypeImage.GetComponent<Image>().sprite = SailTypeImages[sailid];
        currentActiveObjLookId = ObjInStoreList[sailid].activeLookId;

        if (sailid == 0)
        { ObjTypeButtonPrev.SetActive(false); }
        else
        { ObjTypeButtonPrev.SetActive(true); }

        int count = 0;

        if (ObjType == "Sail")
        {
            count = PersistentParameterData.sailList.Count;
        }
        else if (ObjType == "Board")
        {
            count = PersistentParameterData.boardList.Count;
        }
        if (ObjType != "Character")
        { 
        if (sailid == count - 1)
        { ObjTypeButtonNext.SetActive(false); }
        else
        { ObjTypeButtonNext.SetActive(true); }
			ObjTypeText.GetComponentInChildren<TextMeshProUGUI>().SetText(ObjInStoreList[sailid].name);
        }
    }

    public void nextObjButton()
    {
        currentObjId = currentObjId + 1;
        currentObjId = Mathf.Clamp(currentObjId, 0, ObjInStoreList.Count - 1);
        updateObjType(currentObjId);
		assignObj ();
    }
    public void prevObjButton()
    {
        currentObjId = currentObjId - 1;
        currentObjId = Mathf.Clamp(currentObjId, 0, ObjInStoreList.Count - 1);
        updateObjType(currentObjId);
		assignObj ();
    }
    public void nextObjLook()
    {
        Debug.Log("nextlook");
        currentActiveObjLookId = currentActiveObjLookId + 1;

        if (currentActiveObjLookId < 0)
        {
            currentActiveObjLookId = ObjInStoreList[currentObjId].ObjectLooksList.Count - 1;
        }
        if (currentActiveObjLookId > ObjInStoreList[currentObjId].ObjectLooksList.Count - 1)
        {
            currentActiveObjLookId = 0;
        }
        currentActiveObjLookId = Mathf.Clamp(currentActiveObjLookId, 0, ObjInStoreList[currentObjId].ObjectLooksList.Count - 1);
        updateCurrentObjMat(currentObjId, currentActiveObjLookId);
		assignObj ();
    }
    public void prevObjLook()
    {
        Debug.Log("prevlook");
        currentActiveObjLookId = currentActiveObjLookId - 1;

        if (currentActiveObjLookId < 0)
        {
            currentActiveObjLookId = ObjInStoreList[currentObjId].ObjectLooksList.Count - 1;
        }
        if (currentActiveObjLookId > ObjInStoreList[currentObjId].ObjectLooksList.Count - 1)
        {
            currentActiveObjLookId = 0;
        }
        //currentSailLook = Mathf.Clamp(currentSailLook, 0, PersistentParameterData.sailList[currentSail].looks.Count - 1);
        updateCurrentObjMat(currentObjId, currentActiveObjLookId);
		assignObj ();
    }

	public void assignObj()
	{
		if (ObjType == "Board") {
			AssignBoard ();
		}
		if (ObjType == "Sail") {
			AssignSail ();
		}
	}

    public void AssignSail()
    {
        PersistentParameterData.PlayerConfig.sail = currentObjId;
        PersistentParameterData.PlayerConfig.sailLook = currentActiveObjLookId;
        PersistentParameterData.sailList[currentObjId].activeLook = currentActiveObjLookId;

        RaceManagerData.updatePlayerPropsSail(GameObject.Find("Player"));

        PlayerPrefs.SetInt("Sail", currentObjId);
        PlayerPrefs.SetInt("SailLooks", currentActiveObjLookId);
    }

    public void AssignBoard()
    {
        PersistentParameterData.PlayerConfig.board = currentObjId;
        PersistentParameterData.PlayerConfig.boardLook = currentActiveObjLookId;
        PersistentParameterData.boardList[currentObjId].activeLook = currentActiveObjLookId;

        RaceManagerData.updatePlayerPropsBoard(GameObject.Find("Player"));

        PlayerPrefs.SetInt("Board", currentObjId);
        PlayerPrefs.SetInt("BoardLooks", currentActiveObjLookId);
    }

}
[System.Serializable]
public class StoreObject
{
    public string name;
    public GameObject Object;
    public List<ObjectLookSet> ObjectLooksList;
    public int activeLookId;
    
    public StoreObject(string n, GameObject g, List<ObjectLookSet> l, int a)
    {
        name = n;
        Object = g;
        ObjectLooksList = l;
        activeLookId = a;
    }
    public StoreObject(GameObject g)
    {
        Object = g;
    }
    public StoreObject(string n, List<ObjectLookSet> l)
    {
        name = n;
        ObjectLooksList = l;
    }
}