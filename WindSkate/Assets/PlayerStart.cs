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


		StartIdleStarboard = Animator.StringToHash("Manoeuvres_Layer.StartIdleStarboard");
		StartIdleStarboard2 = Animator.StringToHash("Manoeuvres_Layer.StartIdleStarboard_Shorter");
		StartTransitionStarboard = Animator.StringToHash("Manoeuvres_Layer.StartTransitionStarboard");
		StartIdlePort = Animator.StringToHash("Manoeuvres_Layer.StartIdlePort");
		StartIdlePort2 = Animator.StringToHash("Manoeuvres_Layer.StartIdlePort_Shorter");
		StartTransitionPort = Animator.StringToHash("Manoeuvres_Layer.StartTransitionPort");
		if (this.GetComponent<ExternalObjectsReference> ().raceManagerObject.GetComponent<UserPreferenceScript> ().IntroScene == true) {
			Debug.Log ("start Scene");
			StartCoroutine(startAfterDelay(2));
		}
	}

	void updateStartParamameters(bool startbool)
	{
		//sailAnimData.isStarting = startbool;
		playerCollisionData.isStarting = startbool;
		sailSystemData.isStarting = startbool;
		sailSystemData.isStartingActiveSails = startbool;

		boardForcesData.isStarting = startbool;
		isStartingStatus = startbool;
	}

	public void InitializeStartAfterCrash ()
	{
		Debug.Log ("Initialize after start");
		updateStartParamameters (true);
		sailAnimData.animSail.SetInteger ("Starting", 1);
		StartCoroutine(startAfterDelay(1.0f));
		//StartCoroutine(PlayerStartSequence());
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
		print ("starting");
		StartCoroutine(PlayerStartSequence ());
	}

	/// <summary>
	/// Manages the Staring animations. this function must be called when the player has triggered the start (or after crash)
	/// </summary>
	public IEnumerator PlayerStartSequence()
	{
		Debug.Log ("Start Player Sequence");
		//initialize player ready to start
		// force aniamtion state to Idle

		//sailAnimData.animSail.Play("Manoeuvres_Layer.Idle");
		sailAnimData.animSail.SetInteger ("Starting", 1);


		AnimatorStateInfo  currentManState = sailAnimData.currentBaseStateInManoeuvre;
		//first checks the current status of the aniamtion for this player: is it in Start position?
		int startInt = 0;
		//Debug.Log ("currentAnim Hash :" + currentManState.fullPathHash);
		int[] animIdleArray = new int[]{StartIdleStarboard, StartIdleStarboard2, StartIdlePort, StartIdlePort2};
		int idleState = 0;
		foreach (int animState in animIdleArray) {
			if (currentManState.fullPathHash == animState) {
				startInt = 1;
				break;
			}
			idleState++;
		}

		if ((currentManState.fullPathHash == StartIdleStarboard) || (currentManState.fullPathHash == StartIdlePort)){
			//Debug.Log ("Player is in Idle pose");
			startInt = 1;
			//Debug.Log ("start sequence : finished transitions");

		}
		/*if ((currentManState.fullPathHash != StartIdleStarboard) || (currentManState.fullPathHash != StartIdlePort)){
			Debug.Log ("Player is already starting");
			startInt = 2;
		}*/

		//Debug.Log ("Animstate : " + startInt);

		if (startInt == 0) {
			//Debug.Log ("Player is not in Start sequence");
		}
		if (startInt == 1) {
			//sailAnimData.animSail.SetTrigger ("Start");

			sailAnimData.animSail.SetInteger ("Starting", 2);

			//yield return new WaitForSeconds(1f);



			GameObject pushObj = this.GetComponentInChildren <BoardForces> ().gameObject;
			//StartCoroutine (tempPushPlayer(pushObj, 3.0f));

			//updateStartParamameters (false);

			int[] IdleAnimList = new int[]{ // set list of animations
				StartIdleStarboard,
				StartIdleStarboard2,
				StartTransitionStarboard,
				StartIdlePort,
				StartIdlePort2,
				StartTransitionPort
			};

			//Debug.Log ("start sequence : wait to exist start aniamtions");
			yield return StartCoroutine (waitForStartTransitioning(boardForcesData.gameObject, currentManState, IdleAnimList));

			//Debug.Log ("start sequence : exits states");
			sailAnimData.animSail.SetInteger ("Starting", 0);

			//sailAnimData.exitManoeuvre ();

			//this.GetComponent<PlayerCollision> ().resetRb ();
		}
	}

	IEnumerator tempPushPlayer(GameObject obj, float time)
	{
		float i = 0;
		float rate = 1 / time;
		Rigidbody rb = obj.GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		Vector3 currentPos = obj.transform.position;
		while (i < 1)
		{
			updateStartParamameters (true);
			i += Time.deltaTime * rate;

			obj.transform.position = currentPos + obj.transform.forward * 0.1f;
			// TODO : change into: rigidbody.velocity = new Vector3(1f/Time.fixedDeltaTime,0,0); // no need to change it back into rigidbody

			currentPos = obj.transform.position;

			yield return 0;
		}
		updateStartParamameters (false);
		rb.isKinematic = false;
	}

	IEnumerator getStartLayerBlend (float time)
	{
		float i = 0;
		float rate = 1 / time;
		while (i < 1)
		{
			i += Time.deltaTime * rate;
			sailAnimData.Manoeuvre_Weight = i;
			//Debug.Log("SailAnim Weight : " + i);
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
	IEnumerator waitForStartTransitioning(GameObject obj, AnimatorStateInfo currentState, int[] stateList)
	{
		
		//while ((currentState.fullPathHash == targetState)||(currentState.fullPathHash == startState))
		currentState = sailAnimData.currentBaseStateInManoeuvre;
		Rigidbody rb = obj.GetComponent<Rigidbody> ();
		rb.isKinematic = true;
		Vector3 currentPos = obj.transform.position;
		bool b = true;
		while ((b))
		{
			//Debug.Log ("In the loop!");
			b = false;
			currentState = sailAnimData.currentBaseStateInManoeuvre; // updates the current state
			foreach (int i in stateList) { // Verifies if the current state of the animation is in the provided list
				if (currentState.fullPathHash == i) {
					b = true;
				}
			}

			if (sailAnimData.animationDisplacement > 0.0) {
				updateStartParamameters (true);
				rb.isKinematic = false;
				sailSystemData.isStartingActiveSails = true;
				//Debug.Log ("Reading Motion value : " + sailAnimData.animationDisplacement);
				rb.velocity = obj.transform.forward * sailAnimData.animationDisplacement;
				//obj.transform.position = currentPos + obj.transform.forward * sailAnimData.animationDisplacement * 10; //carefull, the motion is apply in gloabl Z, check if this will not cause problems!!!!
			} else {
				//Debug.Log ("No Anim Deplacement");
				updateStartParamameters (false);
				// disables the manoueuvre slow down in SailSystem_Contols script
				sailSystemData.isStartingActiveSails = true;
				rb.isKinematic = false;
			}
			currentPos = obj.transform.position;

			yield return 0;
		}

		//Debug.Log ("Out of the loop!");
		updateStartParamameters (false);
		rb.isKinematic = false;
		//Debug.Log ("test");

		//StartCoroutine (getStartLayerBlend(2));

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			StartCoroutine(PlayerStartSequence ());
			//PlayerStartSequence ();
		}
		sailAnimData.animSail.SetInteger ("Idle_Selector", Random.Range (0, 2));
	}
}
