using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PersistentParameters : MonoBehaviour
{

    public List<string> HUDSettingsNames = new List<string>();
    public List<bool> HUDSettingsBool = new List<bool>();

    public int wheelPhysicsLevel = 2;
    public List<WheelsSettings> playerWheelsSettingsList = new List<WheelsSettings>();
    public List<WheelsSettings> opponentWheelsSettingsList = new List<WheelsSettings>();

    public int qualityLevel = 0;

    public PlayerProperties PlayerConfig;
    public List<CharacterFeature> characterList = new List<CharacterFeature>();

    public List<PlayerProperties> OpponentConfigList = new List<PlayerProperties>();

    public List<Sail> sailList = new List<Sail>();
    public List<Board> boardList = new List<Board>();

    public List<Cloths> hairList = new List<Cloths>();
    public List<Cloths> helmetList = new List<Cloths>();
    public List<Cloths> shirtList = new List<Cloths>();
    public List<Cloths> pantsList = new List<Cloths>();
    public List<Cloths> shoesList = new List<Cloths>();
    public List<Cloths> accessoryList = new List<Cloths>();
	public List<CareerSeasonProps> seasonList = new List<CareerSeasonProps> ();
    public List<TrackList> trackList = new List<TrackList>();
    public List<WindType> ListOfWinds = new List<WindType>();

    public SingleRaceProps currentSingleRaceDefinition = new SingleRaceProps(3, 3, 0, 0);
	public int currentRaceType = 0; // 0 for single race, 1 for carreer race
	public int currentSeasonId = 0;
	public int currentCarreerTrackId = 0;
    public List<int> currentRaceOpponentsListIds = new List<int>();
    public List<ManoeuvreType> tackList = new List<ManoeuvreType>();
    public List<ManoeuvreType> jibeList = new List<ManoeuvreType>();

	/// <summary>
	/// Assigns a default value for each prefs not defined yet, because a value must exist for the script to work later.
	/// </summary>
	public void initPlayerPref()
    {
        if (PlayerPrefs.HasKey("PlayerName") == false)
        {
            PlayerPrefs.SetString("PlayerName", "Player1");
        }
        if (PlayerPrefs.HasKey("PlayerGender") == false)
        {
            PlayerPrefs.SetInt("PlayerGender", 0);
        }
        if (PlayerPrefs.HasKey("Sail") == false)
        {
            PlayerPrefs.SetInt("Sail", 0);
        }
        if (PlayerPrefs.HasKey("SailLooks") == false)
        {
            PlayerPrefs.SetInt("SailLooks", 0);
        }
        if (PlayerPrefs.HasKey("Board") == false)
        {
            PlayerPrefs.SetInt("Board", 0);
        }
        if (PlayerPrefs.HasKey("BoardLook") == false)
        {
            PlayerPrefs.SetInt("BoardLook", 0);
        }
        if (PlayerPrefs.HasKey("Thinness") == false)
        {
            PlayerPrefs.SetFloat("Thinness", 0.0f);
        }
        if (PlayerPrefs.HasKey("Jaws") == false)
        {
            PlayerPrefs.SetFloat("Jaws", 0.0f);
        }
        if (PlayerPrefs.HasKey("Eyebrow 1") == false)
        {
            PlayerPrefs.SetFloat("Eyebrow 1", 0.0f);
        }
        if (PlayerPrefs.HasKey("Eyebrow 2") == false)
        {
            PlayerPrefs.SetFloat("Eyebrow 2", 0.0f);
        }
        if (PlayerPrefs.HasKey("Eyes") == false)
        {
            PlayerPrefs.SetFloat("Eyes", 0.0f);
        }
        if (PlayerPrefs.HasKey("Lips") == false)
        {
            PlayerPrefs.SetFloat("Lips", 0.0f);
        }
        if (PlayerPrefs.HasKey("HairColor") == false)
        {
            PlayerPrefs.SetInt("HairColor", 5);
        }
        if (PlayerPrefs.HasKey("SkinColor") == false)
        {
            PlayerPrefs.SetInt("SkinColor", 2);
        }
        if (PlayerPrefs.HasKey("CamQualitySettings") == false)
        {
            PlayerPrefs.SetInt("CamQualitySettings", 1);
        }
        if (PlayerPrefs.HasKey("CamScreenOcclusion") == false)
        {
            PlayerPrefs.SetInt("CamScreenOcclusion", 0);
        }
		saveStarsInPrefs (0);
    }

	/// <summary>
	/// Saves the stars status in the Player preferences.
	/// </summary>
	/// <param name="init">Init controls the beahvior: 0 to rest all values to 0, 1 to initialize (add only the missing values), 2 to stores the current values</param>
	public void saveStarsInPrefs (int init)
	{
		int seasonId = 0;
		foreach (CareerSeasonProps currentSeason in seasonList) 
		{
			int raceId = 0;
			foreach (SingleRaceProps currentRace in currentSeason.raceList) 
			{
				string currentPrefName = "numbStarsSeason" + seasonId.ToString() + "Race" + raceId.ToString();
				raceId = raceId + 1;
				if (init == 0) {
					PlayerPrefs.SetInt (currentPrefName, 0); // reset all values to 0
				}
				if (init == 1) {
					if (PlayerPrefs.HasKey (currentPrefName) == false) {
						PlayerPrefs.SetInt (currentPrefName, 0); // initializes only missing values to 0
					}
				}
				if (init == 2) {
					//Debug.Log ("saving numb stars" + currentRace.numbStars + ", in " + currentPrefName);
					PlayerPrefs.SetInt (currentPrefName, currentRace.numbStars);
				}
			}
			seasonId = seasonId + 1;
		}
	}

	/// <summary>
	/// Loads the stars from prefs.
	/// </summary>
	public void loadStarsInPrefs ()
	{
		saveStarsInPrefs (1); //Making sure that there is one pref create for each season races

		int seasonId = 0;
		foreach (CareerSeasonProps currentSeason in seasonList) {
			int raceId = 0;
			foreach (SingleRaceProps currentRace in currentSeason.raceList) {
				string currentPrefName = "numbStarsSeason" + seasonId.ToString () + "Race" + raceId.ToString ();
				int n = PlayerPrefs.GetInt (currentPrefName); // retrieves the number of stars for the current season race
				//Debug.Log ("Loading from " + currentPrefName + " to season, race" + seasonId + ", " + raceId + " numb stars : " + n);
				currentRace.numbStars = n; // assigns the stored stars number to the race object
				//Debug.Log ("Number of stars in scene object");
				raceId = raceId + 1;
			}
			seasonId = seasonId + 1;
		}
	}

    // Use this for initialization
    void Start () {
        List<CharacterFeature> characterList = new List<CharacterFeature>();
        characterList.Add(new CharacterFeature("Thinness", PlayerPrefs.GetFloat("Thinness")));
        characterList.Add(new CharacterFeature("Jaws", PlayerPrefs.GetFloat("Jaws")));
        characterList.Add(new CharacterFeature("Eyebrow 1", PlayerPrefs.GetFloat("Eyebrow 1")));
        characterList.Add(new CharacterFeature("Eyebrow 2", PlayerPrefs.GetFloat("Eyebrows 2")));
        characterList.Add(new CharacterFeature("Eyes", PlayerPrefs.GetFloat("Eyes")));
        characterList.Add(new CharacterFeature("Lips", PlayerPrefs.GetFloat("Lips")));
        PlayerConfig = new PlayerProperties("Player 1" ,true, PlayerPrefs.GetInt("Sail"), PlayerPrefs.GetInt("SailLooks"), PlayerPrefs.GetInt("Board"), PlayerPrefs.GetInt("BoardLooks"));
        PlayerConfig.featuresList = characterList;
        foreach (PlayerProperties opponentProp in OpponentConfigList)
        {
            opponentProp.featuresList = characterList;
        }

        sailList[PlayerPrefs.GetInt("Sail")].activeLook = PlayerPrefs.GetInt("SailLooks");
        boardList[PlayerPrefs.GetInt("Board")].activeLook = PlayerPrefs.GetInt("BoardLooks");
        PlayerConfig.skinColor = PlayerPrefs.GetInt("SkinColor");
        PlayerConfig.hairColor = PlayerPrefs.GetInt("HairColor");
        PlayerConfig.gender = PlayerPrefs.GetInt("PlayerGender");

        Debug.Log("Screen width : " + Screen.width);

        qualityLevel = QualitySettings.GetQualityLevel();
        Debug.Log("QualitySettings: " + qualityLevel);


        //List of Parameters Presets for Wheels Physics
        playerWheelsSettingsList.Add(new WheelsSettings( "Low", 0,false,false));
        playerWheelsSettingsList.Add(new WheelsSettings("Medium", 1, true, false));
        playerWheelsSettingsList.Add(new WheelsSettings("High", 1, true, true));
        playerWheelsSettingsList.Add(new WheelsSettings("Very High", 2, true, true));

        opponentWheelsSettingsList.Add(new WheelsSettings("Low", 0, false, false));
        opponentWheelsSettingsList.Add(new WheelsSettings("Medium", 0, false, false));
        opponentWheelsSettingsList.Add(new WheelsSettings("High", 1, true, false));
        opponentWheelsSettingsList.Add(new WheelsSettings("Very High", 1, true, true));

        //HUDSettingsNames.Add("Apparent Wind");
        HUDSettingsNames.Add("Warning Sector");
        HUDSettingsNames.Add("Wind Circle");
        HUDSettingsNames.Add("Upcoming Wind Arrow");
        HUDSettingsNames.Add("Upcoming Wind Lines");

        // sets everything on by default

        for (int i = 0; i < HUDSettingsNames.Count; i++ )
        {
            HUDSettingsBool.Add(true);
        }
        
		loadStarsInPrefs ();
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(HUDSettingsNames[0] + " : " + HUDSettingsBool[0]);
        //Debug.Log(HUDSettingsNames[1] + " : " + HUDSettingsBool[1]);
    }
}


