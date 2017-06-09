using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackIndicatorMotion : MonoBehaviour {

	// this script is derived from FollowTrack to move an indicator along the track race
	public RaceManagerScript RaceManagerData;
	public GameObject Player;
	public GameObject TraceObject;
	public GameObject cameraTarget;
	public float cameraDistance = 120f;
	public float cameraHeight = 100f;
	public float trailSpeed = 5;
	private float trailPace;
	private GameObject trackDef;
	public Setup_track trackData;
	public GameObject currentMark;
	private Mark markData;
	public int currentMarkId = 0; // int that indicate mark number in the sequence
	public int markIndicator = 0; // int that indicate if a mark has been passed
	private float leftSideBoundaryMax;
	private float rightSideBoundaryMax;
	public float currentLeftSideBoundary;
	public float currentRightSideBoundary;
	private Vector3 flagMarkSide2;
	private Vector3 flagMarkPassed2;
	private Vector3 firstPass;
	private Vector3 finalPass;
	private Vector3 nextTarget;
	public float distanceWhenPassingMark;
	private int chooseMark = 0;
	public GameObject debugNexttargetIndicator;
	private float timer;
	private Vector3 initPos;
	private Vector3 targetPos;
	private bool favorOptionRight;
	public float deltaMaxInBound = 30.0f;
	private WindGustsBehavior WindData; 
	public bool goingStarBoard = false;
	public bool goingUpwind = true;
	public GameObject initCamDirObj;
	public GameObject initCamPosObj;

	// Use this for initialization
	void Start () {
		RaceManagerData = GameObject.Find ("RaceManager").GetComponent<RaceManagerScript> ();
		WindData = GameObject.Find ("WindGusts").GetComponent<WindGustsBehavior>();
		Player = RaceManagerData.PlayerObject;
		trackDef = GameObject.Find("Track_Marks");
		trackData = trackDef.GetComponent<Setup_track>();
		currentMark = GameObject.Find(trackData.markSequence[currentMarkId]);
		markData = currentMark.GetComponent<Mark>();

		leftSideBoundaryMax = trackData.BoundaryMin.x;
		rightSideBoundaryMax = trackData.BoundaryMax.x;
		currentLeftSideBoundary = leftSideBoundaryMax;
		currentRightSideBoundary = rightSideBoundaryMax;

		TraceObject.transform.position = Player.transform.position;
		timer = 0;

		favorOptionRight = true;
	}

	public void SetNext()
	{
		if (markIndicator == 0) {
			markIndicator++;
		} else {
			currentMarkId++;
			markIndicator = 0;
			chooseMark = Mathf.RoundToInt (Random.value); // picks a value between 0 and 1 to choose a door to pass
		}
	}

	public void getMark ()
	{
		SetTrackBoundaries ();
		currentMark = GameObject.Find(trackData.markSequence[currentMarkId]);
		markData = currentMark.GetComponent<Mark>();
		nextTarget = getNextTarget (currentMarkId, markIndicator);
		Debug.Log ("Trace Next Target : " + nextTarget);
		Debug.Log ("MarkIndicatorValue : " + markIndicator);
		//yield return StartCoroutine(moveToTarget (TraceObject, TraceObject.transform.position, nextTarget, trailPace));
		initPos = TraceObject.transform.position;
		targetPos = setNextTarget (goingStarBoard); // TODO: targetPos will need to change when going downwind and upwind
		initPath ();
	}

	public Vector3 setNextTarget (bool goStarBoard)
	{
		goingStarBoard = goStarBoard;
		Vector3 target = nextTarget;
		Vector3 currentPos = TraceObject.transform.position;
		Vector3 dirVector = nextTarget - currentPos;
		Quaternion rot = Quaternion.AngleAxis (WindData.initWindOrientation, Vector3.up);
		Vector3 dirWind = rot * Vector3.left;
		Debug.Log ("WindVector : " + dirWind);

		float dirAngleToWind = Vector3.Angle (dirVector,-1*dirWind);
		Debug.Log ("Angle to wind : " + dirAngleToWind);

		if (Mathf.Abs (dirAngleToWind) < 45) {
			goingUpwind = true;
			target = followWind (goStarBoard, goingUpwind);
		} 
		if (Mathf.Abs (dirAngleToWind) > 135){
			goingUpwind = false;
			target = followWind (goStarBoard, goingUpwind);
		}
		if ((Mathf.Abs (dirAngleToWind) >= 45) && (Mathf.Abs (dirAngleToWind) <= 135)) {
			target = goDirectlyToMark ();
		}

		return target;
	}

	void initPath()
	{
		initPos = TraceObject.transform.position;
		TraceObject.transform.forward = targetPos - TraceObject.transform.position ;
		trailPace = trailSpeed / (new Vector2 (targetPos.x - initPos.x , targetPos.z - initPos.z).magnitude);
		timer = 0;
	}

	Vector3 goDirectlyToMark()
	{
		Vector3 target = nextTarget;

		return target;
	}

	Vector3 followWind(bool starboardside, bool goUpWind)
	{
		Vector3 target = Vector3.zero;
		Quaternion rot = Quaternion.AngleAxis (WindData.initWindOrientation, Vector3.up);
		Vector3 dirWind = rot * Vector3.left;

		if (goUpWind) {
			Debug.Log ("is Upwind");
			goingUpwind = true;
			if (starboardside) {
				rot = Quaternion.AngleAxis (45, Vector3.up);
				Debug.Log (rot * Vector3.forward);
				target = rot * (-1 * dirWind) * 1000;
			} else {
				rot = Quaternion.AngleAxis (-45, Vector3.up);
				Debug.Log (rot * Vector3.forward);
				target = rot * (-1 * dirWind) * 1000;
			}
		}
		else {
			Debug.Log ("is Downwind");
			goingUpwind = false;
			if (starboardside) {
				Debug.Log (starboardside);
				rot = Quaternion.AngleAxis (45, Vector3.up);
				Debug.Log (rot * Vector3.forward);
				target = rot * dirWind * 1000;

			} else {
				Debug.Log (starboardside);
				rot = Quaternion.AngleAxis (-45, Vector3.up);
				Debug.Log (rot * Vector3.forward);
				target = rot * dirWind * 1000;
			}
		}
		return target;
	}

	void tackjibHandler()
	{
		Vector3 target = nextTarget;
		Vector3 initDirVector = nextTarget - initPos;
		Vector3 markDir = nextTarget - TraceObject.transform.position;
		float angleDiffTraceToMark = Vector3.Angle (TraceObject.transform.forward, markDir);
		Debug.Log ("WindVector for Jibe/Tack : " + angleDiffTraceToMark);
		if ((Mathf.Abs (angleDiffTraceToMark) > 90)) {
			Debug.Log ("Go to mark directly");
			initPos = TraceObject.transform.position;
			targetPos = goDirectlyToMark ();
			initPath ();
		}

		if (goingUpwind) {
			if (((TraceObject.transform.position.x < currentLeftSideBoundary) && (goingStarBoard)) || ((TraceObject.transform.position.x > currentRightSideBoundary) && (!goingStarBoard))) {
				Debug.Log ("found boundary");
				initPos = TraceObject.transform.position;
				targetPos = setNextTarget (!goingStarBoard);
				initPath ();
			}
		} 
		else {
			if (((TraceObject.transform.position.x < currentLeftSideBoundary) && (!goingStarBoard)) || ((TraceObject.transform.position.x > currentRightSideBoundary) && (goingStarBoard))) {
				Debug.Log ("found boundary");
				initPos = TraceObject.transform.position;
				targetPos = setNextTarget (!goingStarBoard);
				initPath ();
			}
		}
	}

	public void SetTrackBoundaries()
	{
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
	}

	public void interruptTraceIntro()
	{
		currentMarkId = Player.GetComponentInChildren<Follow_track> ().trackData.markSequence.Count +1;
		markIndicator = 2;
		Camera.main.GetComponent<CameraControlScript> ().CameraTargetData.referenceTransformObjectPosition = initCamPosObj;
		this.GetComponent<IntroSequence>().introductionManager (2);
		TraceObject.SetActive (false);
	}

	public void initTrackTrace ()
	{
		initCamPosObj = Camera.main.GetComponent<CameraControlScript> ().CameraTargetData.referenceTransformObjectPosition; // captures the previous camera reference for orientation for restoring the settings 
		Camera.main.GetComponent<CameraControlScript> ().CameraTargetData.referenceTransformObjectPosition = cameraTarget;
		//Camera.main.GetComponent<CameraControlScript> ().CameraTargetData.referenceTransformObjectPosition = cameraTarget;
		Camera.main.GetComponent<SmoothFollow_Fab> ().distance = cameraDistance;
		Camera.main.GetComponent<SmoothFollow_Fab> ().height = cameraHeight;
		timer = 0f;
		currentMarkId = 0;
		markIndicator = 0;
		TraceObject.transform.position = Player.transform.position;
		getMark ();
		TraceObject.SetActive (true);
	}

	public Vector3 getNextTarget(int mark, int statusId)
	{
		Vector3 target = Vector3.zero;
		GameObject thismark = GameObject.Find(trackData.markSequence[mark]);
		Mark thismarkData = thismark.GetComponent<Mark>();
		Vector3 pass1 = Vector3.zero;
		Vector3 pass2 = Vector3.zero;
		if (thismarkData.singleMark == true) {
			pass1 = thismarkData.firstPassValidation;
			pass2 = thismarkData.finalPassValidation;
			if (statusId == 0) {
				target = currentMark.transform.position + (pass1.normalized * distanceWhenPassingMark);
			} 
			else {
				target = currentMark.transform.position + (pass2.normalized * distanceWhenPassingMark);
			}
		}
		else {
			pass2 = thismarkData.finalPassValidation;
			if (chooseMark == 0) {
				pass1 = (thismarkData.Children [1].transform.position - thismarkData.Children [0].transform.position).normalized;
			} else {
				pass1 = (thismarkData.Children [0].transform.position - thismarkData.Children [1].transform.position).normalized;
			}
			if (statusId == 0) {
				target = thismarkData.Children [chooseMark].transform.position + (pass1 * distanceWhenPassingMark);
			}
			else {
				target = thismarkData.Children [chooseMark].transform.position + (pass2 * distanceWhenPassingMark);
			}
		}
		return target;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.C)) {
			initTrackTrace ();
		}

		//handles the mark indicators
		if ((currentMarkId >= Player.GetComponentInChildren<Follow_track> ().trackData.markSequence.Count - 1) && (markIndicator > 0)) { 
			Camera.main.GetComponent<CameraControlScript> ().CameraTargetData.referenceTransformObjectPosition = initCamPosObj;
			TraceObject.SetActive (false);
			StartCoroutine(this.GetComponent<IntroSequence>().introductionManager (2)); // track finished
		} 
		else {
			Debug.Log (currentMarkId);
			Player.GetComponentInChildren<Follow_track> ().handleTrackIndicator (currentMarkId);
			cameraTarget.transform.position = new Vector3 (0f, 0f, TraceObject.transform.position.z);

			// Test to replace the Coroutine mechanism
			if (currentMarkId < trackData.markSequence.Count) {
				if (timer < 1.0f) {
					timer += Time.deltaTime * trailPace;
					float xPos = Mathf.Lerp (initPos.x, targetPos.x, timer);
					float zPos = Mathf.Lerp (initPos.z, targetPos.z, timer);
					//Debug.Log ("Lerp Value : " + timer + ", DeltaTimeValue : " + Time.deltaTime * trailPace);
					TraceObject.transform.position = new Vector3 (xPos, 1f, zPos);
				} else {
					Debug.Log ("Reached destination");
					if (targetPos == nextTarget) {
						SetNext ();
						getMark ();
					} else {
						initPos = TraceObject.transform.position;
						targetPos = setNextTarget (!goingStarBoard);
						initPath ();
					}
				}
			}
			//debugNexttargetIndicator.transform.position = nextTarget;

			tackjibHandler ();
		}
	}
}
