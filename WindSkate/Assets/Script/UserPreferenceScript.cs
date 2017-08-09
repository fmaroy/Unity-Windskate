using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine.UI;


public class UserPreferenceScript : MonoBehaviour {
    public GameObject SceneManagerObject;
    public PersistentParameters PersistentParameterData;
    public GameObject ApparentWindArrow;
    public GameObject trueWindArrow;
    public GameObject WindCircle;
    public GameObject UpcomingWindArrow;
    public GameObject UpcomingWindLines;
    public GameObject Player;
    public GameObject Opponents;
    public GameObject PlayerBoard;
    public List<GameObject> opponentsBoards;
    public bool IntroScene = false;
    public GameObject Camera;
    public bool displayWindFx = false;
    public List<ManoeuvreType> localTackManoeuvres = new List<ManoeuvreType>();
    public List<ManoeuvreType> localJibeManoeuvres = new List<ManoeuvreType>();
	public InterfaceControl controlData;

    public GameObject Playerprefab;
    public int numbOpponenents = 2;

    // Use this for initialization
    void Start() {

        Player = this.gameObject.GetComponent<RaceManagerScript>().PlayerObject;
        Opponents = this.gameObject.GetComponent<RaceManagerScript>().OpponentContainerObject;

        SceneManagerObject = GameObject.Find("Scene_Manager");

        if (SceneManagerObject != null)
        {
			localTackManoeuvres = SceneManagerObject.GetComponent<PersistentParameters> ().tackList;
			localJibeManoeuvres = SceneManagerObject.GetComponent<PersistentParameters> ().jibeList;
            numbOpponenents = SceneManagerObject.GetComponent<PersistentParameters>().currentSingleRaceDefinition.numberOfOpponents;
        }
                
        if (IntroScene == false)
        {
            List<GameObject> playerStartPos = new List<GameObject>();
            foreach (Transform child in GameObject.Find("Start_Positions").transform)
            {
                playerStartPos.Add(child.gameObject);
            }

            for (int i = 0; i <= numbOpponenents; i++)
			{
                if (i < numbOpponenents)
                {
                    instantiatePlayer(Playerprefab, playerStartPos[i].transform, Opponents, false);
                }
                else
                {
					Player = instantiatePlayer(Playerprefab, playerStartPos[i].transform, this.transform.parent.gameObject, true);

					Debug.Log (Player.name);
					this.GetComponent<RaceManagerScript> ().PlayerObject = Player;
					initInstantiatedPlayer (this.gameObject.GetComponent<RaceManagerScript> ().PlayerObject);
                    //SetInitialPlayerPosition(playerStartPos[i].transform);
                }
            }
        }

        if (SceneManagerObject != null)
        {
            Debug.Log("Updating from Scene Manager");
            PersistentParameterData = SceneManagerObject.GetComponent<PersistentParameters>();
            updateSettings();
            updatePlayerPropsSail(Player);
            updatePlayerPropsBoard(Player);
            updatePlayerPropsCharacter(Player);
            updateGraphicSettings();
            updateOpponentsProperties(Opponents);
            updateWindCondition();
        }

        this.GetComponent<RaceManagerScript>().getOpponentList(Opponents);

        // To fix!!!
        //updateDisplayWindFx(Player);
        //updateDisplayWindFx(Opponents);
        //updateDisplayWindFx(GameObject.Find("Track_Details"));
    }

    public void updateWindCondition()
    {
        //Debug.Log("WindType Id : " + PersistentParameterData.currentSingleRaceDefinition.typeOfWindsID);
        int windtypeID = PersistentParameterData.currentSingleRaceDefinition.typeOfWindsID;
        WindType currentWind=  PersistentParameterData.ListOfWinds[windtypeID];
        this.gameObject.GetComponent<RaceManagerScript>().setWindBehavior(currentWind);
    }

