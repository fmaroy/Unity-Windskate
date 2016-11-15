                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIWindIndicator : MonoBehaviour {

    public GameObject WindGustsGameObject;
    public List<GameObject> WindGustsGameObjectList = new List<GameObject>();
    private WindGustsBehavior WindGustsGameObjectData;
    public GameObject ReferenceGameObject;
    private windEffector ReferenceGameObjectData;
    public float UIWindAngle;
    private float UIUpcomingWindAngle;
    private GameObject Arrow_outline;
    public GameObject Arrow_filled;
    private Image UIUpcomingWindArrowRenderer;
    private Image UILocalWindArrowRenderer;
    public float UIWindAngleLastFrame;
    private float UIUpcomingWindAngleLastFrame;
    private float UIWindForceLastFrame;
    private float UIUpcomingWindForceLastFrame;
    private float timer;

    public GameObject upcomingWindContainer;
    
    public List<GameObject> eligibleGust = new List<GameObject>();
    public List<GameObject> sortedEligibleGust = new List<GameObject>();
    public List<GameObject> prevSortedEligibleGust = new List<GameObject>();
    public int transitionAnimation = -1;
    public List<float> angleChangePerGust = new List<float>();
    public List<float> forceChangePerGust = new List<float>();
    public Sprite spriteSlightTurn;
    public Sprite spriteTurn;
    public Sprite spriteStraight;
    public Sprite spriteWindIncrease;
    public Sprite spriteWindDecrease;

    public List<UpcomingWindIcon> iconList;

    private AnimatorStateInfo currentBaseState;

    static int arrowHashDisplacement;
    static int arrowHashExit;
    static int arrowOutlineAnimExitTrigger;
    static int arrowOutlineAnimEnableTrigger;
    static int arrowOutlineAnimStatus;


    // Use this for initialization
    void Start()
    {
        ReferenceGameObjectData = ReferenceGameObject.GetComponent<windEffector>();
        WindGustsGameObjectData = WindGustsGameObject.GetComponent<WindGustsBehavior>();
        UIUpcomingWindAngle = WindGustsGameObjectData.currentWindOrientation;
        UIWindAngle = ReferenceGameObjectData.localWindDirection;

        arrowHashDisplacement = Animator.StringToHash("Base Layer.UI_Arrow_Displacement");
        arrowHashExit = Animator.StringToHash("Base Layer.UI_Wind_Arrow");
        arrowOutlineAnimExitTrigger = Animator.StringToHash("UiWindArrowExit");
        arrowOutlineAnimEnableTrigger = Animator.StringToHash("EnableIconTrigger");
        arrowOutlineAnimStatus = Animator.StringToHash("UIArrowStatus");

        foreach (Transform child in this.transform)
        {
            if (child.gameObject.name == "Arrow_outline")
            {
                Arrow_outline = child.gameObject;
            }
            /*else
            {
                Arrow_filled = child.gameObject;
            }*/
        }
        UILocalWindArrowRenderer = Arrow_outline.GetComponent<Image>();
        //UIUpcomingWindArrowRenderer = Arrow_filled.GetComponent<Image>();
        UIWindForceLastFrame = ReferenceGameObjectData.effectiveLocalWindForce;
        UIWindAngleLastFrame = UIWindAngle;
        UIUpcomingWindAngleLastFrame = UIUpcomingWindAngle;
        UIUpcomingWindForceLastFrame = ReferenceGameObjectData.effectiveLocalWindForce;
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.name.Contains("UpcomingWindsContainer") == true)
            {
                upcomingWindContainer = child.gameObject;

            }
        }
        iconList = new List<UpcomingWindIcon>();
        int i = 0;
        foreach (Transform child in WindGustsGameObject.transform)
        {
            iconList.Add(new UpcomingWindIcon(i ,child.gameObject, -1));
            i++;
        }
    }
    
    /// <summary>
    /// Updates the status of the list iconList. Writes a status int of value -1 it the corresponding
    /// Gust is not enabled, 1 if the gust is coming, and 2 if the gust is passed
    /// </summary>
    /// <param name="playerPos"></param>
    public void updateIconListStatus (float playerPos)
    {
        foreach (UpcomingWindIcon iconObj in iconList)
        {
            if (iconObj.gustObject.activeInHierarchy == true)
            {
                iconObj.status = 1;
            }
            else
            {
                iconObj.status = -1;
            }
            float distanceToGust = playerPos - iconObj.gustObject.transform.position.z;
            //Debug.Log("Distance to gust : " + distanceToGust);
            if (distanceToGust < 0)
            {
                iconObj.status = 2;
            }
        }
    }
    /// <summary>
    /// the follwing calculate the oritnetation diff and the force diff for the current icon
    /// </summary>
    /// <param name="iconObj"></param>
    public List<float> iconWindDiffHandler (UpcomingWindIcon iconObj)
    {
        List<float> changeList = new List<float>();
        float orientChange;
        float forceChange;
        UpcomingWindIcon refIconForDiffCalc = null;
        List<UpcomingWindIcon> tmpIconList = new List<UpcomingWindIcon>();
        // checks that the gust of the current icon is active and upcoming
        if (iconObj.status != 1)
        { return changeList; }
        // browse the othe icons to find the next active one if none is found, then the player value has to be used to calcualte the difference
        foreach(UpcomingWindIcon otherIcon in iconList)
        {
            //Checks if the othe gust is active and upcoming and it is not the same as the current one
            if ((otherIcon.status == 1) && (otherIcon!=iconObj))
            {
                // check if the other gust is further down than the current one.
                if (iconObj.gustObject.transform.position.z < otherIcon.gustObject.transform.position.z)
                {
                    // the other gust is between the current one and the player
                    //now we add the other gust to the list of gust between the current one and the player
                    tmpIconList.Add(otherIcon);
                }
            }
        }
        if (tmpIconList.Capacity > 0)
        {
            // Some othe gust has been found between the current gust and the player
            // We need to find the closest one to the current gust to take it as reference for calculation
            float diff = iconObj.gustObject.transform.position.z - tmpIconList[0].gustObject.transform.position.z;
            refIconForDiffCalc = tmpIconList[0];
            foreach (UpcomingWindIcon icon in tmpIconList)
            {
                if (iconObj.gustObject.transform.position.z - icon.gustObject.transform.position.z < diff)
                {
                    diff = iconObj.gustObject.transform.position.z - icon.gustObject.transform.position.z;
                    refIconForDiffCalc = icon;
                }
            }
            // now we have the reference icon to compute the diff values : refIconForDiffCalc
            orientChange = refIconForDiffCalc.gustObject.GetComponent<currentGustProperties>().thisGustOrientation - iconObj.gustObject.GetComponent<currentGustProperties>().thisGustOrientation;
            forceChange = refIconForDiffCalc.gustObject.GetComponent<currentGustProperties>().thisGustForce - iconObj.gustObject.GetComponent<currentGustProperties>().thisGustForce;
            
        }
        else
        {
            // There are no other gust between the current one and the player
            //We need to take the player data for the diff calcualtion
            orientChange = ReferenceGameObjectData.localWindDirection - iconObj.gustObject.GetComponent<currentGustProperties>().thisGustOrientation;
            forceChange = ReferenceGameObjectData.localWindForce - iconObj.gustObject.GetComponent<currentGustProperties>().thisGustForce;
        }
        // Now we have the diff value for the current icon
        //We still need to apply if to the icon graphics
        changeList.Add(orientChange);
        changeList.Add(forceChange);
        return changeList;
    }
    /// <summary>
    ///  manages the display of the icon as well as it displacement
    /// </summary>
    /// <param name="iconObj"></param>
    public void iconDisplayHandler (UpcomingWindIcon iconObj)
    {
        if (iconObj.status == 1)
        {
            iconObj.iconObject.SetActive(true);
        }
        if (iconObj.status < 1)
        {
            iconObj.iconObject.SetActive(false);
        }

    }

 
    /// <summary>
    /// This is called from the windeffector as the player hits the gust object
    /// </summary>
    /// <param name="gust"></param>
    public void WindGustExitHandler(GameObject gust)
    {
        GameObject iconObject;
        //first we need to find the corresponding icon
        int positionInEligibleList = sortedEligibleGust.IndexOf(gust);
        if (positionInEligibleList != -1)
        {
            iconObject = upcomingWindContainer.transform.GetChild(positionInEligibleList).gameObject;
        }
    }

    /// <summary>
    /// This is called from the WindGustBehavior from the container of the gust lines
    /// </summary>
    /// <param name="gust"></param>
    public void WindGustReleasedHandler(GameObject gust)
    {
        GameObject iconObject;
        UpcomingWindIcon SelectedIcon;
        bool flag = false;
        int whichIconInTreeId = 0;
        if (iconList.Count !=0)
        {
            int i = 0;
            foreach (Transform obj in upcomingWindContainer.transform)
            {
                // Browsing the icon Objects in the hierarchy tree
                foreach(UpcomingWindIcon iconObj in iconList)
                {
                    // For each icon object in the hierarchy, we are here browsing all the availale objects in the iconList
                    if (iconObj.iconObject != obj.gameObject)
                    {
                        // Now we have found a icon object in the hierachy that is not used already in the iconList. 
                        // We are storing the id in the whichIconId
                        flag = true;
                        whichIconInTreeId = i;
                        break;
                    }
                }
                if (flag) { break; }
                i++;
            }
        }
        else
        {
            //the icon List is empty, so we are using the first icon object in the hierarchy tree
            flag = true;
            whichIconInTreeId = 0;
        }

        if (flag == true)
        {
            // get the icon object ot be used in the scene tree to be used
            iconObject = upcomingWindContainer.transform.GetChild(whichIconInTreeId).gameObject;
            SelectedIcon = iconList[0];
            foreach ( UpcomingWindIcon icon in iconList)
            {
                if (icon.gustObject == gust)
                {
                    SelectedIcon = icon;
                }
            }
            List<float> newIconDiff = iconWindDiffHandler(SelectedIcon);
            SelectedIcon.iconObject = iconObject;
            if (newIconDiff.Capacity != 0)
            {
                SelectedIcon.orientDiff = newIconDiff[0];
                SelectedIcon.forceDiff = newIconDiff[1];
            }
            //iconList.Add()
        }
    }
    

    /// <summary>
    /// Manages the animation of the wind arrow as soon as the player touches it
    /// </summary>
    public void AppliedWindGustAnimation(GameObject Gust)
    {
        // checks if there was a previous arrow
        if (prevSortedEligibleGust.Count != 0)
        {   // DUring the previous frame, a eligible gust was present
            if (sortedEligibleGust.Count != 0)
            {   // During the current frame, at least one gust is present
                //Now we are checking that the the first previous frame and the current first frame different. If means that the previous frame has been applied an we need to animate it
                if (prevSortedEligibleGust[0]!= sortedEligibleGust[0])
                {
                    AnimateUpcomingWIndIcon(prevSortedEligibleGust[0]);
                    prevSortedEligibleGust[0].GetComponent<Animator>().SetFloat(arrowOutlineAnimStatus, 2.5f);
                    //prevSortedEligibleGust[0].GetComponent<Animator>().SetTrigger(arrowOutlineAnimExitTrigger);
                    //transitionAnimation = WindGustsGameObjectList.IndexOf(prevSortedEligibleGust[0]);
                }
            }
            else
            {
                // means that there were at least one gust in the previous frame, and none in the current frame
                AnimateUpcomingWIndIcon(prevSortedEligibleGust[0]);
                prevSortedEligibleGust[0].GetComponent<Animator>().SetFloat(arrowOutlineAnimStatus, 2.5f);
                //prevSortedEligibleGust[0].GetComponent<Animator>().SetTrigger(arrowOutlineAnimExitTrigger);
                //transitionAnimation = WindGustsGameObjectList.IndexOf(prevSortedEligibleGust[0]);
            }
        }
        

    }
    public void AnimateUpcomingWIndIcon(GameObject obj)
    {
        //obj.GetComponent<Animator>().enabled = true;
        obj.GetComponent<Animator>().SetTrigger(arrowOutlineAnimExitTrigger);
        //obj.GetComponent<Animator>().SetTrigger(arrowOutlineAnimExitTrigger);
    }


    public void updateLocalWindArrow(float WindOrient, float WindForce )
    {
        //UIWindAngle = WindOrient;
        float UILocalwindArrowScale = WindForce / WindGustsGameObjectData.initWindForce;
        Arrow_outline.transform.localScale = new Vector3(1.0f, UILocalwindArrowScale, 1.0f);
        Arrow_outline.transform.rotation = Quaternion.Euler(0, 0, -1*UIWindAngle + 90);
        UILocalWindArrowRenderer.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        
    }

    /*public void updateUpcomingWindArrow(float WindOrient, float WindForce)
    {
        //UIUpcomingWindAngle = WindOrient;
        float UILocalwindArrowScale = WindForce / WindGustsGameObjectData.initWindForce;
        Arrow_filled.transform.localScale = new Vector3(1.0f, UILocalwindArrowScale, 1.0f);
        Arrow_filled.transform.rotation = Quaternion.Euler(0, 0, -1*WindOrient + 90);
        UIUpcomingWindArrowRenderer.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);

    }*/
    void LateUpdate()
    {
        prevSortedEligibleGust = sortedEligibleGust;
    }
    
    // Update is called once per frame
    void Update () {
        //Get the playewr board gameobject
        
        foreach(Transform child in ReferenceGameObject.transform.parent)
        {
            if (child.gameObject.name.Contains("Board_assembly") == true)
            {
                //Update the position of the upcoming wind icon
                //buildGustList(child.position.z);
                updateIconListStatus(child.position.z);

            }
        }

        UIUpcomingWindAngle = WindGustsGameObjectData.currentWindOrientation;
        float UIUpcomingWindForce = WindGustsGameObjectData.currentWindForce;

        UIWindAngle = ReferenceGameObjectData.localWindDirection;
        float UIWindForce = ReferenceGameObjectData.effectiveLocalWindForce;
        
        if (UIWindAngle != UIWindAngleLastFrame || UIWindForce != UIWindForceLastFrame)
        {
            //Debug.Log("Update local wind");
            updateLocalWindArrow(UIWindAngle, UIWindForce);
        }
        else
        {
            UILocalWindArrowRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        /*
        if (UIUpcomingWindAngle != UIUpcomingWindAngleLastFrame || UIUpcomingWindForce != UIUpcomingWindForceLastFrame)
        {
            timer = 2.0f;
        }
        if (timer >= 0.0f)
        {
            timer = timer - Time.deltaTime;
            updateUpcomingWindArrow(UIUpcomingWindAngle, UIUpcomingWindForce);
        }
        else
        {
            UIUpcomingWindArrowRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }*/

        //float UILocalwindArrowScale = ReferenceGameObjectData.effectiveLocalWindForce/ WindGustsGameObjectData.initWindForce;
        //float UIUpcomingWindArrowScale = WindGustsGameObjectData.currentWindForce/ WindGustsGameObjectData.initWindForce;
        //Arrow_outline.transform.localScale = new Vector3 (1.0f, UILocalwindArrowScale, 1.0f);
        //Arrow_filled.transform.rotation = Quaternion.Euler(0, 0, UIUpcomingWindAngle - 90);
        //Arrow_filled.transform.localScale = new Vector3(1.0f, UIUpcomingWindArrowScale, 1.0f);
        //UILocalWIndArrowRenderer.color = new Color(1.0f, 0.2f, 0.2f, 1.0f);

        //Debug.Log(UIWindAngle);
        //UIWindAngle = ReferenceGameObjectData.localWindDirection;
        UIWindForceLastFrame = UIWindForce ;
        UIWindAngleLastFrame = UIWindAngle;
        UIUpcomingWindForceLastFrame = UIUpcomingWindForce;
        UIUpcomingWindAngleLastFrame = UIUpcomingWindAngle;


    }
    /*public void buildEligibleList(float playerPos)
 {
     WindGustsGameObjectList = new List<GameObject>();
     eligibleGust = new List<GameObject>();
     sortedEligibleGust = new List<GameObject>();
     foreach (Transform child in WindGustsGameObject.transform)
     {
         if ((child.gameObject.activeSelf == true))
         {
             WindGustsGameObjectList.Add(child.gameObject);
             float distanceToGust = playerPos - child.gameObject.transform.position.z;
             Debug.Log("Distance to gust : " + distanceToGust);
             if (distanceToGust > 0)
             {
                 eligibleGust.Add(child.gameObject);
             }
         }
     }
     int i = 0;
     foreach (GameObject gust in eligibleGust)
     {
         float distanceToGust = playerPos - gust.transform.position.z;
         if (i == 0)
         {
             sortedEligibleGust.Add(gust);
         }
         else
         {
             if (distanceToGust < playerPos - eligibleGust[i - 1].transform.position.z)
             {
                 sortedEligibleGust.Insert(i - 1, gust);
             }
             else
             {
                 sortedEligibleGust.Add(gust);
             }
         }
         i++;
     }
     //Sorted eligible Gust built
 }*/

    /*public void buildGustList(float playerPos)
    {
        WindGustsGameObjectList = new List<GameObject>();
        eligibleGust = new List<GameObject>();
        sortedEligibleGust = new List<GameObject>();
        angleChangePerGust = new List<float>();
        forceChangePerGust = new List<float>();
        float minIconPos = -130.0f;
        float maxDistanceToGust = 500.0f;
        float canvenasWidth = upcomingWindContainer.GetComponent<RectTransform>().rect.width;

        foreach (Transform child in WindGustsGameObject.transform)
        {
            if ((child.gameObject.activeSelf == true))
            {
                WindGustsGameObjectList.Add(child.gameObject);
                float distanceToGust = playerPos - child.gameObject.transform.position.z;
                Debug.Log("Distance to gust : " + distanceToGust);
                if (distanceToGust > 0)
                {
                    eligibleGust.Add(child.gameObject);
                }
                // Adds the gust ot the list when the first icon on the list stil in exit animation
                
            }
        }
        
        
        //Sort the created list by distance to the player
        int i = 0;
        foreach (GameObject gust in eligibleGust)
        {
            float distanceToGust = playerPos - gust.transform.position.z;
            if (i == 0)
            {
                sortedEligibleGust.Add(gust);
            }
            else
            {
                if (distanceToGust < playerPos - eligibleGust[i - 1].transform.position.z)
                {
                    sortedEligibleGust.Insert(i - 1, gust);
                }
                else
                {
                    sortedEligibleGust.Add(gust);
                }
            }
            i++;
        }

        // initialize the icons by hiding them
        //foreach (Transform child in upcomingWindContainer.transform)
        //{
        ///    child.gameObject.SetActive(false);
        //}
        i = 0;
        foreach (GameObject gust in sortedEligibleGust)
        {
            float distanceToGust = playerPos - gust.transform.position.z;
            float orientChange = 0.0f;
            float forceChange = 0.0f;
            if (i == 0)
            {
                Debug.Log("Player Local WindOrient : " + ReferenceGameObjectData.localWindDirection);
                Debug.Log("This Gust WInd Orient : " + gust.GetComponent<currentGustProperties>().thisGustOrientation);
                //in this case the current Gust is the first one, so its direction and force has to be compared against the player
                orientChange = ReferenceGameObjectData.localWindDirection - gust.GetComponent<currentGustProperties>().thisGustOrientation;
                forceChange = ReferenceGameObjectData.localWindForce - gust.GetComponent<currentGustProperties>().thisGustForce;
                angleChangePerGust.Add(orientChange);
                forceChangePerGust.Add(forceChange);
            }
            else
            {
                //in this case, the current gust will not be the next one impacting the player, so the direction and force has to be compared to the previous one.
                orientChange = sortedEligibleGust[i - 1].GetComponent<currentGustProperties>().thisGustOrientation - gust.GetComponent<currentGustProperties>().thisGustOrientation;
                forceChange = sortedEligibleGust[i - 1].GetComponent<currentGustProperties>().thisGustForce - gust.GetComponent<currentGustProperties>().thisGustForce;
                angleChangePerGust.Add(orientChange);
                forceChangePerGust.Add(forceChange);
            }

            Debug.Log("Gust " + i + "change Orient: " + orientChange + ", change force : " + forceChange);
            //upcomingWindIndicatorUpdate(i, distanceToGust, orientChange, forceChange);

            //now we setup the position of the upcoming wind icons
            //

            //AppliedWindGustAnimation();

            int iconObjectinterator = 0;
            foreach (Transform child in upcomingWindContainer.transform)
            {
                
                //enables and places the icons correspoinding to only the active gust in sorted eligible
                if (iconObjectinterator == i)
                {
                    //child.gameObject.SetActive(true);
                    Debug.Log("Wind Relative distance: " + distanceToGust / maxDistanceToGust);
                    Debug.Log("Icon Placement: " + -1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos);

                    //child.gameObject.transform.localPosition = new Vector3(-1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos, -3.0f, 0.0f);
                    child.gameObject.GetComponent<Animator>().Play(arrowHashDisplacement, 0, ((1-distanceToGust) / maxDistanceToGust));

                    if ((Mathf.Abs(orientChange) <= 1))
                    {
                        child.GetChild(0).gameObject.GetComponent<Image>().sprite = spriteStraight;
                        
                        child.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(35.0f, 72.0f);
                        //child.gameObject.transform = new Vector3(0.35f, 0.7f, 1.0f);
                    }
                    if ((Mathf.Abs(orientChange) > 1) && (Mathf.Abs(orientChange) <= 5))
                    {
                        child.GetChild(0).gameObject.GetComponent<Image>().sprite = spriteSlightTurn;
                        //child.GetChild(0).gameObject.transform.localPosition = new Vector3(-1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos, 2.0f, 0.0f);
                        child.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(86.0f, 93.0f);
                        //child.gameObject.transform.localScale = new Vector3( 0.78f, 0.9f, 1.0f);

                    }
                    if ((Mathf.Abs(orientChange) > 5))
                    {
                        child.GetChild(0).gameObject.GetComponent<Image>().sprite = spriteTurn;
                        //child.GetChild(0).gameObject.transform.localPosition = new Vector3(-1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos, 5.5f, 0.0f);
                        child.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(121.0f, 122.0f);
                        //child.gameObject.transform.localScale = new Vector3( 0.65f , 0.9f , 1.0f);
                    }

                    if (orientChange > 0)
                    {
                        child.GetChild(0).gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }
                    else
                    {
                        child.GetChild(0).gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    }
                    //foreach (Transform subchild in child)

                    if (Mathf.Abs(forceChange) < 1)
                    {
                        child.GetChild(1).gameObject.SetActive(false);
                    }
                    else
                    {
                        child.GetChild(1).gameObject.SetActive(true);
                        if (forceChange > 0)
                        {
                            child.GetChild(1).gameObject.GetComponent<Image>().sprite = spriteWindIncrease;
                            child.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(20.0f, 20.0f);
                        }
                        else
                        {
                            child.GetChild(1).gameObject.GetComponent<Image>().sprite = spriteWindDecrease;
                            child.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(20.0f, 8.0f);
                        }
                        if ((Mathf.Abs(orientChange) <= 1))
                        {
                            if (orientChange > 0)
                            {
                                child.GetChild(1).gameObject.transform.localPosition = new Vector3(-20.0f, -30.0f, 0.0f);
                            }
                            else
                            {
                                child.GetChild(1).gameObject.transform.localPosition = new Vector3(20.0f, -30.0f, 0.0f);
                            }
                        }
                        else
                        {
                            child.GetChild(1).gameObject.transform.localPosition = new Vector3(20.0f, -30.0f, 0.0f);
                        }
                    }
                }
                iconObjectinterator++;
                int prevGustIterator = 0;
                foreach (GameObject prevGust in prevSortedEligibleGust)
                {
                    if (child != prevGust)
                    {
                        //The current eligible gust 
                    }
                    prevGustIterator++;
                }
            }
            i++;
        }
    }*/
}
/// <summary>
/// Upcoming Gust Object
/// id is the current number of the gust
/// This Object retains the icon object and the corresponding gust object
/// 
/// </summary>
[System.Serializable]
public class UpcomingWindIcon
{
    public int id;
    public GameObject iconObject;
    public GameObject gustObject;
    public int status;
    public float forceDiff;
    public float orientDiff;
    /*public UpcomingWindIcon(int i, GameObject icon, GameObject gust, float force, float orient)
    {
        int id = i;
        GameObject iconObject = icon;
        GameObject gustObject = gust;
        float forcediff = force;
        float orientDiff = orient;
    }*/
    public UpcomingWindIcon(int i, GameObject gust, int s)
    {
        id = i;
        gustObject = gust;
        status = s;
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      