[System.Serializable]
public class CharacterFeature
{
    public string name;
    public float value;

    public CharacterFeature(string s, float f)
    {
        name = s;
        value = f;
    }
}
/*[System.Serializable]
public class Character
{
    //0 : Male, 1: female
    public int gender;
    public List<CharacterFeature> featuresList;
    public Character(int i)
    {
        gender = i;
    }
}*/

[System.Serializable]
public class CareerSeasonProps
{
	public string name;
	public List<SingleRaceProps> raceList;
	public bool enabledTutorial;
	public List<int> tutorialList;

	public CareerSeasonProps(string n)
	{
		name = n;
	}
}

[System.Serializable]
public class SingleRaceProps
{
    public int numberOfOpponents;
    public int opponentsLevel;
    public int typeOfWindsID;
	public int raceId;
	public bool enabledTutorial;
	public List<int> tutorialList;
	public int numbStars;

	public SingleRaceProps(int n, int l, int w, int r)
    {
        numberOfOpponents = n;
        opponentsLevel = l;
        typeOfWindsID = w;
		raceId = r;
    }
}

[System.Serializable]
public class PlayerProperties
{
    public string name;
    public bool isPlayer;
    public int gender;
    public List<CharacterFeature> featuresList;
    public int hair;
    public int hairColor;
    public int eyebrowseColor;
    public int lipsColor;
    public int skinColor;
    public int helmet;
    public int helmetLook;
    public int shirt;
    public int shirtLook;
    public int pants;
    public int pantsLook;
    public int shoes;
    public int shoesLook;
    public int accessory;