    public List<GameObject> recursiveChildrenSearch(GameObject searchObject)
    {
        List<GameObject> childrenObject = new List<GameObject>();
        
        foreach (Transform child in searchObject.transform)
        {
            //Debug.Log(child.gameObject.name);
            childrenObject.Add(child.gameObject);
        }
        if (childrenObject.Count == 0)
        {
            childrenObject = null;
        }
        return childrenObject;
    }

    public void updateDisplayWindFx(GameObject rootObject)
    {
        //List<Rigidbody> buildFXList = new List<Rigidbody>();
        List<GameObject> currentObjBranch = recursiveChildrenSearch(rootObject);
        List<GameObject> buildObjectList = new List<GameObject>();
        foreach (GameObject objToAdd in currentObjBranch)
        {
            buildObjectList.Add(objToAdd);
        }
        while (currentObjBranch != null)
        {
            foreach (GameObject obj in currentObjBranch)
            {
                // new List of Object to be recusively searched on next round;
                currentObjBranch = recursiveChildrenSearch(obj);
                // Retained list of objects
                if (currentObjBranch != null)
                {
                    foreach (GameObject objToAdd in currentObjBranch)
                    {
                        buildObjectList.Add(objToAdd);
                    }
                }
            }
        }
        foreach (GameObject currentObject in buildObjectList)
            {
                //Debug.Log("Looking for FX on : " + currentObject.name);
                if (currentObject.CompareTag("WindEffects") == true)
                {
                    //Debug.Log("WindEffects found on :"+ currentObject.name);
                    if (currentObject.GetComponent<MeshRenderer>() != null)
                    {
                        currentObject.GetComponent<MeshRenderer>().enabled = displayWindFx;
                    }
                }
                
            }
        //return buildFXList;
    }
    
    public void SetInitialPlayerPosition(Transform position)
    {
        Player.transform.position = position.position;
        Player.transform.eulerAngles = position.eulerAngles;
    }

	/// <summary>
	/// Returnground Y position of give world position
	/// </summary>
	/// <returns>The player heigh.</returns>
	/// <param name="pos">Position.</param>
	public float checkPlayerHeight(Vector3 pos)
	{
		float verticalPos = 0f;
		RaycastHit hit;
		Vector3 castdir = Vector3.down;
		if (Physics.Raycast (transform.position + Vector3.up * 10.0f, castdir, out hit, 30)) {
				verticalPos = hit.collider.gameObject.transform.transform.position.y;
		}
		return verticalPos;
	}

	/// <summary>
	/// instantiate the player and opponenents.
	/// </summary>
	/// <returns>The player.</returns>
	/// <param name="playerprefab">Playerprefab.</param>
	/// <param name="position">Position.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="isplayer">If set to <c>true</c> isplayer.</param>
	public GameObject instantiatePlayer(GameObject playerprefab, Transform position, GameObject parent, bool isplayer)
    {
		float positioningHeight = checkPlayerHeight (position.position);
		GameObject temp = (GameObject)Instantiate(playerprefab,position.position + Vector3.up * positioningHeight ,position.rotation);
		temp.GetComponent<ExternalObjectsReference> ().initPlayer();
        temp.transform.parent = parent.transform;
        //temp.transform.position = position.position;
        //temp.transform.eulerAngles = position.eulerAngles;
		temp.GetComponent<PlayerCollision>().isPlayer = isplayer;

		if (isplayer == false) {
			foreach (Transform child in temp.transform) {
				if (child.gameObject.name.Contains ("Canvas_WindCircle") == true) {
					child.gameObject.SetActive (false);
				}
			}
		}

		return temp;
    }

