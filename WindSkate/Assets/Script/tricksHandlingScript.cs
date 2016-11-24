using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class tricksHandlingScript : MonoBehaviour {

    public Sprite idleStar;
    public Sprite pressedStar;
    public List<GameObject> TricksButton_Level;
    public GameObject TrickButtons;
    public GameObject JumpButton;
    public GameObject EnergySlider;
    public GameObject ButtonManoeuvreLeft;
    public GameObject ButtonManoeuvreRight;

    public GameObject SlidersContainer;
    public List<Slider> SlidersList = new List<Slider>();
    private UserPreferenceScript raceDataUserPref;
    public List<ManoeuvreType> localTackList = new List<ManoeuvreType>();
    public List<ManoeuvreType> localJibeList = new List<ManoeuvreType>();

    public float playerMaxEnergyLevel;

    public int currentTrickType = 0;
    public float EnergyLoadMultiplier = 1.0f;
    public float EnergyMaxLevel = 0.0f;
    public float SpeedMaxLevel = 0.0f;
    public float AngleMaxLevel = 0.0f;
    public int EnergyStarLevel = 0;
    public int SpeedStarLevel = 0;
    public int AngleStarLevel = 0;

    public int StarsMaxLevel = 0;
    public int PrevStarsMaxLevel = 0;
    

    public float currentAngleLevel = 0.0f;
    public float currentEnergyLevel = 0.0f;
    public float currentSpeed = 0.0f;

    public int activeSector = 0;
    public float WindAngle = 0.0f;

    private Sail_System_Control sailControlData;
    private Follow_track followTrackData;

    public List<float> EnergyCostPerTrick = new List<float>();
    public bool isCurrentPlayer;

    static int starHighlightHashEnable ;
    static int starHighlightHashTrigger;

    // Use this for initialization
    void Start () {
        isCurrentPlayer = this.gameObject.GetComponent<PlayerCollision>().isPlayer;
        starHighlightHashEnable = Animator.StringToHash("StarHighlightEnable");
        starHighlightHashTrigger = Animator.StringToHash("StarHighlightTrigger");
        StarsMaxLevel = PrevStarsMaxLevel;

        raceDataUserPref = GameObject.Find("RaceManager").GetComponent<UserPreferenceScript>();
        sailControlData = this.GetComponent<PlayerCollision>().SailSystem.GetComponent<Sail_System_Control>();
        followTrackData = this.GetComponent<PlayerCollision>().Board.GetComponent<Follow_track>();

        if (GameObject.Find("Scene_Manager") != null)
        {
            localTackList = GameObject.Find("Scene_Manager").GetComponent<PersistentParameters>().tackList;
            localJibeList = GameObject.Find("Scene_Manager").GetComponent<PersistentParameters>().jibeList;
        }
        else
        {
            localTackList = raceDataUserPref.localTackManoeuvres;
            localJibeList = raceDataUserPref.localJibeManoeuvres;
        }
        
        EnergyMaxLevel = Mathf.Max(localTackList[2].costEnergy, localJibeList[2].costEnergy);
        SpeedMaxLevel = Mathf.Max(localTackList[2].costSpeed, localJibeList[2].costSpeed);
        AngleMaxLevel = Mathf.Max(localTackList[2].costAngle, localJibeList[2].costAngle);

        if (isCurrentPlayer == true)
        {
            SlidersList = new List<Slider>();
            foreach (Transform child in SlidersContainer.transform)
            {
                SlidersList.Add(child.gameObject.GetComponent<Slider>());
            }

            initSliders();

            playerMaxEnergyLevel = EnergyMaxLevel;
            //ButtonDisplayManoeuvreHandling();
            
        }
    }

    
    void ButtonDisplayManoeuvreHandling()
    {
        if (Mathf.Abs( WindAngle) < 70)
        {
            if (WindAngle < 0)
            {
                ButtonManoeuvreLeft.SetActive(true);
                ButtonManoeuvreRight.SetActive(false);
            }
            else
            {
                ButtonManoeuvreLeft.SetActive(false);
                ButtonManoeuvreRight.SetActive(true);
            }
        }
        if ((Mathf.Abs(WindAngle) > 110))
        {
            if (WindAngle < 0)
            {
                ButtonManoeuvreLeft.SetActive(false);
                ButtonManoeuvreRight.SetActive(true);
            }
            else
            {
                ButtonManoeuvreLeft.SetActive(true);
                ButtonManoeuvreRight.SetActive(false);
            }
        }
        if ((Mathf.Abs(WindAngle) > 70) && (Mathf.Abs(WindAngle) < 110))
        {
            ButtonManoeuvreLeft.SetActive(false);
            ButtonManoeuvreRight.SetActive(false);
        }

    }
    void initSliders()
    {
        int i = 0;
        foreach( Slider slider in SlidersList)
        {
            if (i == 0) { slider.maxValue = EnergyMaxLevel; }
            if (i == 1) { slider.maxValue = SpeedMaxLevel; }
            if (i == 2) { slider.maxValue = AngleMaxLevel; }
            i++;
        }
    }
    void showSliderStars(bool b)
    {
        string nameStar = "Star1";
        // j cycles through the enregy, speed, angle
        
        foreach (Slider Slider in SlidersList)
        {
            for (int i=0; i <= 2; i++)
            {
                if (i == 0) { nameStar = "Star1"; }
                if (i == 1) { nameStar = "Star2"; }
                if (i == 2) { nameStar = "Star3"; }
                GameObject starObj = Slider.gameObject; //temporary assignement
                foreach (Transform child in Slider.gameObject.transform)
                {
                    if (child.gameObject.name == nameStar)
                    {
                        child.gameObject.SetActive(b); // get the object of the Star to be moved
                    }
                }
            }
        }
    }
    void updateStarsPosition(List<ManoeuvreType> manoeuvreList)
    {
        int i = 0;
        int j = 0;
        string nameStar = "Star1";
        float currentLevel = 0;
        float sliderWidth = 200.0f;

        foreach (ManoeuvreType manoeuvre in manoeuvreList)
        {
            //cycle through the tck/jibe manoeuvre
            if (i == 0) { nameStar = "Star1"; }
            if (i == 1) { nameStar = "Star2"; }
            if (i == 2) { nameStar = "Star3"; }
            // j cycles through the enregy, speed, angle
            j = 0;
            foreach (Slider Slider in SlidersList)
            {
                GameObject starObj = Slider.gameObject; //temporary assignement
                foreach (Transform child in Slider.gameObject.transform)
                {
                    if (child.gameObject.name == nameStar)
                    {
                        starObj = child.gameObject; // get the object of the Star to be moved
                    }
                }
                if (j == 0) { currentLevel = manoeuvre.costEnergy; }
                if (j == 1) { currentLevel = manoeuvre.costSpeed; }
                if (j == 2) { currentLevel = manoeuvre.costAngle; }

                sliderWidth = Slider.gameObject.GetComponent<RectTransform>().rect.width;
                float sliderMax = Slider.maxValue;
                starObj.transform.localPosition = new Vector3((currentLevel / sliderMax * sliderWidth) - (sliderWidth / 2), 0.0f, 0.0f);

                j++;
            }
            i++;
        }
    }

    void updateSlider()
    {
        int i = 0;
        foreach (Slider slider in SlidersList)
        {
            if (i == 0) { slider.value = currentEnergyLevel; }
            if (i == 1) { slider.value = currentSpeed; }
            if (i == 2) { slider.value = currentAngleLevel; }
            i++;
        }
    }

	public void enableTrick(int TrickLevel)
    {
        //TricksButton_Level[TrickLevel].GetComponent<Button>().interactable = false;
        
        if (activeSector >2 )
        {
            //Manoeuvre is Jibe
            currentEnergyLevel = currentEnergyLevel - localJibeList[TrickLevel].costEnergy;
        }
        else
        {
            currentEnergyLevel = currentEnergyLevel - localTackList[TrickLevel].costEnergy;
        }
     
        if (currentEnergyLevel < 0) { currentEnergyLevel = 0.0f; }

        this.GetComponentInChildren<SailAnimScript>().Manoeuvre_level = TrickLevel + 1;
    }

    public void enableJump(int TrickLevel)
    {
        Debug.Log("Jump");
    }

    void FixedUpdate()
    {
        currentEnergyLevel = currentEnergyLevel + Time.fixedDeltaTime;
        
        if (currentEnergyLevel >= playerMaxEnergyLevel+1) { currentEnergyLevel = playerMaxEnergyLevel; }

        if (activeSector != 0)
        {
            currentAngleLevel = currentAngleLevel + Time.fixedDeltaTime;
        }
        else
        {
            currentAngleLevel = 0;
        }
    }

    void Update()
    {
        WindAngle = followTrackData.angleBoardToWind;
        currentSpeed = sailControlData.Board_Speed;
        // in the best sector:
        if ((sailControlData.trueWindAngleLocal > 40 && sailControlData.trueWindAngleLocal < 50) || (sailControlData.trueWindAngleLocal > 130 && sailControlData.trueWindAngleLocal < 140))
        {

            if (sailControlData.trueWindAngleLocal > 40 && sailControlData.trueWindAngleLocal < 50)
            {
                if (WindAngle < 0)
                {
                    //Debug.Log("Upwind Starboard");
                    activeSector = 1;
                }
                else
                {
                    //Debug.Log("Upwind Port");
                    activeSector = 2;
                }
            }
            if (sailControlData.trueWindAngleLocal > 130 && sailControlData.trueWindAngleLocal < 140)
            {
                if (WindAngle < 0)
                {
                    //Debug.Log("Downwind Starboard");
                    activeSector = 3;
                }
                else
                {
                    //Debug.Log("Downwind Port");
                    activeSector = 4;
                }
            }
        }
        else
        {
            activeSector = 0;
            EnergyStarLevel = 0;
            SpeedStarLevel = 0;
            AngleStarLevel = 0;
        }

        List<ManoeuvreType> currentManoeuvre = new List<ManoeuvreType>();
        if (sailControlData.trueWindAngleLocal < 70 || sailControlData.trueWindAngleLocal > 110)
        {
            if (sailControlData.trueWindAngleLocal < 70)
            {
                currentManoeuvre = localTackList;
            }
            if (sailControlData.trueWindAngleLocal > 110)
            {
                currentManoeuvre = localJibeList;
            }

            int i = 1;
            foreach (ManoeuvreType manoeuvre in currentManoeuvre)
            {
                if (manoeuvre.costEnergy < currentEnergyLevel)
                {
                    EnergyStarLevel = i;
                }
                if (manoeuvre.costSpeed < currentSpeed)
                {
                    SpeedStarLevel = i;
                }
                if (manoeuvre.costAngle < currentAngleLevel)
                {
                    AngleStarLevel = i;
                }
                i++;
            }
            updateStarsPosition(currentManoeuvre);
            showSliderStars(true);
        }
        else
        {
            //Debug.Log("Hide the stars");
            showSliderStars(false);
        }
        StarsMaxLevel = Mathf.Min(EnergyStarLevel, SpeedStarLevel, AngleStarLevel);

        if (isCurrentPlayer == true)
        {
            updateSlider();
            updateSliderStars();
            ButtonDisplayManoeuvreHandling();
            updateButtonStar(StarsMaxLevel, ButtonManoeuvreLeft);
            updateButtonStar(StarsMaxLevel, ButtonManoeuvreRight);
            //Debug.Log("Star Max Level : "+ StarsMaxLevel);
            animateStarsSprites(StarsMaxLevel);
        }
        
    }
    void LateUpdate()
    {
        PrevStarsMaxLevel = StarsMaxLevel;
    }

    public void animateStarsSprites(int Level)
    {
        List<GameObject> starListButtonRight = new List<GameObject>();
        List<GameObject> starListButtonLeft = new List<GameObject>();
        foreach (Transform child in ButtonManoeuvreLeft.transform)
        {
            if (child.gameObject.name.Contains("Star")) { starListButtonLeft.Add(child.gameObject); }

        }
        foreach (Transform child in ButtonManoeuvreRight.transform)
        {
            if (child.gameObject.name.Contains("Star")) { starListButtonRight.Add(child.gameObject); }
        }
        
        foreach (Slider slider in SlidersList)
        {
            foreach (Transform starObj in slider.gameObject.transform)
            {
                if (Level == 1)
                {
                    if (starObj.gameObject.name == "Star1")
                    {
                        if (PrevStarsMaxLevel != StarsMaxLevel)
                        {
                            //Debug.Log("Playing_Anim");
                            //starObj.gameObject.GetComponent<Animator>().SetBool(starHighlightHashEnable, true);
                            starObj.gameObject.GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                            starListButtonLeft[0].GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                            starListButtonRight[0].GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                        }
                    }
                }
                if (Level == 2)
                {
                    if (starObj.gameObject.name == "Star2")
                    {
                        if (PrevStarsMaxLevel != StarsMaxLevel)
                        {
                            //Debug.Log("Playing_Anim");
                            starObj.gameObject.GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                            starListButtonLeft[1].GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                            starListButtonRight[1].GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                        }
                    }
                }
                if (Level == 3)
                {
                    if (starObj.gameObject.name == "Star3")
                    {
                        if (PrevStarsMaxLevel != StarsMaxLevel)
                        {
                            //Debug.Log("Playing_Anim");
                            starObj.gameObject.GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                            starListButtonLeft[2].GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                            starListButtonRight[2].GetComponent<Animator>().SetTrigger(starHighlightHashTrigger);
                        }
                    }
                }
            }
        }
    }

    public void updateButtonStar(int maxLevel, GameObject Button)
    {
        List<GameObject> starList = new List<GameObject>();
        foreach (Transform child in Button.transform)
        {
            if (child.gameObject.name.Contains("Star")) { starList.Add(child.gameObject); }
            
        }
        int i = 1;
        foreach (GameObject star in starList)
        {
            if (maxLevel >= i)
            {
                star.GetComponent<Image>().sprite = pressedStar;
            }
            else
            {
                star.GetComponent<Image>().sprite = idleStar;
            }
            i++;
        }
    }
    public void updateSliderStars()
    {
        int numbStar = 0;
        int i = 0;
        foreach (Slider slider in SlidersList)
        {
            if (i == 0) { numbStar = EnergyStarLevel; }
            if (i == 1) { numbStar = SpeedStarLevel; }
            if (i == 2) { numbStar = AngleStarLevel; }
            
            foreach (Transform starObj in slider.gameObject.transform)
            {
                if (starObj.gameObject.name == "Star1")
                {
                    if (numbStar >= 1)
                    {
                        starObj.gameObject.GetComponent<Image>().sprite = pressedStar;
                    }
                    else
                    {
                        starObj.gameObject.GetComponent<Image>().sprite = idleStar;
                    }
                }
                if (starObj.gameObject.name == "Star2")
                {
                    if (numbStar >= 2)
                    {
                        starObj.gameObject.GetComponent<Image>().sprite = pressedStar;
                    }
                    else
                    {
                        starObj.gameObject.GetComponent<Image>().sprite = idleStar;
                    }
                }
                if (starObj.gameObject.name == "Star3")
                {
                    if (numbStar >= 3)
                    {
                        starObj.gameObject.GetComponent<Image>().sprite = pressedStar;
                    }
                    else
                    {
                        starObj.gameObject.GetComponent<Image>().sprite = idleStar;
                    }
                }
            }
            i++;
        }
    }

	// Update is called once per frame
	void Update_old () {
        currentEnergyLevel = currentEnergyLevel + Time.deltaTime * EnergyLoadMultiplier;
        
        if (currentEnergyLevel <= EnergyMaxLevel)
        {
            if (isCurrentPlayer == true)
            {
                EnergySlider.GetComponent<Slider>().value = currentEnergyLevel;
            }
        }
        else
        {
            currentEnergyLevel = EnergyMaxLevel;
            
        }
        if (isCurrentPlayer == true)
        {
            for (var n = 0; n < TricksButton_Level.Count; n++)
            {
                if (currentEnergyLevel >= EnergyCostPerTrick[n])
                {
                    TricksButton_Level[n].GetComponent<Button>().interactable = true;
                }
                else
                {
                    TricksButton_Level[n].GetComponent<Button>().interactable = false;
                }
            }
        }
        else
        {
            if (currentEnergyLevel >= EnergyCostPerTrick[0])
            {
                enableTrick(0);
            }
        }
        
    }
}
