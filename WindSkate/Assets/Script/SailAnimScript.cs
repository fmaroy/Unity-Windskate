using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SailAnimScript : MonoBehaviour
{
    public Animator animSail;

    private float SailAngleAnim;
    Sail_System_Control SailControlComponent;

    static int ManoeuvreIdleState;
    static int StarboardStateTenDeg;
    static int PortStateTenDeg;
    static int PortStateNinetyDeg;
    static int StarboardStateNinetyDeg;
	static int StartTransitionStarboard;
	static int StartTransitionPort;



    public float WindLayerWeight = 50.0f;
    //private Sail_System_Control SailControlComponent;
    private float SailAngle;
    public bool isManoeuvreTransitioning = false;
    public bool isManoeuvreing = false;
    private bool previsManoeuvreing = false;
    public float timerManoeuvreTarget = 1.0f;
    private float timerManoeuvreCurrent = 0.0f;
    private float targetLayerValue = 1.0f;
    private float initLayerValue = 1.0f;

    // next is 0 if no manoeuvre, 1 if basic tacking
    public float Manoeuvre_Weight = 0.0f;
    public int intManoeuvreState = 0;
    public AnimatorStateInfo currentBaseStateInManoeuvre;
    private AnimatorStateInfo previousBaseStateInManoeuvre;

    public float minRange = 10.0f;
    public float maxRange = 40.0f;

    public int Manoeuvre_level = 0;
    private float initCamRotationDamping ;
    private float initCamDistance;
    private bool ManoeuvreFXGoingOn = false;
    private float ManoeuvreFXtimer;
    private float ManoeuvreFXtimerTarget = 2.0f;
    private CameraControlScript CamControlData;

	public int inProgressManoeuverLevel = 0;

	public bool isStarting;
	public Vector3 animatorRootMotion;
	public float animationDisplacement;

    // Use this for initialization
    void Start()
    {
        animSail = GetComponent<Animator>();
        ManoeuvreIdleState = Animator.StringToHash("Manoeuvres_Layer.Idle");
        StarboardStateTenDeg = Animator.StringToHash("Manoeuvres_Layer.10degStarboard");
        PortStateTenDeg = Animator.StringToHash("Manoeuvres_Layer.10degPort");
        PortStateNinetyDeg = Animator.StringToHash("Manoeuvres_Layer.FromPort90deg");
        StarboardStateNinetyDeg = Animator.StringToHash("Manoeuvres_Layer.FromStarboard90deg");


        if (animSail.layerCount >= 2)
        {
            animSail.SetLayerWeight(1, 1);
        }
        CamControlData = Camera.main.GetComponent<CameraControlScript>();
        SailAngle = gameObject.GetComponent<Sail_System_Control>().apparentWindAngleLocal;

        currentBaseStateInManoeuvre = animSail.GetCurrentAnimatorStateInfo(2);
        previousBaseStateInManoeuvre = currentBaseStateInManoeuvre;
        isManoeuvreTransitioning = false;
        isManoeuvreing = false;
        previsManoeuvreing = false;

        ManoeuvreFXtimer = ManoeuvreFXtimerTarget;
		ManoeuvreFXGoingOn = false;

		//isStarting = true;
    }
	public void OnAnimatorMove() {
		//Animator animator = GetComponent<Animator>();
		if (animSail) {
			float temp = animSail.GetFloat("Runspeed") * Time.deltaTime;

			//Debug.Log ("Runspeed :" + temp);
			if (temp > 0) {
				animationDisplacement = temp;
				//Debug.Log ("Runspeed :" + animationDisplacement);
			} else {
				animationDisplacement = 0.0f;
			}
				
			/*Vector3 newPosition = transform.position;
			newPosition.z += animator.GetFloat("Runspeed") * Time.deltaTime;

			Debug.Log ("Motion Z : " + newPosition);
			//transform.position = newPosition;
			animatorRootMotion = newPosition;*/
		}
	}

    void LateUpdate()
    {
        // recording the state of the manoeuvring flag to detect if it is changed
        previsManoeuvreing = isManoeuvreing;
        //Debug.Log("assigning the value of is Manoeuvre to prevIsManoeuvre at the end of Frame rendering : isManoeuvreing: " + isManoeuvreing + ", previsManoeuvreing: " + previsManoeuvreing);
    }

    public void exitManoeuvre()
    {
        //Debug.Log("Exit Manoeuvre, initWeight " + animSail.GetLayerWeight(2));
        isManoeuvreTransitioning = true;
        timerManoeuvreCurrent = 0.0f;
        targetLayerValue = 0.0f;
        initLayerValue = animSail.GetLayerWeight(2);
        Manoeuvre_level = 0;
    }

    public void enterManoeuvre()
    {
        isManoeuvreTransitioning = true;
        timerManoeuvreCurrent = 0.0f;
        targetLayerValue = 1.0f;
        initLayerValue = animSail.GetLayerWeight(2);
    }

    public void StartManoeuvreFX(float SlowMotionFactor)
    {
        ManoeuvreFXGoingOn = true;
        
        Time.timeScale = SlowMotionFactor;
        Time.fixedDeltaTime = 0.01F * Time.timeScale;
        CamControlData.SmoothFollowData.rotationDamping = 0.5f;
        CamControlData.SmoothFollowData.heightDamping = 0.5f;
        CamControlData.CameraTargetData.offsetOrientation = new Vector3(0.0f, Mathf.LerpAngle(-120, 120, Random.value), 0.0f);
        CamControlData.SmoothFollowData.height = Mathf.Lerp(0f, 15.0f, Random.value);
        CamControlData.SmoothFollowData.distance = Mathf.Lerp(8.0f, 20.0f, Random.value);
    }

    public void EndManoeuvreFX()
    {
        if (ManoeuvreFXGoingOn == true)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.01F * Time.timeScale;
            CamControlData.SmoothFollowData.rotationDamping = CamControlData.initCamRotationDamping;
            CamControlData.SmoothFollowData.heightDamping = CamControlData.initCamHeightDamping;
            if (CamControlData.ViewpointsList[CamControlData.CameraId].rotationFollowTrack == false)
            {
                //Camera is not aligned with the track
                CamControlData.CameraTargetData.offsetOrientation = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                //Camera is aligned with the track, we need ot make sure the orientation is reapplied correctly
                CamControlData.CameraTargetData.offsetOrientation = new Vector3(0.0f, CamControlData.ViewpointsList[CamControlData.CameraId].orientBaseAngle, 0.0f);
            }
            
            CamControlData.SmoothFollowData.height = CamControlData.ViewpointsList[CamControlData.CameraId].height;
            CamControlData.SmoothFollowData.distance = CamControlData.ViewpointsList[CamControlData.CameraId].distance;
        }

        ManoeuvreFXGoingOn = false;
    }


    // Update is called once per frame
    void Update()
    {
        animSail.SetInteger("Manoeuver_Level", Manoeuvre_level);
		/*if (isStarting == true) {
			animSail.SetInteger ("Starting", 1);
		} else {
			animSail.SetInteger ("Starting", 0);
		}*/
        SailAngleAnim = 0.0f;
        SailAngle = gameObject.GetComponent<Sail_System_Control>().apparentWindAngleLocal;
        float TrueWind = gameObject.GetComponent<Sail_System_Control>().trueWindAngleLocal;
        //Debug.Log(TrueWind);
        previousBaseStateInManoeuvre = currentBaseStateInManoeuvre;
        currentBaseStateInManoeuvre = animSail.GetCurrentAnimatorStateInfo(2);

        SailAngleAnim = SailAngle / 180;

        // to do: apparent wind force impact on animation layer too low.

        float LightWind_Weight = 1.0f - Mathf.InverseLerp(minRange, maxRange, gameObject.GetComponent<Sail_System_Control>().Board_Speed);
	 	
		if (LightWind_Weight < 0.0f)
        {
            LightWind_Weight = 0.0f;
        }
        else if (LightWind_Weight > 1.0f)
        {
            LightWind_Weight = 1.0f;
        }

        float HeavyWind_Weight = 1.0f - LightWind_Weight;


        Manoeuvre_Weight = 0.0f;

        if ((TrueWind < 20) || (TrueWind > 170))
        {
            Manoeuvre_Weight = 1.0f;
            LightWind_Weight = 0.0f;
            HeavyWind_Weight = 0.0f;
        }
        else
        {
            Manoeuvre_Weight = 0.0f;
        }
        if ((TrueWind <= 30) && (TrueWind >= 20))
        {
            //Debug.Log((30 - TrueWind) / 10);
            Manoeuvre_Weight = (30 - TrueWind) / 10;
        }
        if ((TrueWind >= 160) && (TrueWind <= 170))
        {
            Manoeuvre_Weight = (TrueWind - 160) / 10;
        }
			
		if ((currentBaseStateInManoeuvre.fullPathHash != ManoeuvreIdleState) && (currentBaseStateInManoeuvre.fullPathHash != StarboardStateTenDeg) && (currentBaseStateInManoeuvre.fullPathHash != PortStateTenDeg) && (currentBaseStateInManoeuvre.fullPathHash != StarboardStateNinetyDeg) && (currentBaseStateInManoeuvre.fullPathHash != PortStateNinetyDeg)) {
			//current state is not in the idle spot startboard or port 10 deg nor 90 Deg

			Manoeuvre_Weight = 1.0f;
			intManoeuvreState = 1;
		} else {

			intManoeuvreState = 0;
			if ((previousBaseStateInManoeuvre.fullPathHash != ManoeuvreIdleState) && (previousBaseStateInManoeuvre.fullPathHash != StarboardStateTenDeg) && (previousBaseStateInManoeuvre.fullPathHash != PortStateTenDeg) && (previousBaseStateInManoeuvre.fullPathHash != StarboardStateNinetyDeg) && (previousBaseStateInManoeuvre.fullPathHash != PortStateNinetyDeg)) {
				exitManoeuvre ();
			}
		}

		if (isManoeuvreTransitioning == true) {
			if (timerManoeuvreCurrent < timerManoeuvreTarget) {
				timerManoeuvreCurrent = timerManoeuvreCurrent + Time.deltaTime;
				//timerManoeuvreCurrent / timerManoeuvreTarget varies from 1 to 0
				// initLayerValue - targetLayerValue varies from 1 to 0 when exiting and from 1 to 0 when entering
				Manoeuvre_Weight = Mathf.Lerp (initLayerValue, targetLayerValue, timerManoeuvreCurrent / timerManoeuvreTarget);
				//Debug.Log("CountDown : " + timerManoeuvreCurrent + "Weight " + Manoeuvre_Weight);
			}
		}

		/*if (isStarting == true) {
			Manoeuvre_Weight = 1.0f;
		}*/
			//Debug.Log ("SailAngleAnim : " + SailAngle + ", LightWind_Weight: "+ LightWind_Weight + " ,HeavyWind_Weight : " + HeavyWind_Weight);

			//Debug.Log("LightWindLayerWeight : " + LightWind_Weight);
			animSail.SetLayerWeight (0, LightWind_Weight);
			animSail.SetLayerWeight (1, HeavyWind_Weight);
			animSail.SetLayerWeight (2, Manoeuvre_Weight);

			animSail.SetFloat ("Velocity", this.gameObject.GetComponent<Sail_System_Control> ().Board_Speed);

			if (this.gameObject.GetComponent<Sail_System_Control> ().sailTiltDir.y > 0) {
				animSail.SetFloat ("SailAngle", TrueWind);

			} else {
				animSail.SetFloat ("SailAngle", -1 * TrueWind);
			}
			if (Manoeuvre_Weight < 1.0f) {
				if (this.gameObject.GetComponent<Sail_System_Control> ().sailTiltDir.y > 0) {
					animSail.Play ("LightWind_Layer.LightWindStarboard", 0, SailAngleAnim);
					animSail.Play ("HeavyWind_Layer.HeavyWindStarboard", 1, SailAngleAnim);
				} else {
					animSail.Play ("LightWind_Layer.LightWindPort", 0, SailAngleAnim);
					animSail.Play ("HeavyWind_Layer.HeavyWindPort", 1, SailAngleAnim);
				}
			}
			if (Manoeuvre_Weight != 0) {
				isManoeuvreing = true;
				// This is applied when no special manoeuvre is used. If a special manoeuvre is used, this gets overridden later on
				// Not needed anymore, should be handled by the method Sail_System_Control.manoeuvreThrustModifier() that include ow this special case

			} else {
				isManoeuvreing = false;
			}
		if (previsManoeuvreing != isManoeuvreing) {
			if (this.gameObject.transform.parent.gameObject.GetComponent<PlayerCollision> ().isPlayer == true) {
				if (isManoeuvreing == true) {
					if (Manoeuvre_level > 0) {
						// TODO: based on the level of the manoeuvre chance that a special animation started get higher:
						if (Random.value / Manoeuvre_level < 0.5) {
							StartManoeuvreFX (0.5f);
						}
					}
				} else {
					EndManoeuvreFX ();
				}
			}
		}
    }
}