	/// <summary>
	/// Setup all the variables of all the object of the scene that need some information from the player
	/// </summary>
	/// <param name="player">Player.</param>
	public void initInstantiatedPlayer(GameObject player)
	{
		if (IntroScene == false) {
			if (GameObject.Find ("OnScreenButtons") != null) {
				controlData = GameObject.Find ("OnScreenButtons").GetComponent<InterfaceControl> ();
				controlData.initControls (player);
			}
		}

		PlayerBoard = player.GetComponentInChildren<BoardForces> ().gameObject;

		//Camera.GetComponent<CameraControlScript> ().playerObject = player;

		GameObject temp = null;
		GameObject temp2 = null;
		foreach (Transform child in Player.GetComponentInChildren<windEffector>().transform) {
			if (child.gameObject.name == "Armature") {
				foreach (Transform subchild in child) {
					if (subchild.gameObject.name == "Torso_00") {
						temp = subchild.gameObject;
					}
				}
			}
		}

		GameObject obj = null;
		foreach (Transform child in player.transform) {
			if (child.gameObject.name == "Canvas_WindCircle") {
				obj = child.gameObject;
			}
		}

		WindCircle = obj;
		trueWindArrow = obj.GetComponent<CircleIndicators>().trueWindArrow;
		ApparentWindArrow = obj.GetComponent<CircleIndicators>().apparentWindArrow;

		temp2 = player.GetComponentInChildren<Follow_track> ().gameObject;

		//Camera.GetComponent<CameraControlScript> ().CameraTarget.GetComponent<CameraTargetScript> ().referenceTransformObjectPosition = temp;
		Camera.GetComponent<CinemachineControls>().playerPosTarget = temp;
		Debug.Log("assign board to camera : " + temp2.name);
		//Camera.GetComponent<CameraControlScript> ().CameraTarget.GetComponent<CameraTargetScript> ().referenceTransformObjectDirection = temp2;
		Camera.GetComponent<CinemachineControls>().playerOrientTarget = temp2;

		Camera.GetComponent<CinemachineControls> ().initCamera ();

	}

    public void updateOpponentsProperties(GameObject OpponentsObj)
    {

        int i = 0;
        foreach (Transform opponent in OpponentsObj.transform)
        {
            opponent.gameObject.GetComponent<PlayerCollision>().opponentId = SceneManagerObject.GetComponent<PersistentParameters>().currentRaceOpponentsListIds[i];
            opponent.gameObject.name = SceneManagerObject.GetComponent<PersistentParameters>().OpponentConfigList[opponent.gameObject.GetComponent<PlayerCollision>().opponentId].name;
            updatePlayerPropsSail(opponent.gameObject);
            updatePlayerPropsBoard(opponent.gameObject);
            updatePlayerPropsCharacter(opponent.gameObject);
            i++;
        }
    }

    public PlayerProperties getPlayerProps(GameObject currentplayer)
    {
        PlayerProperties playerconfig;
        playerconfig = PersistentParameterData.PlayerConfig;

        if (currentplayer.transform.IsChildOf(Opponents.transform) == true)
        {
            playerconfig = PersistentParameterData.OpponentConfigList[currentplayer.GetComponent<PlayerCollision>().opponentId];
        }
        return playerconfig;
    }

    public void updatePlayerPropsCharacter(GameObject currentplayer)
    {
        Debug.Log("Updating Player Character settings");

        PlayerProperties currentPlayerConfig = getPlayerProps(currentplayer);
        playerInventory currentInventory = currentplayer.GetComponent<playerInventory>();
		currentInventory.PlayerName = currentPlayerConfig.name;

        //Debug.Log("Name : " + currentPlayerConfig.name);
        //Debug.Log("Gender: " + currentPlayerConfig.gender);
        GameObject characterObject = currentplayer;
        GameObject currentHair = currentplayer;
        Color hairColor = SceneManagerObject.GetComponent<DataHolder>().HairColorsList[currentPlayerConfig.hairColor];
        Color skinColor = SceneManagerObject.GetComponent<DataHolder>().SkinColorsList[currentPlayerConfig.skinColor];
        int i = 0;
        foreach (GameObject gender in currentInventory.characterList)
        {
            if (i == currentPlayerConfig.gender)
            {
                gender.SetActive(true);
            }
            else
            {
                gender.SetActive(false);
            }
            foreach (Transform child in gender.transform)
            {
                if (child.gameObject.name == "Character")
                {
                    characterObject = child.gameObject;
                    characterObject.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", skinColor);
                    characterObject.GetComponent<SkinnedMeshRenderer>().materials[1].SetColor("_Color", hairColor);
                    int j = 0;
                    foreach (CharacterFeature feature in currentPlayerConfig.featuresList)
                    {
                        characterObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(j, feature.value);
                        j++;
                    }
                }
                ///Search for all hair to change the color of all hair
                foreach (Cloths hair in PersistentParameterData.hairList)
                {
                    if (child.gameObject.name == hair.typeName)
                    {
                        child.gameObject.GetComponent<SkinnedMeshRenderer>().materials[0].SetColor("_Color", hairColor);
                    }
                }
            }
            i++;
        }
    }

