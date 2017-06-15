using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour {

	public bool started = false;
	public bool isStartingStatus = true;
	public SailAnimScript sailAnimData;
	public PlayerCollision playerCollisionData;
	public Sail_System_Control sailSystemData;
	public BoardForces boardForcesData;
	static int StartIdleStarboard;
	static int StartIdleStarboard2;
	static int StartTransitionStarboard;
	static int StartIdlePort;
	static int StartIdlePort2;
	static int StartTransitionPort;
	static int currentBaseStateInManoeuvre;

	public float transitionTime = 1;
	public float startBlendValue = 1;


	// Use this for initialization
	void Start () {
		
		sailAnimData = this.gameObject.GetComponentsInChildren<SailAnimScript> () [0];

		playerCollisionData = this.gameObject.GetComponent<PlayerCollision> ();

		sailSystemData = this.gameObject.GetComponentsInChildren<Sail_System_Control> () [0];

		boardForcesData = this.gameObject.GetComponentsInChildren<BoardForces> () [0];

		updateStartParamameters (isStartingStatus);
		//updateStartParamameters (false);

		StartIdleStarboard = Animator.StringToHash("Manoeuvres_Layer.StartIdleStarboard");
		StartIdleStarboard2 = Animator.StringToHash("Manoeuvres_Layer.StartIdleStarboard_Shorter");
		StartTransitionStarboard = Animator.StringToHash("Manoeuvres_Layer.StartTransitionStarboard");
		StartIdlePort = Animator.StringToHash("Manoeuvres_Layer.StartIdlePort");
		StartIdlePort2 = Animator.StringToHash("Manoeuvres_Layer.StartIdlePort_Shorter");
		StartTransitionPort = Animator.StringToHash("Manoeuvres_Layer.StartTransitionPort");
	}

	void updateStartParamameters(bool startbool)
	{
		//sailAnimData.isStarting = startbool;
		playerCollisionData.isStarting = startbool;
		sailSystemData.isStarting = startbool;
		boardForcesData.isStarting = startbool;
		isStartingStatus = startbool;
	}

	public void InitializeStartAfterCrash ()
	{
		updateStartParamameters (true);
		sailAnimData.animSail.SetInteger ("Starting", 1);
		StartCoroutine(startAfterDelay(1f));
	}

	public IEnumerator startAfterDelay(float time)
	{
		float i = 0;
		float rate = 1 / time;
		while (i < 1)
		{
			i += Time.deltaTime * rate;
			yield return 0;
		}
		StartCoroutine(PlayerStartSequence ());
	}

	/// <summary>
	/// Manages the Staring animations. this function must be called when the player has triggered the start (or after crash)
	/// </summary>
	public IEnumerator PlayerStartSequence()
	{

		AnimatorStateInfo currentManState = sailAnimData.currentBaseStateInManoeuvre;
		//first checks the current status of the aniamtion for this player: is it in Start position?
		int startInt = 0;
		Debug.Log ("currentAnim Hash :" + currentManState.fullPathHash);
		int[] animIdleArraw = new int[]{StartIdleStarboard, StartIdleStarboard2, StartIdlePort, StartIdlePort2};
		int idleState = 0;
		foreach (int animState in animIdleArraw) {
			if (currentManState.fullPathHash == animState) {
				startInt = 1;
				break;
			}
			idleState++;
		}
		if ((currentManState.fullPathHash == StartIdleStarboard) || (currentManState.fullPathHash == StartIdlePort)){
			Debug.Log ("Player is in Idle pose");
			startInt = 1;
			//Debug.Log ("start sequence : finished transitions");

		}
		if ((currentManState.fullPathHash != StartIdleStarboard) || (currentManState.fullPathHash == StartIdlePort)){
			Debug.Log ("Player is already starting");
			startInt = 2;
		}
		if (startInt == 0) {
			Debug.Log ("Player is not in Start sequence");
		}
		if (startInt == 1) {
			sailAnimData.animSail.SetTrigger ("Start");

			if (idleState < 2)// is Starboard Position
			{
				yield return StartCoroutine (waitForStartTransitioning(currentManState, StartIdleStarboard, StartTransitionStarboard));
			}
			else // is Port Position
			{
				yield return StartCoroutine (waitForStartTransitioning(currentManState, StartIdlePort, StartTransitionPort));
			}
			Debug.Log ("start sequence : exits states");
			//sailAnimData.exitManoeuvre ();
			updateStartParamameters (false);
			sailAnimData.animSail.SetInteger ("Starting", 0);
		}
	}

	IEnumerator getStartLayerBlend (float time)
	{
		float i = 0;
		float rate = 1 / time;
		while (i < 1)
		{
			i += Time.deltaTime * rate;
			sailAnimData.Manoeuvre_Weight = i;
			Debug.Log("SailAnim Weight : " + i);
			yield return 0;
		}
	}

	/// <summary>
	/// This coroutine waits that the animation states has passed the Start sequences.
	/// </summary>
	/// <returns>The for start transitioning.</returns>
	/// <param name="currentState">Current state.</param>
	/// <param name="startState">Start state.</param>
	/// <param name="targetState">Target state.</param>
	IEnumerator waitForStartTransitioning(AnimatorStateInfo currentState, int startState, int targetState)
	{
		//while ((currentState.fullPathHash == targetState)||(currentState.fullPathHash == startState))
		currentState = sailAnimData.currentBaseStateInManoeuvre;
		while ((currentState.fullPathHash == startState))
		{
			currentState = sailAnimData.currentBaseStateInManoeuvre;
			yield return 0;
		}
		Debug.Log ("test");

		//StartCoroutine (getStartLayerBlend(2));

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			StartCoroutine(PlayerStartSequence ());
		}
		sailAnimData.animSail.SetInteger ("Idle_Selector", Random.Range (0, 2));
	}
}
