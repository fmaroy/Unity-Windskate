using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Follow_track : MonoBehaviour
{
	public GameObject ExternalObjectHandler;
	public UserPreferenceScript userPrefData;
    public int currentMarkId = 0;
    public bool leaveOnStarboard = true;
    private GameObject trackDef;
    public Setup_track trackData;
    public GameObject currentMark;
    private Mark markData;
    private Vector3 distance_to_mark;
    public Vector3 flagMarkPassed;
    public Vector3 flagMarkSide;
    //Door specific
    private int chooseMark = 0;
    private Vector3 flagMarkSide2;
    private Vector3 flagMarkPassed2;
    private Vector3 firstPass;
    private Vector3 finalPass;
    private Vector3 directionToMark;
    public float angleToMark;

    public float distanceWhenPassingMark = 15.0f;
    public Vector3 nextTarget;
    // Next is 0 when the player is targeting a direct mark, 1 when the player is targeting upwind,
    //and 2 when the player is targeting downwind
    public int isNextTargetMark = 0;
    public int previousFrame_isNextTargetMark;
    // Next is defining is a mark is not passed (0), has passed the first validation (1) 
    //or is completely passed (2)
    public int intMarkStatus;
    public int prevIntMarkStatus;
    public GameObject windSource;
    public windEffector windData;
    public float angleMarkToWind;
    public float angleBoardToWind;
    private Vector3 windDirTransform;
    public bool driveStarboard = true;
    public bool previousdriveStarboardStatus;

    private float leftSideBoundaryMax;
    private float rightSideBoundaryMax;

    public bool isPlayer;
    public bool triggerManoeuvre = false;
    public bool localManualDrive;

    //is player = false specific
    private BoardForces boardControlData;
    public List<float> SteerBoundary = new List<float>();
    public float currentSteerBoundary = 15f;
    public float Steerintensity = 1.0f;
    public float currentLeftSideBoundary;
    public float currentRightSideBoundary;
    public float deltaMaxInBound = 30.0f;
    public bool favorOptionRight = true;
    public bool doTackToPort = false;
    public bool doTackToStarboard = false;
    public bool doJibeToPort = false;
    public bool doJibeToStarboard = false;
    public bool prevTackToPort = false;
    private bool prevTackToStarboard = false;
    private bool prevJibeToPort = false;
    private bool prevJibeToStarboard = false;

    public GameObject UpfrontSensor;
    // end of is player = false specific

    //is Player = true specific

    public List<string> markIndicatorName = new List<string>()
    {
    "Mark_rotation_indicator",
    "Mark_rotation_indicator2"
    };
    public List<GameObject> markIndicator;

    public GameObject FinishedImage;
    public GameObject PauseButton;

    public void FinishedTrack(int level2)
    {
        if (this.gameObject.GetComponentInParent<PlayerCollision>().isPlayer == true)
        {
            FinishedImage.SetActive(true);
            PauseButton.SetActive(false);
            Time.timeScale = 0.0f;
        }
    }

    // end of is Player = true specidic

    public GameObject NavAgentObject;
    private UnityEngine.AI.NavMeshPath currentNavMeshPath;
    private Vector3 NavMeshNextDir;
    private Vector3 NavMeshNextCorner;
    public float TargetAngleWithWind;
    public Transform target;
    private UnityEngine.AI.NavMeshHit hit;
    public GameObject DestinationIndicator;
    public GameObject arrivalObject = null;

    // Use this for initialization
    void Start()
    {
		userPrefData = ExternalObjectHandler.GetComponent<ExternalObjectsReference>().UserPrefs;
        SteerBoundary = new List<float>();
        SteerBoundary.Add(5.0f);
        SteerBoundary.Add(15.0f);
        currentSteerBoundary = SteerBoundary[1];
        isPlayer = transform.parent.gameObject.GetComponent<PlayerCollision>().isPlayer;
        localManualDrive = transform.parent.gameObject.GetComponent<PlayerCollision>().ManualDrive;
        //windSource = GameObject.Find("Wind");
        //windSource = this.gameObject;
        windData = windSource.GetComponent<windEffector>();
		FinishedImage = ExternalObjectHandler.GetComponent<ExternalObjectsReference>().FinishImage;
		PauseButton = ExternalObjectHandler.GetComponent<ExternalObjectsReference>().PauseButton;


        trackDef = GameObject.Find("Track_Marks");
        trackData = trackDef.GetComponent<Setup_track>();
        boardControlData = this.GetComponent<BoardForces>();
        currentMark = GameObject.Find(trackData.markSequence[currentMarkId]);
        markData = currentMark.GetComponent<Mark>();

        leftSideBoundaryMax = trackData.BoundaryMin.x;
        rightSideBoundaryMax = trackData.BoundaryMax.x;
        currentLeftSideBoundary = leftSideBoundaryMax;
        currentRightSideBoundary = rightSideBoundaryMax;

        intMarkStatus = 0;

        /// valid if isPlayer = true 
        if (isPlayer == true)
        {
            foreach (string name in markIndicatorName)
            {
                markIndicator.Add(GameObject.Find(name));
                //Debug.Log(GameObject.Find(name).name);
            }

            if (markData.singleMark == true)
            {
                markIndicator[0].SetActive(true);
                /*if (Vector3.Cross(firstPass, finalPass).y < 0)
                {
                    markIndicator[0].transform.GetChild(1).localEulerAngles = new Vector3(90.0f, 180.0f, 0.0f);
                }
                else
                {
                    markIndicator[0].transform.GetChild(1).localEulerAngles = new Vector3(270.0f, 180.0f, 180.0f);
                }*/
                markIndicator[1].SetActive(false);
                markIndicator[0].transform.position = new Vector3(currentMark.transform.position.x, 0.0f, currentMark.transform.position.z);
                
            }
        }
        /// calculation for  isPlayer = true
        if (markData.singleMark == true)
        {
            firstPass = markData.firstPassValidation;
            finalPass = markData.finalPassValidation;
            nextTarget = currentMark.transform.position + (firstPass.normalized * distanceWhenPassingMark);
        }
        else
        {
            finalPass = markData.finalPassValidation;
            if (chooseMark == 0)
            {
                firstPass = (markData.Children[1].transform.position - markData.Children[0].transform.position).normalized;
            }
            else
            {

                firstPass = (markData.Children[0].transform.position - markData.Children[1].transform.position).normalized;
            }
            nextTarget = markData.Children[chooseMark].transform.position + (firstPass * distanceWhenPassingMark);
        }
        
        if (localManualDrive == false)
        {
            //Debug.Log("setNextNav : " + nextTarget);
            //NavAgentObject.GetComponent<NavMeshAgent>().SetDestination(nextTarget);
        }
        previousFrame_isNextTargetMark = isNextTargetMark;

        previousdriveStarboardStatus = driveStarboard;

        prevTackToPort = doTackToPort;
        prevTackToStarboard = doTackToStarboard;
        prevJibeToPort = doJibeToPort;
        prevJibeToStarboard = doJibeToStarboard;
        prevIntMarkStatus = intMarkStatus;

        triggerManoeuvre = false;
        //setNextmark();
        if (isPlayer == false)
        {
            pathRecalculateLogic();
        }

        if ((transform.parent.gameObject.GetComponent<PlayerCollision>().isPlayer == true) && (Camera.main.GetComponent<CameraControlScript>().isIntroScene == false))
        {
            arrivalObject = GameObject.Find("Arrival");
        }
		if ((userPrefData.IntroScene == false)&&(isPlayer == true)) {
			handleTrackIndicator (currentMarkId);
		}
    }

    public void updateManualDriveFollowTrack()
    {
        //Debug.Log("change follow track ManualDrive");
        localManualDrive = this.gameObject.GetComponentInParent<PlayerCollision>().ManualDrive;
        if (localManualDrive == false)
        {
            //Debug.Log("Disable ManualDrive: Auto Mode");
            if (triggerManoeuvre == false)
            {
                //Debug.Log("NavMeshHandling when updating Manual mode");
                NavMeshHandling(isNextTargetMark, driveStarboard, transform.position);
            }
        }
        else
        {
            //Debug.Log("Manual Mode enabled");
            triggerManoeuvre = false;
        }
    }

    public int getRacerStatusOnTrack(float currentAngleToWind)
    {
        int NextTargetStatus = 0;

        float WindShiftModifier = windData.localWindDirection - windData.WindGustsBehavior.initWindOrientation;
        //Debug.Log(WindShiftModifier);
        if (currentAngleToWind > 45 + WindShiftModifier && currentAngleToWind < 135 + WindShiftModifier)
        {
            //Debug.Log("Drive to Mark");
            NextTargetStatus = 0;
            doJibeToPort = false;
            doJibeToStarboard = false;
            doTackToPort = false;
            doTackToStarboard = false;
        }
        else
        {
            if (currentAngleToWind <= 45 + WindShiftModifier)
            {
                //Debug.Log("Drive Upwind");
                NextTargetStatus = 1;
            }
            else
            {
                //Debug.Log("Drive DownWind");
                NextTargetStatus = 2;
            }
        }
        //following test forced opponent to tack/Jibe, based on the chosen side after passing mark
        if (NextTargetStatus == 2 && isNextTargetMark != 2)
        {
            if (driveStarboard == true && favorOptionRight == true)
            {
                doJibeToPort = true;
            }
            if (driveStarboard == false && favorOptionRight == false)
            {
                doJibeToStarboard = true;
            }

        }
        if (NextTargetStatus == 1 && isNextTargetMark != 1)
        {
            if (driveStarboard == true && favorOptionRight == false)
            {
                doTackToPort = true;
            }
            if (driveStarboard == false && favorOptionRight == true)
            {
                doTackToStarboard = true;
            }

        }
        return NextTargetStatus;
    }

    void updateBoundingBox(GameObject nextMarkObject)
    {
        if (nextMarkObject.GetComponent<Mark>().PreviousBoundingBoxOverwrite == false)
        {
            leftSideBoundaryMax = trackData.BoundaryMin.x;
            rightSideBoundaryMax = trackData.BoundaryMax.x;
        }
        else
        {
            leftSideBoundaryMax = nextMarkObject.GetComponent<Mark>().BoundingBoxMin.x;
            rightSideBoundaryMax = nextMarkObject.GetComponent<Mark>().BoundingBoxMax.x;
        }
    }

    Vector3 setNextTarget(GameObject Mark, int MarkId1, Vector3 firstPass1, Vector3 finalPass1)
    {
        //Debug.Log(Mark.name);
        //Debug.Log("MarkId: " + MarkId1);
        Vector3 NextTargetPos = Mark.transform.position;

        Mark currentMarkData = Mark.GetComponent<Mark>();

        updateBoundingBox(Mark);
        if (currentMarkData.isWaypoint == false)
        {
            //Based on the favored side chosen above, the opponent will dirve more on one side of the track
            // MarkId1 == 0 --> must set the definition of the final validation for the mark
            if (MarkId1 < 1)
            {
                if (currentMarkData.singleMark == true)
                {
                    firstPass = firstPass1;
                    finalPass = finalPass1;
                    NextTargetPos = Mark.transform.position + (finalPass.normalized * distanceWhenPassingMark);
                }
                else
                {
                    firstPass = firstPass1;
                    finalPass = finalPass1;
                    if (chooseMark == 0)
                    {
                        NextTargetPos = currentMarkData.Children[chooseMark].transform.position + (finalPass.normalized * distanceWhenPassingMark);
                    }
                    else
                    {
                        NextTargetPos = currentMarkData.Children[chooseMark].transform.position + (finalPass.normalized * distanceWhenPassingMark);
                    }
                }
            }
            else // Must set the objective for first validation of curretn mark (usually, the next one)
            {
                //Debug.Log("Set Objective for next Mark");
                if (Mathf.Round(Random.value) == 0)
                {
                    favorOptionRight = true;
                }
                else
                {
                    favorOptionRight = false;
                }
                if (favorOptionRight == true)
                {
                    currentLeftSideBoundary = leftSideBoundaryMax + (Random.value * deltaMaxInBound / 2);
                    currentRightSideBoundary = rightSideBoundaryMax - (Random.value * deltaMaxInBound);
                }
                else
                {
                    currentLeftSideBoundary = leftSideBoundaryMax + (Random.value * deltaMaxInBound);
                    currentRightSideBoundary = rightSideBoundaryMax - (Random.value * deltaMaxInBound / 2);
                }

                if (currentMarkData.singleMark == true)
                {
                    firstPass = firstPass1;
                    finalPass = finalPass1;
                    NextTargetPos = Mark.transform.position + (firstPass.normalized * distanceWhenPassingMark);
                }
                else
                {
                    Vector3 M0 = markData.Children[0].transform.position;
                    Vector3 M1 = markData.Children[1].transform.position;

                    firstPass = (M0 - M1).normalized;

                    finalPass = finalPass1;

                    if (Mathf.Round(Random.value) == 0)
                    {
                        chooseMark = 0;
                    }
                    else
                    {
                        chooseMark = 1;
                    }
                    if (chooseMark == 0)
                    {
                        NextTargetPos = currentMarkData.Children[chooseMark].transform.position - (firstPass.normalized * distanceWhenPassingMark);
                    }
                    else
                    {
                        NextTargetPos = currentMarkData.Children[chooseMark].transform.position + (firstPass.normalized * distanceWhenPassingMark);
                    }
                }
            }
        }

        //Debug.Log("Define next Objective : " + NextTargetPos);

        return NextTargetPos;
    }

    public void handleTrackIndicator(int id)
    {
        //Debug.Log("refreshing display of track mark : " + id);
        for (int i = 0; i < trackData.markSequence.Count ; i++)
        {
            //Debug.Log("Disabling track sign : " + i);
			if (GameObject.Find(trackData.markSequence[i]).GetComponent<Mark>() != null)
			{ 	//Debug.Log("Mark " + trackData.markSequence[i] + " has componenent Mark : ");
				
            	if (GameObject.Find(trackData.markSequence[i]).GetComponent<Mark>().Children.Count != 0){
                	foreach (GameObject obj in GameObject.Find(trackData.markSequence[i]).GetComponent<Mark>().Children)
                	{
                	    obj.transform.FindChild("Projector").gameObject.SetActive(false);
                	}

            	}
            	else
            	{
					if (GameObject.Find (trackData.markSequence [i]).transform.FindChild ("Projector") != null) {
						GameObject.Find (trackData.markSequence [i]).transform.FindChild ("Projector").gameObject.SetActive (false);
					}

            	}
        	}
		}
		// checks if next mark is a waypoint. If yes, then the next mark is going to be used
		bool flag = false;
		while (flag) {
			if (id <= trackData.markSequence.Count -1){
				if (GameObject.Find (trackData.markSequence [id]).GetComponents<Mark> () != null) {
					id = id + 1;
				} 
				else {
					flag = true;
				}
			}
			else{
				flag = true;
			}
		}
		//Debug.Log ("currentMark Mark Touch Behaviour used " + trackData.markSequence [id]);
		//checks if current mark if is the last one
        if (id != trackData.markSequence.Count -1)
		{	// conditional for waypoint because waypoint don't have a Mark object.
			
				// checks if current mark has children
				if (GameObject.Find (trackData.markSequence [id]).GetComponent<Mark> ().Children.Count != 0) {
					foreach (GameObject obj in GameObject.Find(trackData.markSequence[id]).GetComponent<Mark>().Children) {
						obj.transform.FindChild ("Projector").gameObject.SetActive (true);
					}
				} else {
					GameObject.Find (trackData.markSequence [id]).transform.FindChild ("Projector").gameObject.SetActive (true);
				}
				if (arrivalObject != null) {
					arrivalObject.SetActive (false);
				}
			}
        
        else
        {
            if (arrivalObject != null)
            {
                arrivalObject.SetActive(true);
            }
        }
    }

    int getStatusOnPassingSingleMark(int MarkIndicatorId, GameObject currentNextMark, Vector3 firstPass, Vector3 finalPass)
    {

        Vector3 targetMark = currentNextMark.transform.position;
        //Debug.Log("targetMark : " + targetMark);

        Vector3 pos; 
        if (isPlayer == true)
        {
            pos = transform.position;
        }
        else
        {
            //Debug.Log("Set player to Upfront sensor");
            pos = UpfrontSensor.transform.position;
        }

        Vector3 Vx = pos - targetMark;
        
        Vector3 V1 = firstPass;
        Vector3 V2 = finalPass;
        if (Vector3.Cross(V1, V2).y > 0)
        {
            leaveOnStarboard = true;
        }
        else
        {
            leaveOnStarboard = false;
        }

        float VxV1Indicator = Vector3.Cross(Vx, V1).y;
        float VxV2Indicator = Vector3.Cross(Vx, V2).y;
        if (leaveOnStarboard == true)
        {
            VxV1Indicator = -1 * VxV1Indicator;
            VxV2Indicator = -1 * VxV2Indicator;

        }
        //Debug.Log("Current Pos: " + UpfrontSensor.transform.position);
        //Debug.Log("Vx " + Vx);
        //Debug.Log("current Mark : " + currentMarkId + ", MarkIndicator : "+ MarkIndicator + ", VxV1Indicator : " + VxV1Indicator + ", VxV2Indicator : " + VxV2Indicator);
        // get passing definitions

        if (MarkIndicatorId == 0)
        {
            if (VxV1Indicator > 0 && VxV2Indicator < 0)
            {
                //Debug.Log(currentMarkId + " , " + trackData.markSequence.Count);
                if (currentMarkId >= trackData.markSequence.Count - 1)
                {
                    if (trackData.cyclicTrack == false)
                    {
                        FinishedTrack(0);
                    }
                }
                //Debug.Log("SetSetIndicator");
                nextTarget = setNextTarget(currentNextMark, MarkIndicatorId, firstPass, currentMark.GetComponent<Mark>().finalPassValidation);
                MarkIndicatorId = 1;
                //Debug.Log("Passed First Valid");
            }
            if (VxV1Indicator > 0 && VxV2Indicator > 0)
            {
                MarkIndicatorId = -1;
            }
        }
        if (MarkIndicatorId == -1)
        {
            if (VxV1Indicator < 0 && VxV2Indicator < 0)
            {
                MarkIndicatorId = 0;
            }
        }
        if (MarkIndicatorId == 1)
        {
            if (VxV1Indicator > 0 && VxV2Indicator > 0)
            {
                //Debug.Log(currentMarkId);
                if (currentMarkId >= trackData.markSequence.Count - 1)
                {
                    if (trackData.cyclicTrack == true)
                    {
                        currentMarkId = 0;
                    }
                }
                else
                {
                    currentMarkId = currentMarkId + 1;


                }
                currentMark = GameObject.Find(trackData.markSequence[currentMarkId]);

                //skip waypoint fro the player
                if (currentMark.name.Contains("Waypoint") == true)
                {
                    if (isPlayer == true)
                    {
                        currentMarkId = currentMarkId + 1;
                        currentMark = GameObject.Find(trackData.markSequence[currentMarkId]);
                    }
                    else
                    {
                        List<GameObject> pointObjects = new List<GameObject>();
                        foreach (Transform point in currentMark.transform)
                        {
                            pointObjects.Add(point.gameObject);
                        }
                        currentMark = pointObjects[Random.Range(0, pointObjects.Count)];
                    }
                }

                markData = currentMark.GetComponent<Mark>();
                setNextmark();

                //if (transform.parent.gameObject.GetComponent<PlayerCollision>().isPlayer == true)
				if ((userPrefData.IntroScene == false)&&(isPlayer == true)) {
                    handleTrackIndicator(currentMarkId);
                }
                nextTarget = setNextTarget(currentMark, MarkIndicatorId, markData.firstPassValidation, markData.finalPassValidation);

                MarkIndicatorId = 0;
            }
        }

        //Debug.Log("set MarkIndicator to : " + MarkIndicator);
        return MarkIndicatorId;
    }

    int getStatusOnPassingDoubleMark(int MarkId, GameObject currentNextMark)
    {
        int SelectedMark;

        Vector3 pos ;
        if (isPlayer == true)
        {
            pos = transform.position;
        }
        else
        {
            pos = UpfrontSensor.transform.position;
        }

        //Vector3 targetMark = currentNextMark.transform.position;
        // door marks postions
        Vector3 M0 = markData.Children[0].transform.position;
        Vector3 M1 = markData.Children[1].transform.position;
        //Distance of player to each mark
        Vector3 Vx0 = pos - M0;
        Vector3 Vx1 = pos - M1;

        // get closest Mark 
        float Vx0_Mag_carre = Mathf.Pow(Vx0.x, 2) + Mathf.Pow(Vx0.z, 2);
        float Vx1_Mag_carre = Mathf.Pow(Vx1.x, 2) + Mathf.Pow(Vx1.z, 2);
        //Debug.Log("MarkId Before : " + MarkId);
        //get cloasest mark for singlemark routine

        if (Vx0_Mag_carre < Vx1_Mag_carre)
        {
            SelectedMark = 0;
            //Debug.Log("Closer to Mark : " + SelectedMark + ", FirstPassVector : ");
            //Debug.Log(M0 - M1);
            //Debug.Log("Final Pass Valid : ");
            //Debug.Log(markData.finalPassValidation);
            MarkId = getStatusOnPassingSingleMark(MarkId, markData.Children[SelectedMark], (M1 - M0).normalized, markData.Children[SelectedMark].GetComponent<Mark>().finalPassValidation);
        }
        else
        {
            SelectedMark = 1;
            //Debug.Log("Closer to Mark : " + SelectedMark + ", FirstPassVector : ");
            //Debug.Log(M0 - M1);
            //Debug.Log("Final Pass Valid : ");
            //Debug.Log(markData.finalPassValidation);
            MarkId = getStatusOnPassingSingleMark(MarkId, markData.Children[SelectedMark], (M0 - M1).normalized, markData.Children[SelectedMark].GetComponent<Mark>().finalPassValidation);
        }

        //Debug.Log("MarkId After : " + MarkId);

        //Debug.Log("Closer to Mark : " + SelectedMark);

        return MarkId;
    }

    int getStatusOnPassingWayPoint(GameObject currentNextMark)
    {
        int MarkStatus = 0;
        Vector3 targetMark = currentNextMark.transform.position;
        //Debug.Log("targetMark : " + targetMark);
        Vector3 Vx = transform.position - targetMark;
        if (isPlayer == false)
        {
            Vx = UpfrontSensor.transform.position - targetMark;
        }

        if (Vector3.Cross(Vx, currentNextMark.transform.forward).y < 0)
        {
            //Debug.Log("Waypoint Passed");
            currentMarkId = currentMarkId + 1;

            currentMark = GameObject.Find(trackData.markSequence[currentMarkId]);

            //skip waypoint fro the player
            if (currentMark.name.Contains("Waypoint") == true)
            {
                if (isPlayer == true)
                {
                    currentMark = GameObject.Find(trackData.markSequence[currentMarkId + 1]);
                }
                else
                {
                    List<GameObject> pointObjects = new List<GameObject>();
                    foreach (Transform point in currentMark.transform)
                    {
                        pointObjects.Add(point.gameObject);
                    }
                    currentMark = pointObjects[Random.Range(0, pointObjects.Count)];
                }
            }
            //Debug.Log(currentMark.name);
            markData = currentMark.GetComponent<Mark>();
            setNextmark();
            int MarkIndicatorInt = 1;
            nextTarget = setNextTarget(currentMark, MarkIndicatorInt, markData.firstPassValidation, markData.finalPassValidation);
            //Debug.Log("NavMeshHandling When Passing Waypoint : " + nextTarget);
            NavMeshHandling(isNextTargetMark, driveStarboard, transform.position);
            MarkIndicatorInt = 0;
        }
        return MarkStatus;
    }

    void setNextmark()
    {
        if (isPlayer == true)
        {
            firstPass = markData.firstPassValidation;
            finalPass = markData.finalPassValidation;

            if (markData.singleMark == true)
            {
                //Debug.Log("Placing next single mark indicators");
                markIndicator[0].SetActive(true);
                markIndicator[0].transform.position = new Vector3(currentMark.transform.position.x, 0.0f, currentMark.transform.position.z);
                markIndicator[1].SetActive(false);
                //Update Viewpoint in camera to orient the casmera with the track

            }
            else
            {
                firstPass = markData.Children[0].transform.position - markData.Children[1].transform.position;
                markData.firstPassValidation = firstPass;
                //Debug.Log("Next first pass: " + firstPass);
                //Debug.Log("Next final pass: " + finalPass);
                int iter2 = 0;
                foreach (GameObject doorMark in markData.Children)
                {
                    //Debug.Log("Placing next double mark indicators");
                    markIndicator[iter2].SetActive(true);
                    markIndicator[iter2].transform.position = new Vector3(doorMark.transform.position.x, 0.0f, doorMark.transform.position.z);
                    //Debug.Log("MarkIndicator # " + iter2 + ", set at pos : " + markIndicator[iter2].transform.position);
                    if (iter2 == 0)
                    {
                        Vector3 temp_vector = markData.Children[1].transform.position - markData.Children[0].transform.position;
                    }
                    else
                    {
                        Vector3 temp_vector = markData.Children[0].transform.position - markData.Children[1].transform.position;
                    }
                    iter2++;
                }
            }
            //Debug.Log("Updating Viewpoint");
            updateViewpointOrient(GameObject.Find(trackData.markSequence[currentMarkId]).transform.position, GameObject.Find(trackData.markSequence[currentMarkId - 1]).transform.position);
        }
    }

    void updateViewpointOrient(Vector3 firstMark, Vector3 secondMark)
    {
        GameObject currentCamera = GameObject.Find("Camera Player");
        Vector3 diffVector = firstMark - secondMark;
        float diffAngle = Vector3.Angle(diffVector, Vector3.right);
        if (Vector3.Cross(Vector3.right, diffVector).y < 0)
        {
            diffAngle = -1 * diffAngle;
        }
        //Debug.Log("TrackOrientUpdateAngle : " + diffAngle);
        currentCamera.GetComponent<CameraControlScript>().updateCurrentPositionOnTrack(diffAngle);
    }

    void opponenentTackAndJibeAtBoundary()
    {
        //Debug.Log("Opponent Tack and Jibe at Boundary");
        Vector3 referencePosition = transform.position;
        if (isPlayer == false)
        {
            referencePosition = UpfrontSensor.transform.position;
        }

        if (referencePosition.x > currentRightSideBoundary)
        {
            if (isNextTargetMark == 2)
            {
                //Debug.Log("Trigger jibe to Port");
                doJibeToPort = true;
            }
            if (isNextTargetMark == 1)
            {
                //Debug.Log("Trigger tack to Port");
                doTackToPort = true;
            }
        }
        if (referencePosition.x < currentLeftSideBoundary)
        {
            if (isNextTargetMark == 2)
            {
                //Debug.Log("Trigger jibe to starboard");
                doJibeToStarboard = true;
            }
            if (isNextTargetMark == 1)
            {
                //Debug.Log("Trigger tack to starboard");
                doTackToStarboard = true;
            }
        }
    }

    /// <summary>
    /// Everytime one of those parameter upate, this function must be launched to recalculate the path 
    /// If target status is 1 or 2, then the path should be calculated at 45 deg of the course 
    /// </summary>
    /// <param name="targetSatus"></param>
    /// <param name="driveStarboard"></param>
    public Vector3 NavMeshHandling(int intNextTarget, bool isstarboard, Vector3 playerPosition)
    {
        float angleRecommendedToWind = 45.0f;
        Vector3 nextObjectivePosition = new Vector3(0.0f, 0.0f, 0.0f);
        float angleWind = windData.localWindDirection;
        //Debug.Log("AngleWind : " + angleWind);

        //Debug.Log("NavMeshhandling Function : targetStatus : " + intNextTarget + " is Starboard : " + isstarboard);
        if (intNextTarget == 0)
        {
            //Debug.Log("Setting Navigation to next Target: " + nextTarget);
            nextObjectivePosition = nextTarget;
        }
        if (intNextTarget == 1)
        {
            if (isstarboard == true)
            {
                //Debug.Log("Setting Navigation Upwind on Starboard");
                //Debug.Log(playerPosition.y + rightSideBoundaryMax);
                float side_distance_to_Boundary = rightSideBoundaryMax - playerPosition.x;
                //Debug.Log("side_distance_to_Boundary : " + side_distance_to_Boundary);
                float navDirectionAngle = angleWind - angleRecommendedToWind;
                nextObjectivePosition = new Vector3(rightSideBoundaryMax, 0.0f, playerPosition.z - 1 * Mathf.Tan(navDirectionAngle * Mathf.Deg2Rad) * side_distance_to_Boundary);
            }
            else
            {
                //Debug.Log("Setting Navigation Upwind on Port");
                //Debug.Log(playerPosition.y + leftSideBoundaryMax);
                float side_distance_to_Boundary = leftSideBoundaryMax - playerPosition.x;
                //Debug.Log("side_distance_to_Boundary : " + side_distance_to_Boundary);
                float navDirectionAngle = angleWind + angleRecommendedToWind;
                nextObjectivePosition = new Vector3(leftSideBoundaryMax, 0.0f, playerPosition.z - 1 * Mathf.Tan(navDirectionAngle * Mathf.Deg2Rad) * side_distance_to_Boundary);
            }
        }
        if (intNextTarget == 2)
        {
            if (isstarboard == true)
            {
                //Debug.Log("Setting Navigation Downwind on Starboard");

                float side_distance_to_Boundary = rightSideBoundaryMax - playerPosition.x;
                //Debug.Log("side_distance_to_Boundary : " + side_distance_to_Boundary);
                float navDirectionAngle = angleWind - angleRecommendedToWind;

                nextObjectivePosition = new Vector3(rightSideBoundaryMax, 0.0f, playerPosition.z + Mathf.Tan(navDirectionAngle * Mathf.Deg2Rad) * side_distance_to_Boundary);
                //Debug.Log("nextObjectivePosition calc on Jibe on Starboard: " + nextObjectivePosition);
            }
            else
            {
                //Debug.Log("Setting Navigation Downwind on Port");

                float side_distance_to_Boundary = currentLeftSideBoundary - playerPosition.x;
                //Debug.Log("side_distance_to_Boundary : " + side_distance_to_Boundary);
                float navDirectionAngle = angleWind + angleRecommendedToWind;

                nextObjectivePosition = new Vector3(currentLeftSideBoundary, 0.0f, playerPosition.z + Mathf.Tan(navDirectionAngle * Mathf.Deg2Rad) * side_distance_to_Boundary);
                //Debug.Log("nextObjectivePosition calc on Jibe on Port: " + nextObjectivePosition);
            }
        }
        //Debug.Log("Set Nav to : " + nextObjectivePosition);
        //bool NavTargetValid = NavAgentObject.GetComponent<NavMeshAgent>().SetDestination(nextObjectivePosition);
        bool NavTargetValid = false;
        
        NavTargetValid = NavAgentObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(nextObjectivePosition);

        //previousFrame_isNextTargetMark = isNextTargetMark;
        //previousdriveStarboardStatus = driveStarboard;
        bool blocked = false;
        blocked = UnityEngine.AI.NavMesh.Raycast(transform.position, nextObjectivePosition, out hit, UnityEngine.AI.NavMesh.AllAreas);
        if (isPlayer)
        {
            //Debug.Log("Player " + this.gameObject.transform.parent.gameObject.name + " Navigation set to Destination : " + nextObjectivePosition);
            //Debug.DrawLine(playerPosition, nextObjectivePosition, blocked ? Color.red : Color.green, 3.0f);
        }
        else
        {
            //Debug.Log("Opponent " + this.gameObject.transform.parent.gameObject.name + " Navigation set to Destination : " + nextObjectivePosition);
            //Debug.DrawLine(playerPosition, nextObjectivePosition, blocked ? Color.cyan : Color.blue, 3.0f);
        }

        return nextObjectivePosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isPlayer == false)
        {
            if (other.gameObject.CompareTag("NavMeshDestination"))
            {
                //Checks if the NavmeshDestination box is the one of the current player
                if (transform.parent.gameObject == this.transform.parent.gameObject)
                {
                    //Debug.Log("NavMeshHandling when entered triggered box:: disabled for testing");
                    //NavMeshNextDir = NavMeshHandling(isNextTargetMark, driveStarboard, transform.position);
                }
            }
        }
    }

    public void updatePlayerBoard(Vector3 playerpos)
    {
        if (localManualDrive == true)
        {
            if (markData.singleMark == true)
            {
                distance_to_mark = playerpos - currentMark.transform.position;
            }
            else
            {
                distance_to_mark = playerpos - markData.Children[1].transform.position;
            }
        }
        else
        {
            // for Opponenent nextTarget represent the mark plus the distance to pass the mark
            //distance_to_mark = transform.position - nextTarget;
            //Test with upfront sensor
            
            distance_to_mark = playerpos - nextTarget;
        }

        // 12.9.2016 : test change to fix wrong "angleBoardToWind" that doesn"t behave correctly
        //windDirTransform = new Vector3(-1 * windData.localWindDirectionVector.x, 0.0f, windData.localWindDirectionVector.z);
        windDirTransform = new Vector3(windData.localWindDirectionVector.x, 0.0f, windData.localWindDirectionVector.z);

        angleMarkToWind = Vector3.Angle(distance_to_mark, windDirTransform);
        //Debug.Log("angleMarkToWind : " + angleMarkToWind);

        isNextTargetMark = 0;
        if (angleMarkToWind < 45)
        {
            isNextTargetMark = 1;
        }
        if (angleMarkToWind > 135)
        {
            isNextTargetMark = 2;
        }

        angleBoardToWind = Vector3.Angle(windDirTransform, -1 * transform.forward);

        Vector3 BoardToWindCrossProduct = Vector3.Cross(windDirTransform, -1 * transform.forward);

        if (BoardToWindCrossProduct.y > 0)
        {
            angleBoardToWind = -1 * angleBoardToWind;
            driveStarboard = false;
        }
        else
        {
            driveStarboard = true;
        }
    }

    void Update()
    {
        // get opponent current parameters, distance and angle vs mark.
        if (isPlayer == true)
        {
            updatePlayerBoard(transform.position);
        }
        else
        {
            updatePlayerBoard(UpfrontSensor.transform.position);
        }

        //Debug.Log("isNextTargetMark : " + isNextTargetMark);

        // Set player strategy, vs placement on racetrack

        if (isPlayer == true)
        {
            if (triggerManoeuvre == false)
            {
                if (markData.isWaypoint == false)
                {
                    //getPlayerStatusOnPassingMark();
                    if (markData.singleMark == true)
                    {
                        //Debug.Log("intMarkStatus single before: " + intMarkStatus);
                        intMarkStatus = getStatusOnPassingSingleMark(intMarkStatus, currentMark, currentMark.GetComponent<Mark>().firstPassValidation, currentMark.GetComponent<Mark>().finalPassValidation);
                        //Debug.Log("intMarkStatus single after: " + intMarkStatus);
                    }
                    else
                    {
                        //Debug.Log("intMarkStatus double before: " + intMarkStatus);
                        intMarkStatus = getStatusOnPassingDoubleMark(intMarkStatus, currentMark);
                        //Debug.Log("intMarkStatus double after: " + intMarkStatus);
                    }
                    //Debug.Log("intMarkStatus Step0: " + intMarkStatus);
                }
            }
            //Once the manoeuvre has been initiated, it need to checks when the manoeuvre is over.
            else
            {
                getEndOfManoeuvre();
            }
        }
        else
        {
            opponenentTackAndJibeAtBoundary();
            //Debug.Log("intMarkStatus Step1: " + intMarkStatus);
            if (markData.isWaypoint == false)
            {
                if (markData.singleMark == true)
                {
                    //Debug.Log("intMarkStatus single before: " + intMarkStatus);
                    intMarkStatus = getStatusOnPassingSingleMark(intMarkStatus, currentMark, currentMark.GetComponent<Mark>().firstPassValidation, currentMark.GetComponent<Mark>().finalPassValidation);
                    //Debug.Log("intMarkStatus single after: " + intMarkStatus);
                }
                else
                {
                    //Debug.Log("intMarkStatus double before: " + intMarkStatus);
                    intMarkStatus = getStatusOnPassingDoubleMark(intMarkStatus, currentMark);
                    //Debug.Log("intMarkStatus double after: " + intMarkStatus);
                }
            }
            else
            {
                intMarkStatus = getStatusOnPassingWayPoint(currentMark);
            }

            //Debug.Log("intMarkStatus: " + intMarkStatus + ", prevIntMarkStatus: " + prevIntMarkStatus);
            pathRecalculateLogic();
            
            NavMeshNextCorner = NavAgentObject.GetComponent<UnityEngine.AI.NavMeshAgent>().steeringTarget;

            TargetAngleWithWind = Vector3.Angle(NavMeshNextCorner, windData.localWindDirectionVector);

        }
        if (localManualDrive == true)
        {
            if (Input.GetButtonDown("Jump") == true)
            {
                triggeredManoeuvre();
            }
        }
        if (isPlayer == true)
        {
            DestinationIndicator.transform.position = NavMeshNextDir + new Vector3(0.0f, 3.0f, 0.0f);
        }
        else
        {
            DestinationIndicator.transform.position = NavMeshNextCorner + new Vector3(0.0f, 3.0f, 0.0f);
        }
    }

    public void triggeredManoeuvre()
    {
		int trickLevel = this.gameObject.transform.parent.GetComponent<tricksHandlingScript>().StarsMaxLevel-1;
        bool manoeuvreFlag = false;
		bool jumpFlag = false;

		if ((angleBoardToWind >= 70) & (angleBoardToWind <= 110)) 
		{
			jumpFlag = true;
		}
        if ((angleBoardToWind > 0) & (angleBoardToWind < 70))
        {
            //Debug.Log("Tack to Port");
            doTackToPort = true;
            //prevTackToPort = false;
            manoeuvreFlag = true;
        }
        if ((angleBoardToWind < 0) & (angleBoardToWind > -70))
        {
            //Debug.Log("Tack to Starboard");
            doTackToStarboard = true;
            //prevTackToStarboard = false;
            manoeuvreFlag = true;
        }
        if ((angleBoardToWind > 0) & (angleBoardToWind > 110))
        {
            //Debug.Log("Jibe to Port");
            doJibeToPort = true;
            //prevJibeToPort = false;
            manoeuvreFlag = true;
        }
        if ((angleBoardToWind < 0) & (angleBoardToWind < -110))
        {
            //Debug.Log("Jibe to Starboard");
            doJibeToStarboard = true;
            //prevJibeToStarboard = false;
            manoeuvreFlag = true;
        }
        if (manoeuvreFlag)
        {
            triggerManoeuvre = true;
            this.gameObject.transform.parent.GetComponent<PlayerCollision>().ManualDrive = false;
            this.gameObject.transform.parent.GetComponent<PlayerCollision>().updateManualDrive();
            pathRecalculateLogicTriggeredManoeuvre(0.0f);
            //Debug.Log(this.gameObject.transform.parent.GetComponent<tricksHandlingScript>().StarsMaxLevel);
            if (this.gameObject.transform.parent.GetComponent<tricksHandlingScript>().StarsMaxLevel > 0)
            {
                this.gameObject.transform.parent.GetComponent<tricksHandlingScript>().enableTrick(trickLevel);
            }
        }
    }

    public void controlManoeuvre()
    {
        //checks if the Manoeuvre is finished
    }

    void ManouevreCompleted()
    {
        bool b = this.gameObject.transform.parent.GetComponent<PlayerCollision>().isPlayer;
        bool m = this.gameObject.transform.parent.GetComponent<PlayerCollision>().ManualDrive;
        if (b == true)
        {
            if (m == false)
            {
                // give the control back to the user
                //Debug.Log("Give the control back to the user for player : " + this.gameObject.transform.parent.gameObject.name);
                //Debug.Log("Angle to Wind = " + angleBoardToWind);
                //Debug.Log(isNextTargetMark);
                this.gameObject.transform.parent.GetComponent<PlayerCollision>().ManualDrive = true;
                this.gameObject.transform.parent.GetComponent<PlayerCollision>().updateManualDrive();
                //Debug.Log("this.gameObject.transform.parent.GetComponent<PlayerCollision>().ManualDrive");
                // declare the manoeuvre finished
                //triggerManoeuvre = false;
            }
            // Test moving this one down
            // declare the manoeuvre finished
            triggerManoeuvre = false;
        }
        else
        {
            triggerManoeuvre = false;
        }
        doJibeToStarboard = false;
        doJibeToPort = false;
        doTackToPort = false;
        doTackToStarboard = false;
    }

    public void getEndOfManoeuvre()
    {
        //Debug.Log("End Of Manoeuvre ");
        float endTack = 45.0f;
        float endJibe = 135.0f;
        if (isPlayer == false)
        {
            endTack = 25.0f;
            endJibe = 155.0f;
        }
        
            //int sector = this.gameObject.GetComponentInParent<tricksHandlingScript>().activeSector;
        if (doTackToPort == true && angleBoardToWind <= 0 && angleBoardToWind < -1 * endTack) { ManouevreCompleted(); }
        if (doTackToStarboard == true && angleBoardToWind >= 0 && angleBoardToWind > endTack) { ManouevreCompleted(); }
        if (doJibeToStarboard == true && angleBoardToWind >= 0 && angleBoardToWind < endJibe) { ManouevreCompleted(); }
        if (doJibeToPort == true && angleBoardToWind <= 0 && angleBoardToWind > -1 * endJibe) { ManouevreCompleted(); }

		// This is doone to avoid a special case where the player gets stuck without control
		/*if ((previousFrame_isNextTargetMark != isNextTargetMark) && (isNextTargetMark == 0)) {
			//in this case the state of isNexttargetMark has changed during the manoeuvre to direct direction ot the next mark.
			ManouevreCompleted();
		}*/

    }

    public void pathRecalculateLogicTriggeredManoeuvre(float targetDuration)
    {
        //Debug.Log("Triggered Path Logic");
        bool recalculatePath = false;
        bool drivestarboardforrecalc = false;
        int nexttargetreforcalc = 0;
        
        if (doJibeToPort == true)
        {
            if (prevJibeToPort == false)
            {
                //Debug.Log("recalculate path to Jibe to Port");
                nexttargetreforcalc = 2;
                drivestarboardforrecalc = false;
                recalculatePath = true;
            }
        }
        if (doJibeToStarboard == true)
        {
            if (prevJibeToStarboard == false)
            {
                //Debug.Log("recalculate path to Jibe to Port");
                nexttargetreforcalc = 2;
                drivestarboardforrecalc = true;
                recalculatePath = true;
            }
        }
        if (doTackToPort == true)
        {
            //Debug.Log("Abs AngleBoard = " + Mathf.Abs(angleBoardToWind));
            //Debug.Log("angleBoard = " + angleBoardToWind);
            if (prevTackToPort == false)
            {
                //Debug.Log("recalculate path to Tack to Port");
                nexttargetreforcalc = 1;
                drivestarboardforrecalc = false;
                recalculatePath = true;
            }
        }
        if (doTackToStarboard == true)
        {
            if (prevTackToStarboard == false)
            {
                //Debug.Log("recalculate path to Tack to Port");
                nexttargetreforcalc = 1;
                drivestarboardforrecalc = true;
                recalculatePath = true;
            }
        }

        if (recalculatePath == true)
        {
            if (isPlayer == true)
            {
                //Debug.Log("Calculate Nav : " + nexttargetreforcalc + " , " + drivestarboardforrecalc);
            }
            //Debug.Log("NavMeshDir on triggered Manoeuvre path calculation");
            NavMeshNextDir = NavMeshHandling(nexttargetreforcalc, drivestarboardforrecalc, transform.position);
        }
    }
    void pathRecalculateLogic()
    {
        //Debug.Log("Default Path Logic");
        //Parameters to recalculate the path max only once per frame
        bool recalculatePath = false;
        int nexttargetreforcalc = 0;
        bool drivestarboardforrecalc = false;
        float endTack = 45.0f;
        float endJibe = 135.0f;

        if ((isPlayer == false))
        {
            //Debug.Log("triggerManoeuvre disabled");
            if (previousdriveStarboardStatus != driveStarboard)
            {
                //Debug.Log("driveStarboard changed");
                nexttargetreforcalc = isNextTargetMark;
                //drivestarboardforrecalc = driveStarboard;
                recalculatePath = true;
                //NavMeshHandling(isNextTargetMark, driveStarboard);
            }
            if (previousFrame_isNextTargetMark != isNextTargetMark)
            {
                //Debug.Log("is Next Target changed");
                nexttargetreforcalc = isNextTargetMark;
                drivestarboardforrecalc = driveStarboard;
                //Debug.Log("Recalcualte Logic because of mark changed : " + isNextTargetMark);
                if (isPlayer == false) { recalculatePath = true; }
                ManouevreCompleted();
                //NavMeshHandling(isNextTargetMark, driveStarboard);

            }
            if (prevIntMarkStatus != intMarkStatus)
            {
                //Debug.Log("is IntMarkStatus changed");
                nexttargetreforcalc = isNextTargetMark;
                drivestarboardforrecalc = driveStarboard;
                //Debug.Log("Recalcualte Logic bacause of passing mark chnaged status to : " + isNextTargetMark);
                if (isPlayer == false) { recalculatePath = true; }
                ManouevreCompleted();
                //NavMeshHandling(isNextTargetMark, driveStarboard);

            }
            
        }
        

        if (doJibeToPort == true)
        {
            if (prevJibeToPort == false)
            {
                Debug.Log("recalculate path to Jibe to Port");
                //nexttargetreforcalc = isNextTargetMark;
                //drivestarboardforrecalc = false;
                recalculatePath = true;
            }
            //if (isPlayer == true) { Debug.Log("Jibing to Port towards : " + angleToMark); }

            if ((angleBoardToWind > -1 * endJibe) && (angleBoardToWind < 0))
            {
                //Debug.Log("Jibe finished");
                doJibeToPort = false;
                Debug.Log("recalculate path to Jibe to Port at jibe end");
                //nexttargetreforcalc = isNextTargetMark;
                drivestarboardforrecalc = false;
                if (isPlayer == false) { recalculatePath = true; }
                ManouevreCompleted();
            }
        }
        if (doJibeToStarboard == true)
        {
            if (prevJibeToStarboard == false)
            {
                Debug.Log("recalculate path to jibe to Starboard");
                //nexttargetreforcalc = isNextTargetMark;
                //drivestarboardforrecalc = true;
                recalculatePath = true;
            }
            //Debug.Log("Jibing to Starboard towards : " + angleToMark);
            //if (angleBoardToWind > -55)
            if ((angleBoardToWind < endJibe) && (angleBoardToWind > 0))
            {
                //Debug.Log("Jibe finished");
                doJibeToStarboard = false;
                Debug.Log("recalculate path to Jibe to Starboard at jibe end");
                //nexttargetreforcalc = isNextTargetMark;
                drivestarboardforrecalc = true;
                if (isPlayer == false) { recalculatePath = true; }
                ManouevreCompleted();
            }
        }
        if (doTackToPort == true)
        {
            if (prevTackToPort == false)
            {
                //Debug.Log("recalculate path to Tack to Port");
                //nexttargetreforcalc = isNextTargetMark;
                //drivestarboardforrecalc = false;
                recalculatePath = true;
            }
            //Debug.Log("Tacking to Port towards : " + angleToMark);

            if ((angleBoardToWind < -1 * endTack) && (angleBoardToWind < 0))
            {
                //Debug.Log("Tack finished");
                doTackToPort = false;
                //Debug.Log("recalculate path to Tack to Port at jibe end");
                //nexttargetreforcalc = isNextTargetMark;
                drivestarboardforrecalc = false;
                if (isPlayer == false) { recalculatePath = true; }
                ManouevreCompleted();
            }
        }
        if (doTackToStarboard == true)
        {
            if (prevTackToStarboard == false)
            {
                //Debug.Log("recalculate path to Tack to Starboard");
                //nexttargetreforcalc = isNextTargetMark;
                //drivestarboardforrecalc = true;
                recalculatePath = true;
            }
            //Debug.Log("Jibing to Starboard towards : " + angleToMark);
            if ((angleBoardToWind > endTack) && (angleBoardToWind > 0))
            {
                //Debug.Log("Tack finished");
                doTackToStarboard = false;
                //Debug.Log("recalculate path to Tack to Starboard at jibe end");
                //nexttargetreforcalc = isNextTargetMark;
                drivestarboardforrecalc = true;
                if (isPlayer == false) { recalculatePath = true; }
                ManouevreCompleted();
            }
        }
        if ((doTackToStarboard == true) || (doJibeToStarboard == true))
        {
            drivestarboardforrecalc = true;
        }
        if ((doTackToPort == true) || (doJibeToPort == true))
        {
            drivestarboardforrecalc = false;
        }
        if (isPlayer == true)
        {
            if ((doTackToStarboard == true) || (doTackToPort == true))
            {
                //Debug.Log("Set nexttargetreforcalc to 1, Player driving Upwind");
                nexttargetreforcalc = 1;
                recalculatePath =true;
            }
            if ((doJibeToStarboard == true) || (doJibeToPort == true))
            {
                nexttargetreforcalc = 1;
                //Debug.Log("Set nexttargetreforcalc to 2, Player driving Downwind");
                recalculatePath = true;
            }

        }
        else
        {
            //Debug.Log("TriigerManeoeuvre off");
            nexttargetreforcalc = isNextTargetMark;
        }

        //Debug.Log("recalculatePath : " + recalculatePath);
        if (recalculatePath == true)
        {
            if (isPlayer == true)
            {
                //Debug.Log("Calculate Opponenet Nav : " + nexttargetreforcalc + " , " + drivestarboardforrecalc);
            }
            //Debug.Log("NavMeshDir on regular path calculation");
            NavMeshNextDir = NavMeshHandling(nexttargetreforcalc, drivestarboardforrecalc, transform.position);
        }
        //Debug.Log(NavAgentObject.GetComponent<NavMeshAgent>().steeringTarget);
    }

    void LateUpdate()
    {
        previousFrame_isNextTargetMark = isNextTargetMark;
        prevIntMarkStatus = intMarkStatus;
        previousdriveStarboardStatus = driveStarboard;
        prevTackToPort = doTackToPort;
        prevTackToStarboard = doTackToStarboard;
        prevJibeToPort = doJibeToPort;
        prevJibeToStarboard = doJibeToStarboard;
    }

    void FixedUpdate()
    {
        if ((isPlayer == true )&& (localManualDrive == false))
        {
            //SteerBoard(angleToMark);
            SteerBoard(NavMeshNextDir);
        }
        if (isPlayer == false)
        {
            SteerBoard(NavMeshNextCorner);
        }
    }

    void SteerBoard(Vector3 currentNavMeshDir)
    {
        Vector3 toDirection = currentNavMeshDir - transform.position;
        //GameObject.Find("DirectionTest").transform.position = currentNavMeshDir;
        Vector3 dirCrossProduct = Vector3.Cross(new Vector3(1.0f, 0.0f, 0.0f), toDirection);

        float angletodir = Vector3.Angle(new Vector3(1.0f, 0.0f, 0.0f), toDirection);
        
        if (dirCrossProduct.y < 0)
        {
            angletodir = -1 * angletodir;
        }

        //float angletodir = Vector3.Angle( new Vector3(toDirection.x, 0.0f, toDirection.z), new Vector3(1.0f, 0.0f, 0.0f));
        //Debug.Log("AngleToDir: " + angletodir);
        SteerBoard(angletodir);
    }

    void SteerBoard(float TargetDirection)
    {
        float Delta_steer = Mathf.DeltaAngle(TargetDirection, (transform.eulerAngles.y) - 90);
        //Debug.Log("Delta_steer :" + Delta_steer);

        float steeringOpponent = Delta_steer;

        if (Delta_steer > currentSteerBoundary)
        {
            steeringOpponent = -1 * Steerintensity;
        }
        if (Delta_steer < -1 * currentSteerBoundary)
        {
            steeringOpponent = Steerintensity;
        }
        if (Delta_steer <= currentSteerBoundary && Delta_steer >= -1 * currentSteerBoundary)
        {
            steeringOpponent = -1 * Delta_steer / currentSteerBoundary * Steerintensity;
        }
        boardControlData.rotationToDirection = steeringOpponent;
    }
}
