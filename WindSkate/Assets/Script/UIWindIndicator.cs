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

    public float PlayerPosition;

    private AnimatorStateInfo currentBaseState;

    static int arrowHashDisplacement;
    static int arrowHashExitState;
    static int arrowOutlineAnimExitTrigger;
    static int arrowOutlineAnimEnableTrigger;
    static int arrowOutlineAnimStatus;
    static int upcomingWindArrowAppliedAnimStatus;
    static int upcomingWindArrowAppliedInt;


    // Use this for initialization
    void Start()
    {
        ReferenceGameObjectData = ReferenceGameObject.GetComponent<windEffector>();
        WindGustsGameObjectData = WindGustsGameObject.GetComponent<WindGustsBehavior>();
        UIUpcomingWindAngle = WindGustsGameObjectData.currentWindOrientation;
        UIWindAngle = ReferenceGameObjectData.localWindDirection;

        arrowHashDisplacement = Animator.StringToHash("Base Layer.UI_Arrow_Displacement");
        arrowHashExitState = Animator.StringToHash("Base Layer.UI_Wind_Arrow");
        arrowOutlineAnimExitTrigger = Animator.StringToHash("UiWindArrowExit");
        arrowOutlineAnimEnableTrigger = Animator.StringToHash("EnableIconTrigger");
        arrowOutlineAnimStatus = Animator.StringToHash("UIArrowStatus");
        upcomingWindArrowAppliedInt = Animator.StringToHash("UiIconAnimApply");

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
    /// the following calculates the oritnetation diff and the force diff for the current icon
    /// </summary>
    /// <param name="iconObj"></param>
    /// 

    public void updateUpcomingWindIconPosition(UpcomingWindIcon icon)
    {
        float minIconPos = -130.0f;
        float maxDistanceToGust = 500.0f;
        float canvenasWidth = upcomingWindContainer.GetComponent<RectTransform>().rect.width;

        if (icon.status == 1)
        {
            float distanceToGust = PlayerPosition - icon.gustObject.transform.position.z;

            //Debug.Log("Wind Relative distance: " + distanceToGust / maxDistanceToGust);
            //Debug.Log("Icon Placement: " + -1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos);

            icon.iconObject.transform.localPosition = new Vector3(-1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos, -3.0f, 0.0f);
            //icon.gameObject.GetComponent<Animator>().Play(arrowHashDisplacement, 0, ((1 - distanceToGust) / maxDistanceToGust));
        }
    }

    public List<float> iconWindDiffHandler (UpcomingWindIcon iconObj)
    {
        List<float> changeList = new List<float>();
        float orientChange;
        float forceChange;
        UpcomingWindIcon refIconForDiffCalc = null;
        List<UpcomingWindIcon> tmpIconList = new List<UpcomingWindIcon>();
        // checks that the gust of the current icon is active and upcoming
        if (iconObj.status != 1)
        {
            //Debug.Log("The Icon is not active nor upcoming");
            return changeList;
        }
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
            orientChange = iconObj.gustObject.GetComponent<currentGustProperties>().thisGustOrientation - refIconForDiffCalc.gustObject.GetComponent<currentGustProperties>().thisGustOrientation;
            forceChange = iconObj.gustObject.GetComponent<currentGustProperties>().thisGustForce - refIconForDiffCalc.gustObject.GetComponent<currentGustProperties>().thisGustForce;
            
        }
        else
        {
            // There are no other gust between the current one and the player
            //We need to take the player data for the diff calcualtion
            orientChange = iconObj.gustObject.GetComponent<currentGustProperties>().thisGustOrientation - ReferenceGameObjectData.localWindDirection;
            forceChange = iconObj.gustObject.GetComponent<currentGustProperties>().thisGustForce - ReferenceGameObjectData.localWindForce;
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
    public void iconDisplayHandler(UpcomingWindIcon iconObj)
    {
        if (iconObj.iconObject != null)
        {
            iconObj.iconObject.transform.GetChild(0).gameObject.SetActive(false);
            if (iconObj.status == 1)
            {
                //Debug.Log(iconObj.iconObject.name + ", active icon to enable found, "+ iconObj.iconObject.transform.GetChild(0).gameObject.name);
                iconObj.iconObject.transform.GetChild(0).gameObject.SetActive(true);
                if (Mathf.Abs(iconObj.forceDiff) < 1)
                {
                    iconObj.iconObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    iconObj.iconObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
            }
            arrowHashExitState = Animator.StringToHash("Base Layer.UI_Wind_Arrow");
            currentBaseState = iconObj.iconObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            if ((iconObj.status == 2) && (currentBaseState.fullPathHash == arrowHashExitState))
            {
                iconObj.iconObject.transform.GetChild(0).gameObject.SetActive(true);
                if (Mathf.Abs(iconObj.forceDiff) < 1)
                {
                    iconObj.iconObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    iconObj.iconObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                }
            }
            
            /*if (iconObj.status < 1)
            {
                iconObj.iconObject.transform.GetChild(0).gameObject.SetActive(false);
            }*/
        }
    }
 
    /// <summary>
    /// This is called from the windeffector as the player hits the gust object
    /// </summary>
    /// <param name="gust"></param>
    public void WindGustExitHandler(GameObject gust)
    {
        GameObject iconObject = null;
        UpcomingWindIcon currentIconObject;
        //first we need to find the corresponding icon
        /*int positionInEligibleList = sortedEligibleGust.IndexOf(gust);
        if (positionInEligibleList != -1)
        {
            iconObject = upcomingWindContainer.transform.GetChild(positionInEligibleList).gameObject;
        }*/
        
        foreach (UpcomingWindIcon icon in iconList)
        {
            if (icon.gustObject == gust)
            {
                iconObject = icon.iconObject;
                currentIconObject = icon;
            }
        }
        //iconObject.GetComponent<Animator>().SetInteger(upcomingWindArrowAppliedInt, 1);
        iconObject.GetComponent<Animator>().SetTrigger(arrowOutlineAnimExitTrigger);
        //currentIconObject.exitAnimFlag = true;

    }

    /// <summary>
    /// This is called from the WindGustBehavior from the container of the gust lines
    /// </summary>
    /// <param name="gust"></param>
    public void WindGustReleasedHandler(GameObject gust)
    {
        GameObject iconObject;
        UpcomingWindIcon SelectedIcon;
        int whichIconInTreeId = 0;
        bool flagIconInUse = false;
       
        int i = 0;
        //Looking for a suitable icon Object
        foreach (Transform obj in upcomingWindContainer.transform)
        {
            // Browsing the icon Objects in the hierarchy tree
            // gloal is to check if the current icon is already in used for an other iconObject (Gust)
            flagIconInUse = false;
            foreach (UpcomingWindIcon iconObj in iconList)
            {
                // For each icon object in the hierarchy, we are here browsing all the availale objects in the iconList
                if (iconObj.iconObject == obj.gameObject)
                {
                    // Now we have found a icon object in the hierachy that is not used already in the iconList. 
                    // We are storing the id in the whichIconId
                    flagIconInUse = true;
                    //Debug.Log("Icon " + obj.gameObject.name + " is already used for " + gust.name);
                    
                    //This icon is already being used by this iconObject, jumping to the next one.
                    break;
                }
            }
            if (flagIconInUse == false)
            {
                //Debug.Log("The icon " + obj.gameObject.name + " is not being use by any other iconObj Gust, it can be associated with " + gust.name);
                whichIconInTreeId = i;
                //Debug.Log("Icon id : " + whichIconInTreeId);
                break;
            }
            i++;
        }
        
        if (flagIconInUse == false)
        {
            // get the icon object ot be used in the scene tree to be used
            iconObject = upcomingWindContainer.transform.GetChild(whichIconInTreeId).gameObject;
            SelectedIcon = iconList[0];
            foreach ( UpcomingWindIcon icon in iconList)
            {
                if (icon.gustObject == gust)
                {
                    //We get the icon object of the released gust
                    SelectedIcon = icon;
                }
            }

            SelectedIcon.iconObject = iconObject;
            SelectedIcon.status = 1;
            SelectedIcon.exitAnimFlag = false;
            List<float> newIconDiff = iconWindDiffHandler(SelectedIcon);
            
            if (newIconDiff.Capacity != 0)
            {
                SelectedIcon.orientDiff = newIconDiff[0];
                SelectedIcon.forceDiff = newIconDiff[1];
            }
            //Now we need to handle the icon is displayed

            if ((Mathf.Abs(SelectedIcon.orientDiff) <= 1))
            {
                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = spriteStraight;

                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(35.0f, 72.0f);
                //child.gameObject.transform = new Vector3(0.35f, 0.7f, 1.0f);
            }
            if ((Mathf.Abs(SelectedIcon.orientDiff) > 1) && (Mathf.Abs(SelectedIcon.orientDiff) <= 5))
            {
                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = spriteSlightTurn;
                //child.GetChild(0).gameObject.transform.localPosition = new Vector3(-1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos, 2.0f, 0.0f);
                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(86.0f, 93.0f);
                //child.gameObject.transform.localScale = new Vector3( 0.78f, 0.9f, 1.0f);

            }
            if ((Mathf.Abs(SelectedIcon.orientDiff) > 5))
            {
                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = spriteTurn;
                //child.GetChild(0).gameObject.transform.localPosition = new Vector3(-1 * (distanceToGust / maxDistanceToGust * canvenasWidth) + minIconPos, 5.5f, 0.0f);
                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(121.0f, 122.0f);
                //child.gameObject.transform.localScale = new Vector3( 0.65f , 0.9f , 1.0f);
            }

            if (SelectedIcon.orientDiff > 0)
            {
                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                
            }
            else
            {
                SelectedIcon.iconObject.transform.GetChild(0).GetChild(0).localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            //foreach (Transform subchild in child)

            if (Mathf.Abs(SelectedIcon.forceDiff) > 1)
            {
                //SelectedIcon.iconObject.transform.GetChild(1).gameObject.GetComponent<Image>().enabled = true;
                if (SelectedIcon.forceDiff > 0)
                {
                    SelectedIcon.iconObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = spriteWindIncrease;
                    SelectedIcon.iconObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(20.0f, 20.0f);
                }
                else
                {
                    SelectedIcon.iconObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = spriteWindDecrease;
                    SelectedIcon.iconObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(20.0f, 8.0f);
                }
                if ((Mathf.Abs(SelectedIcon.orientDiff) <= 1))
                {
                    if (SelectedIcon.orientDiff > 0)
                    {
                        SelectedIcon.iconObject.transform.GetChild(0).GetChild(1).gameObject.transform.localPosition = new Vector3(-20.0f, -30.0f, 0.0f);
                    }
                    else
                    {
                        SelectedIcon.iconObject.transform.GetChild(0).GetChild(1).gameObject.transform.localPosition = new Vector3(20.0f, -30.0f, 0.0f);
                    }
                }
                else
                {
                    SelectedIcon.iconObject.transform.GetChild(0).GetChild(1).gameObject.transform.localPosition = new Vector3(20.0f, -30.0f, 0.0f);
                }
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


    void LateUpdate()
    {
        prevSortedEligibleGust = sortedEligibleGust;
        foreach(UpcomingWindIcon icon in iconList)
        {
            if (icon.iconObject != null)
            {
                icon.iconObject.GetComponent<Animator>().ResetTrigger(arrowOutlineAnimExitTrigger);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = 0.0f;

        //Get the player board gameobject
        foreach (Transform child in ReferenceGameObject.transform.parent)
        {
            if (child.gameObject.name.Contains("Board_assembly") == true)
            {
                //Update the position of the upcoming wind icon
                //buildGustList(child.position.z);
                PlayerPosition = child.position.z;
            }
        }
        updateIconListStatus(PlayerPosition);
        foreach (Transform icon in upcomingWindContainer.transform)
        {
            icon.GetChild(0).gameObject.SetActive(false);
        }
        foreach (UpcomingWindIcon icon in iconList)
        {
            iconDisplayHandler(icon);
            updateUpcomingWindIconPosition(icon);
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
    }
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
    public bool exitAnimFlag;
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