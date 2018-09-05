﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class tricksHandlingScript : MonoBehaviour {

    public InterfaceControl UIData;
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

    public Sail_System_Control sailControlData;
    public Follow_track followTrackData = null;

    public List<float> EnergyCostPerTrick = new List<float>();
    public bool isCurrentPlayer;

    static int starHighlightHashEnable ;
    static int starHighlightHashTrigger;

	public List<Sprite> UIManeuverSpriteList = new List<Sprite> ();
	public List<Sprite> UITrickSpriteList = new List<Sprite> ();

	public bool leftManoeuvreButtonPushed = false;
	public bool rightManoeuvreButtonPushed = false;

	public string manoeuvreStatus = "none";

    public void Init()
    {
        //Debug.Log("Awake");
        Start();

    }
    // Use this for initialization
    void Start()
    {
        //Debug.Log("Start");
        //UIData = this.gameObject.GetComponent<ExternalObjectsReference>().UIControlData;

        isCurrentPlayer = this.gameObject.GetComponent<PlayerCollision>().isPlayer;
        starHighlightHashEnable = Animator.StringToHash("StarHighlightEnable");
        starHighlightHashTrigger = Animator.StringToHash("StarHighlightTrigger");
        StarsMaxLevel = PrevStarsMaxLevel;

        //raceDataUserPref = GameObject.Find("RaceManager").GetComponent<UserPreferenceScript>();
        raceDataUserPref = this.GetComponent<ExternalObjectsReference>().UserPrefs;
        sailControlData = this.GetComponent<PlayerCollision>().SailSystem.GetComponent<Sail_System_Control>();
        followTrackData = this.GetComponent<PlayerCollision>().Board.GetComponent<Follow_track>();

        UIData = GameObject.Find("OnScreenButtons").GetComponent<InterfaceControl>();
        Debug.Log("UIData");
        Debug.Log(UIData);
        setupUIRigging();
    }


    /*public void makingsureAllUIIsRigged()
    {
        if ((SlidersContainer == null)||(ButtonManoeuvreLeft == null)||(ButtonManoeuvreRight == null))
        {
            Debug.Log("Updating UI Rigging");
            setupUIRigging();
        }
    }*/

    public void setupUIRigging()
    {
        Debug.Log("Setting up UI rigging");

        if (this.gameObject.GetComponentInParent<PlayerCollision>().isPlayer == true)
        {
            Debug.Log(UIData);
            ButtonManoeuvreLeft = UIData.ManoeuvreLeftButton;
            ButtonManoeuvreRight = UIData.ManoeuvreRightButton;
            SlidersContainer = UIData.SliderContainer;
        }

        EnergyMaxLevel = Mathf.Max(localTackList[localTackList.Count - 1].costEnergy, localJibeList[localJibeList.Count - 1].costEnergy);
        SpeedMaxLevel = Mathf.Max(localTackList[localTackList.Count - 1].costSpeed, localJibeList[localJibeList.Count - 1].costSpeed);
        AngleMaxLevel = Mathf.Max(localTackList[localTackList.Count - 1].costAngle, localJibeList[localJibeList.Count - 1].costAngle);

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
			manoeuvreStatus = "tack";
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
		if (Mathf.Abs(WindAngle) > 110) 
        {
			manoeuvreStatus = "jibe";
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
		if ((Mathf.Abs (WindAngle) < 40) || (Mathf.Abs (WindAngle) > 145)) {
			ButtonManoeuvreLeft.SetActive(false);
			ButtonManoeuvreRight.SetActive(false);
		}

        if ((Mathf.Abs(WindAngle) > 70) && (Mathf.Abs(WindAngle) < 110))
        {
			manoeuvreStatus = "none";
			//Disabled for beta shipping, disabling the buttons since the functionality is not ready yet:
            ButtonManoeuvreLeft.SetActive(false);
            ButtonManoeuvreRight.SetActive(false);
			updateTrickButtonSprite(ButtonManoeuvreLeft, UITrickSpriteList);
			updateTrickButtonSprite(ButtonManoeuvreRight, UITrickSpriteList);

        } 
		else 
		{
			updateTrickButtonSprite(ButtonManoeuvreLeft, UIManeuverSpriteList);
			updateTrickButtonSprite(ButtonManoeuvreRight, UIManeuverSpriteList);
		}

    }

	void updateTrickButtonSprite(GameObject ButtonObject, List<Sprite> spriteList)
	{
		ButtonObject.GetComponent<Image> ().sprite = spriteList [0];
	}

    void initSliders()
    {
        int i = 0;
        foreach( Slider slider in SlidersList)
        {
			if (i == 0) {slider.maxValue = EnergyMaxLevel;}
            if (i == 1) {slider.maxValue = SpeedMaxLevel;}
			if (i == 2) {slider.maxValue = AngleMaxLevel;}
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
            //cycle through the tack/jibe manoeuvre
            if (i == 1) { nameStar = "Star1"; }
            if (i == 2) { nameStar = "Star2"; }
            if (i == 3) { nameStar = "Star3"; }
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
			if (i == 0) { 
				//if (slider.gameObject.GetComponent<sliderEffectScript> ().animated == false) {
				slider.value = currentEnergyLevel;
			}
            if (i == 1) { slider.value = currentSpeed; }
            if (i == 2) { 
				//if (slider.gameObject.GetComponent<sliderEffectScript> ().animated == false) {
				slider.value = currentAngleLevel; 
			}
            i++;
        }
    }

	public void enableTrick(int TrickLevel)
    {
		float prevEnergylevel = currentEnergyLevel;
        if (activeSector >2 )
        {
            //Manoeuvre is Jibe
            currentEnergyLevel = currentEnergyLevel - localJibeList[TrickLevel+1].costEnergy;
        }
        else
        {
            currentEnergyLevel = currentEnergyLevel - localTackList[TrickLevel+1].costEnergy;
        }
     
        if (currentEnergyLevel < 0) { currentEnergyLevel = 0.0f; }
		//Debug.Log ("Enable trick before slider animation");
		//SlidersList[0].gameObject.GetComponent<sliderEffectScript> ().sliderDecreaseAnimation (prevEnergylevel, currentEnergyLevel);

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
        /*if (this.gameObject.GetComponentInParent<PlayerCollision>().isPlayer == true)
        {
            Debug.Log("Updating UI rigging from Update");
            makingsureAllUIIsRigged();
        }*/

        /*if (followTrackData == null)
        {
            followTrackData = this.GetComponent<PlayerCollision>().Board.GetComponent<Follow_track>();
        }

        if (sailControlData == null)
        {
            sailControlData = this.GetComponent<PlayerCollision>().SailSystem.GetComponent<Sail_System_Control>();
        }*/


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
				
					if (manoeuvre.costEnergy < currentEnergyLevel) {
						EnergyStarLevel = i -1;
					}
					if (manoeuvre.costSpeed < currentSpeed) {
						SpeedStarLevel = i -1;
					}
					if (manoeuvre.costAngle < currentAngleLevel) {
						AngleStarLevel = i -1;
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
		//Removed because i decided to remove the Angle and Speed Sliders now StarsMaxLevel is equal to Energy only
        //StarsMaxLevel = Mathf.Min(EnergyStarLevel, SpeedStarLevel, AngleStarLevel);

		StarsMaxLevel = EnergyStarLevel;

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
}