    public void updatePlayerPropsBoard(GameObject currentplayer)
    {
        playerInventory currentInventory = currentplayer.GetComponent<playerInventory>();
        PlayerProperties currentPlayerConfig = getPlayerProps(currentplayer);
        //Debug.Log("Updating " + currentPlayerConfig.name + " Board settings");
        currentInventory.currentBoard = currentPlayerConfig.board;
        int i = 0;
        foreach (GameObject board in currentInventory.boardList)
        {
            //Debug.Log("Assign Board");
            if (i == currentPlayerConfig.board)
            {
                board.SetActive(true);
                currentplayer.GetComponentInChildren<Sail_System_Control>().boardDrag = PersistentParameterData.boardList[i].drag;
                Material[] mats = board.GetComponent<MeshRenderer>().materials;
                for (int j = 0; j < mats.Length; j++)
                {
                    // for board, the first material slot is for the mast base. The if checks the first slot and aplies it's own material
                    if (j == 0) { mats[j] = board.GetComponent<MeshRenderer>().materials[0]; }
                    else
                    {
                        //mats[j] = PersistentParameterData.boardList[i].looks[PersistentParameterData.boardList[i].activeLook].lookList[j - 1];
                        mats[j] = PersistentParameterData.boardList[i].looks[currentPlayerConfig.boardLook].lookList[j - 1];
                    }
                }
                // for board the looks has to be appplyied on the sub element
                board.GetComponent<MeshRenderer>().materials = mats;

                foreach (AxleList axle in currentInventory.boardAxleList)
                {
                    if (PersistentParameterData.boardList[i].axleName == axle.name)
                    {
                        //Debug.Log(0);
                        GameObject boardForcesObject;
                        foreach (Transform child in currentplayer.transform)
                        {
                            if (child.gameObject.GetComponent<BoardForces>() != null)
                            {
                                boardForcesObject = child.gameObject;
                            }
                        }
                        int axleId = 0;
                        foreach (GameObject obj in axle.axleList)
                        {
                            obj.SetActive(true);
                            // Get board assembly axles
                            foreach (Transform child in currentplayer.transform)
                            {
                                if (child.gameObject.GetComponent<BoardForces>() != null)
                                {
                                    boardForcesObject = child.gameObject;
                                    if (axleId == 0)
                                    {
                                        boardForcesObject.GetComponent<BoardForces>().front_axis = obj;
                                        boardForcesObject.GetComponent<BoardForces>().updateFrontWheelsControls(obj);
                                    }
                                    if (axleId == 1)
                                    {
                                        boardForcesObject.GetComponent<BoardForces>().rear_axis = obj;
                                        boardForcesObject.GetComponent<BoardForces>().updateRearWheelsControls(obj);
                                    }

                                    axleId++;

                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject obj in axle.axleList)
                        {
                            obj.SetActive(false);
                        }
                    }

                }
            }
            else
            {
                board.SetActive(false);
            }
            i++;
        }
    }

    public bool updateSailsLooks(GameObject sailObj, ObjectLookSet lookSet)
    {
        sailObj.GetComponent<SkinnedMeshRenderer>();
        
        Material[] mats = sailObj.GetComponent<SkinnedMeshRenderer>().materials;

        for (int j = 0; j < mats.Length; j++)
        {
            mats[j] = lookSet.lookList[0];
            //mats[j]= PersistentParameterData.sailList[i].looks[PersistentParameterData.sailList[i].activeLook].looks;
        }
        sailObj.GetComponent<SkinnedMeshRenderer>().materials = mats;
        return true;
    }

    public void updatePlayerPropsSail(GameObject currentplayer)
    {
        //Debug.Log("Updating Player Sail settings");
        playerInventory currentInventory = currentplayer.GetComponent<playerInventory>();
        PlayerProperties currentPlayerConfig = getPlayerProps(currentplayer);
        //PlayerProperties currentPlayerProps = 
        currentInventory.currentSail = currentPlayerConfig.sail;
        int i = 0;
        foreach (GameObject sail in currentInventory.sailsList)
        {
            //Debug.Log(currentPlayerConfig.sail);
            if (i == currentPlayerConfig.sail)
            {
                sail.SetActive(true);
                foreach (Transform child in currentplayer.transform)
                {
                    if (child.gameObject.GetComponent<Sail_System_Control>() != null)
                    {
                        child.gameObject.GetComponent<Sail_System_Control>().sailGeom = sail;
                        child.gameObject.GetComponent<Sail_System_Control>().SailGeomSkinnedMesh = sail.GetComponent<SkinnedMeshRenderer>();

                        updateSailsLooks(sail, PersistentParameterData.sailList[i].looks[currentPlayerConfig.sailLook]);
                        child.gameObject.GetComponent<Sail_System_Control>().sailThrust_factor = PersistentParameterData.sailList[i].power;
                        child.gameObject.GetComponent<Sail_System_Control>().sailDrag_factor = PersistentParameterData.sailList[i].drag;
                        child.gameObject.GetComponent<Sail_System_Control>().sailDrag_rearwind_factor = PersistentParameterData.sailList[i].powerRearWind;
                    }
                }
            }
            else
            {
                sail.SetActive(false);
            }
            i++;
        }
    }
    public void updateGraphicSettings()
    {
        //Debug.Log("update graphic settings");
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("CamQualitySettings"));
        //Debug.Log(PlayerPrefs.GetInt("CamScreenOcclusion"));
        
    }

    public void updateSettings()
    {
        if (SceneManagerObject != null)
        {
			PersistentParameterData = SceneManagerObject.GetComponent<PersistentParameters>();
            //Debug.Log("Update Settings");
            //get Board Object
            int WheelsPhysicsDetailsLevel = PersistentParameterData.wheelPhysicsLevel;
            //Debug.Log("Wheel physics details : " + WheelsPhysicsDetailsLevel);
            foreach (Transform child in Player.transform)
            {
                if (child.gameObject.name == "Board_assembly")
                {
                    //Debug.Log(PersistentParameterData.playerWheelsSettingsList.Count);
                    child.gameObject.GetComponent<WheelsBehavior>().WheelsPhysicsDetails = PersistentParameterData.playerWheelsSettingsList[WheelsPhysicsDetailsLevel].physics;
                    //Debug.Log("test1.2");
                    //Debug.Log("Wheel physics details : " + child.gameObject.GetComponent<WheelsBehavior>().WheelsPhysicsDetails);
                    child.gameObject.GetComponent<WheelsBehavior>().ObjectForPhysicsUpdate = child.gameObject.GetComponent<WheelsBehavior>().updateWheelPhysicsDetails(child.gameObject.GetComponent<WheelsBehavior>().WheelsPhysicsDetails);
                    //Debug.Log("test_1.5");
                    child.gameObject.GetComponent<WheelsBehavior>().WheelSkidMarksEnabled = PersistentParameterData.playerWheelsSettingsList[WheelsPhysicsDetailsLevel].skidEnabled;
                    child.gameObject.GetComponent<WheelsBehavior>().tireParticlesEnabled = PersistentParameterData.playerWheelsSettingsList[WheelsPhysicsDetailsLevel].particleEnabled;
                    //Debug.Log("test1.8");
                }
            }
            //Debug.Log("test2");
            foreach (Transform currentOpponent in Opponents.transform)
            {
                foreach (Transform child in currentOpponent)
                {
                    if (child.gameObject.name == "Board_assembly")
                    {
                        //Debug.Log("Found Opponent Board : " + currentOpponent.gameObject.name);
                        child.gameObject.GetComponent<WheelsBehavior>().WheelsPhysicsDetails = PersistentParameterData.opponentWheelsSettingsList[WheelsPhysicsDetailsLevel].physics;
                        child.gameObject.GetComponent<WheelsBehavior>().ObjectForPhysicsUpdate = child.gameObject.GetComponent<WheelsBehavior>().updateWheelPhysicsDetails(child.gameObject.GetComponent<WheelsBehavior>().WheelsPhysicsDetails);
                        child.gameObject.GetComponent<WheelsBehavior>().WheelSkidMarksEnabled = PersistentParameterData.opponentWheelsSettingsList[WheelsPhysicsDetailsLevel].skidEnabled;
                        child.gameObject.GetComponent<WheelsBehavior>().tireParticlesEnabled = PersistentParameterData.opponentWheelsSettingsList[WheelsPhysicsDetailsLevel].particleEnabled;
                    }
                }
            }
            if (IntroScene == false)
            {
                int i = 0;
                foreach (string settingName in PersistentParameterData.HUDSettingsNames)
                {
                    if (settingName == "Apparent Wind")
                    {
                        setApparentArrow();
                    }
                    if (settingName == "Warning Sector")
                    {
                        setWarningSector();
                    }
                    if (settingName == "Wind Circle")
                    {
                        setWindCircle();
                    }
                    if (settingName == "Upcoming Wind Arrow")
                    {
                        setUpcomingWindArrow();
                    }
                    if (settingName == "Upcoming Wind Lines")
                    {
                        setUpcomingWindLines();
                    }

                    i++;
                }
            }
        }
        else
        {
            //When the Scene Manager is not found (running the scen stand alone), this skips the settings update to avoid errors
            Debug.Log("Skipping Update Settings");
        }
    }
    
    
    void setApparentArrow()
    {
        int index = PersistentParameterData.HUDSettingsNames.IndexOf("Apparent Wind");
        //Debug.Log("Setting Apparent Wind to " + PersistentParameterData.HUDSettingsBool[index]);
        ApparentWindArrow.SetActive(PersistentParameterData.HUDSettingsBool[index]);
    }
    void setWindCircle()
    {
        int index = PersistentParameterData.HUDSettingsNames.IndexOf("Wind Circle");
        //Debug.Log("Setting Wind Circle to " + PersistentParameterData.HUDSettingsBool[index]);
        WindCircle.SetActive(PersistentParameterData.HUDSettingsBool[index]);
    }
    void setUpcomingWindArrow()
    {
        int index = PersistentParameterData.HUDSettingsNames.IndexOf("Upcoming Wind Arrow");
        //Debug.Log("Setting UpcomingWindArrow to " + PersistentParameterData.HUDSettingsBool[index]);
        UpcomingWindArrow.SetActive(PersistentParameterData.HUDSettingsBool[index]);
    }
    void setUpcomingWindLines()
    {
        int index = PersistentParameterData.HUDSettingsNames.IndexOf("Upcoming Wind Lines");
        //Debug.Log("Setting UpcomingWindArrow to " + PersistentParameterData.HUDSettingsBool[index]);
        foreach (Transform line in UpcomingWindLines.transform)
        {
            line.gameObject.GetComponent<MeshRenderer>().enabled = PersistentParameterData.HUDSettingsBool[index];
        }
    }
    void setWarningSector()
    {
        int index = PersistentParameterData.HUDSettingsNames.IndexOf("Warning Sector");
        //Debug.Log("Setting Warning Sector to " + PersistentParameterData.HUDSettingsBool[index]);
        trueWindArrow.GetComponent<UI_True_Wind_Direction>().enableSectors = PersistentParameterData.HUDSettingsBool[index];
        
    }

    // Update is called once per frame
    void Update () {
        


    }
}
