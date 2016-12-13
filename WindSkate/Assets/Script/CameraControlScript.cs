using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Utility;

public class CameraControlScript : MonoBehaviour {

    public int CameraId = 0;
    public bool isIntroScene = false;
    public GameObject rotationControl;
    private GesturePanelScript rotationControlData;
    public List<CameraViewpoints> ViewpointsList = new List<CameraViewpoints>();
    private float CamRotation = 0.0f;

    public SmoothFollow_Fab SmoothFollowData;
    public GameObject CameraTarget;
    public CameraTargetScript CameraTargetData;
    private GameObject referenceCamTargetRefOrient;
    //first Object off the list is tracking the player, second Object is fixed
    public List<GameObject> refRotationControlObjectList;
    private float currentTime;


    public GameObject backgroundImage;
    private Animator backgroundImageAnim;

    private float backgoundImageInitPos;
    private float AnimStatus;
    public float camshiftValue;
    public float camRotShiftValue;
    public float initBackgroundShift;
    //private Vector3 camShift;
    private GameObject playerObject;
    private Follow_track playerFollowTrack;
    private float currentLegOrient;
    // Use this for initialization
    void Start() {
        playerObject = GameObject.Find("Player");
        playerFollowTrack = playerObject.GetComponentInChildren<Follow_track>();
        SmoothFollowData = this.gameObject.GetComponent<SmoothFollow_Fab>();
        CameraTarget = SmoothFollowData.target.gameObject;
        CameraTargetData = CameraTarget.GetComponent<CameraTargetScript>();

        ViewpointsList = new List<CameraViewpoints>();
        if (isIntroScene == false)
        {
            ViewpointsList.Add(new CameraViewpoints("PlayerTrack", true, false, 0, 1, 20));
            ViewpointsList.Add(new CameraViewpoints("TopTracking", true, false, 0, 8, 30));
            ViewpointsList.Add(new CameraViewpoints("TopView", false, true, 0, 8, 30));
        }
        else
        {
            ViewpointsList.Add(new CameraViewpoints("PlayerSideView", false, false, -90, 1, 20));
            ViewpointsList.Add(new CameraViewpoints("TopSideView", false, false, -90, 8, 30));
            ViewpointsList.Add(new CameraViewpoints("FrontRearView", true, false, 180, 0, 20));
            ViewpointsList.Add(new CameraViewpoints("FrontFaceView", true, false, -180, 0, 10));
        }
        refRotationControlObjectList = new List<GameObject>();
        refRotationControlObjectList.Add(CameraTargetData.referenceTransformObjectDirection);
        refRotationControlObjectList.Add(GameObject.Find("Player"));
        setViewpoint(CameraId);
        currentTime = 0;
        if (backgroundImage != null)
        {
            initBackgroundShift = backgroundImage.GetComponent<RectTransform>().transform.position.x;
            backgroundImageAnim = backgroundImage.GetComponent<Animator>();
        }
    }
	
    public void nextViewpoint()
    {
        CameraId = CameraId + 1;
        if (CameraId >= ViewpointsList.Count)
        {
            CameraId = 0;
        }
        setViewpoint(CameraId);
    }

    public void resetViewpoint()
    {
        setViewpoint(CameraId);

    }

    public void setViewpoint (int ViewId)
    {
        CameraTargetData.offsetOrientation = new Vector3(0.0f, ViewpointsList[ViewId].orientBaseAngle, 0.0f);
        SmoothFollowData.distance = ViewpointsList[ViewId].distance;
        SmoothFollowData.height = ViewpointsList[ViewId].height;
        if (ViewpointsList[ViewId].rotationTrack == true)
        {
            CameraTargetData.referenceTransformObjectDirection = refRotationControlObjectList[0];
        }
        else
        {
            CameraTargetData.referenceTransformObjectDirection = refRotationControlObjectList[1];
        }
        //referenceCamTargetRefOrient = ;
    }
    public void updateCurrentPositionOnTrack(float orient)
    {
        currentLegOrient = orient;
        foreach (CameraViewpoints viewpoint in ViewpointsList)
        {
            if (viewpoint.rotationFollowTrack == true)
            {
                viewpoint.orientBaseAngle = orient;
            }
            resetViewpoint();
        }
    }

    public void resetCamPosition()
    {
        CamRotation = 0.0f;
        //this.GetComponent<SmoothFollow>().offsetOrientation = CameraTargetData.offsetOrientation;   
    }

    public void CamShiftTarget (float targetShift)
    {
        CameraTarget.GetComponent<CameraTargetScript>().offsetPosition = new Vector3(targetShift, CameraTarget.GetComponent<CameraTargetScript>().offsetPosition.y, CameraTarget.GetComponent<CameraTargetScript>().offsetPosition.z);
    }

    public void CamRotShiftTarget (float targetrotshift)
    {
        Vector3 camtargetorientvector= CameraTarget.GetComponent<CameraTargetScript>().offsetOrientation;
        CameraTarget.GetComponent<CameraTargetScript>().offsetOrientation = new Vector3(camtargetorientvector.x, targetrotshift, camtargetorientvector.z);
    }

	// Update is called once per frame
	void Update () {

	    if (isIntroScene == true)
        {
            currentTime = currentTime + Time.deltaTime;
            if (currentTime > 10)
            {
                currentTime = 0;
                nextViewpoint();
            }
        }
        if (backgroundImage != null)
        {
            AnimStatus = backgroundImageAnim.GetInteger("OpenPannelStatus");
            camshiftValue = backgroundImage.GetComponent<RectTransform>().transform.position.x- initBackgroundShift;
            CamShiftTarget(-camshiftValue/40.0f);

            /*if (AnimStatus == 1)
            {
                camshiftValue = backgroundImage.GetComponent<RectTransform>().transform.position.x;
                CamShiftTarget(new Vector3(-camshiftValue, 0.0f, 0.0f));
            }
            else
            {
                CamShiftTarget(new Vector3(0.0f, 0.0f, 0.0f));
            }*/
        }

    }
    /*void LateUpdate()
    {
        if (rotationControlData.isMoving == true)
        {
            CamRotation = CamRotation + rotationControlData.CamRotation;
        }
        else
        {
            
        }
    }*/
}
public class CameraViewpoints
{
    public string name;
    public bool rotationTrack;
    public bool rotationFollowTrack;
    public float orientBaseAngle;
    public float height;
    public float distance;

    public CameraViewpoints(string n,bool b, bool rFollow, float a, float h, float d)
    {
        name = n;
        rotationTrack = b;
        rotationFollowTrack = rFollow;
        orientBaseAngle = a;
        height = h;
        distance = d;


    }
}