    public int sail;
    public int sailLook;
    public int board;
    public int boardLook;
    public PlayerProperties(string n, bool y, int s, int sl, int b, int bl)
    {
        name = n;
        isPlayer = y;
        sail = s;
        sailLook = sl;
        board = b;
        boardLook = bl;

    }
    public PlayerProperties(string n, bool y, int g, List<CharacterFeature> lchar, int ha, int helm, int shi, int p, int sho, int a ,int s, int sl, int b, int bl)
    {
        name = n;
        isPlayer = y;
        gender = g;
        featuresList = lchar;
        hair = ha;
        helmet = helm;
        shirt = shi;
        pants = p;
        shoes = sho;
        accessory = a;
        sail = s;
        sailLook = sl;
        board = b;
        boardLook = bl;

    }
}

[System.Serializable]
public class WheelsSettings
{
    public string name;
    public int physics;
    public bool skidEnabled;
    public bool particleEnabled;

    public WheelsSettings(string v1, int v2, bool v3, bool v4)
    {
        name = v1;
        physics = v2;
        skidEnabled = v3;
        particleEnabled = v4;
    }
}
[System.Serializable]
public class WindType
{
    public string name;
    public string label;
    // Next is 1 or -1
    //public int rightShift;
    //small shift is 10, big is 20
    public float shiftRange;
    //small oscilation is 5, big is 10
    public float oscillationRange;
    public float targetWindForce;
    public WindType(string s, string l, float sh, float osc, float force)
    {
        name = s;
        label = l;
        shiftRange = sh;
        oscillationRange = osc;
        targetWindForce = force;
    }
}
[System.Serializable]
public class ManoeuvreType
{
    //level varies from 1 to 3
    public int level;
    public string name;
    //type can be 1 for tack, 2 for jibe
    public int type;
    public float duration;
    public float slowDownFactor;
    public float costEnergy;
    public float costSpeed;
    public float costAngle;
    public ManoeuvreType (int l)
    {
        int level = l;
    }
}

