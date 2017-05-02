using UnityEngine;
using System.Collections;

public class Sail_System_Control : MonoBehaviour 
	{
	public GameObject externalObjectsHandler;
	public UserPreferenceScript userPrefs;
	public GameObject raceManager;
	public GameObject windSource;
	private windEffector windData;
	private Vector3 windDir;
	private Vector3 windForce;
	private Vector3 Sail_Rotation;
	public Vector3 apparentWind;
	private float apparentWindAngle;
	public float apparentWindAngleLocal;
	public float trueWindAngleLocal;
    private float sailDrag_rearwind = -2;
    public Vector3 sailTiltDir;
	private float braquingAngle;
	public float SetSailAngle;
	public GameObject player;
	public GameObject sailGeom;
	public SkinnedMeshRenderer SailGeomSkinnedMesh;
	public Rigidbody rbBoard;
	
	public float sailThrust_factor = 4f;
	public float sailDrag_rearwind_factor = 4f;
	public float sailDrag_factor = 0.5f;
    public float boardDrag = 1.0f;
	public float linearBoardDrag_factor= 0.05f;

	private float ApparentWindMagnitude;

	public float Board_Speed;
    public GameObject SailBone;
    public Rigidbody SailRigidBody;
    private ConfigurableJoint BoardJoint;
    public bool isFalling = false;

    private PlayerCollision parentGameObjectData;
    public float crashingThreshold = 5.0f;
    public float twist_value;
    public bool pressureOnStarboardSide = true; 

	// Change the force on the board during the manoeuvre
	public float manoeuvreModifier = 1.0f;

    // Use this for initialization
    void Start ()
    {
		userPrefs = externalObjectsHandler.GetComponent<ExternalObjectsReference> ().UserPrefs;
		raceManager = externalObjectsHandler.GetComponent<ExternalObjectsReference> ().raceManagerObject;
		windData= windSource.GetComponent<windEffector>();
		SailGeomSkinnedMesh = sailGeom.GetComponent<SkinnedMeshRenderer>();
        
        SailRigidBody = SailBone.GetComponent<Rigidbody>();
        /*foreach (Transform childObject in this.transform)
        {
            if (childObject.gameObject.name == "Sail")
            {
                SailGroup = childObject.gameObject;
                SailRigidBody = childObject.gameObject.GetComponent<Rigidbody>();
            }
        }*/
        //BoardJoint = player.GetComponent<ConfigurableJoint>();
        
        SailRigidBody.isKinematic = true;
        parentGameObjectData = this.gameObject.transform.parent.gameObject.GetComponent<PlayerCollision>();
        Board_Speed = 0.0f;
    }
   
    
    void OnCollisionEnter(Collision Col)
    {
 
        if (Col.relativeVelocity.magnitude > crashingThreshold)
        {
            Debug.Log("Collision Detected between" + this.gameObject.name + " and " + Col.gameObject.name);

            parentGameObjectData.playerCollisionHandling(this.gameObject, Col.gameObject); 
        }
    }

	public float manoeuvreThrustModifier(float thrust, float modifier, float boost)
	{
		float thrustModif = 0.0f;
		// this is 0 when not manoeuvrering, 1, when Manoeuering.
		float currentmanoeuvreWeight = this.gameObject.GetComponent<SailAnimScript> ().Manoeuvre_Weight;

		thrustModif = currentmanoeuvreWeight * thrust * (1 - modifier); 
		//Debug.Log("Manoeuvre Weight : " + currentmanoeuvreWeight);
		return thrustModif; // Thrust modifier adds up to the current Thrust factor
	}

    void FixedUpdate()
	{
       
        if (isFalling == false)
		{
            float sailThrust = Mathf.Sin((SetSailAngle + braquingAngle) * Mathf.Deg2Rad) * ApparentWindMagnitude;
            sailDrag_rearwind = 0.0f;
            if (trueWindAngleLocal < 90)
            {
                sailDrag_rearwind = 0.0f;
            }
            else
            {
                sailDrag_rearwind = (90.0f - trueWindAngleLocal );
            }

			/* The following has been replaced by different level of Manoeuver slow down
            if (this.gameObject.GetComponent<SailAnimScript>().intManoeuvreState == 1)
            {
                sailThrust = sailThrust /4;
            }*/


            /*if (Mathf.Abs(apparentWindAngleLocal) > 90)
            {
                sailDrag_rearwind = Mathf.Cos((apparentWindAngleLocal + braquingAngle) * Mathf.Deg2Rad) * ApparentWindMagnitude;
            }*/

            float SideForce = Mathf.Sin((SetSailAngle + braquingAngle) * Mathf.Deg2Rad) * ApparentWindMagnitude;
            float sailDrag = Mathf.Sin((SetSailAngle + braquingAngle) * Mathf.Deg2Rad) * ApparentWindMagnitude * sailDrag_factor;
            if (sailTiltDir.y <= 0)
            {
                SideForce = -1 * SideForce;
            }
            float linearBoardDrag = boardDrag * linearBoardDrag_factor * (Board_Speed * Board_Speed);

			float SailThrustForce = sailThrust * sailThrust_factor + (1* sailDrag_rearwind * sailDrag_rearwind_factor) - sailDrag - linearBoardDrag;

			float SailSideForce = -1 * SideForce * sailThrust_factor / 4;

			ManoeuvreType currentManoeuvre;
			if (this.GetComponentInParent<tricksHandlingScript> ().manoeuvreStatus == "tack") {
				currentManoeuvre = raceManager.GetComponent<UserPreferenceScript> ().localTackManoeuvres [this.gameObject.GetComponent<SailAnimScript> ().Manoeuvre_level];
			}
			else{
				currentManoeuvre = raceManager.GetComponent<UserPreferenceScript> ().localJibeManoeuvres [this.gameObject.GetComponent<SailAnimScript> ().Manoeuvre_level];
			}

			manoeuvreModifier = manoeuvreThrustModifier (SailThrustForce, currentManoeuvre.slowDownFactor, 1.5f);

			//Debug.Log ("Thrust : " + SailThrustForce +", Manoeuvre modifier : " + manoeuvreModifier);

			rbBoard.AddRelativeForce(SailSideForce, 0.0f , SailThrustForce - manoeuvreModifier);
            //Debug.Log(SailThrustForce);
        }
        else
        {
            //SailRigidBody.isKinematic = false;
            //Vector3 direction_sail = sailGeom.transform.up;
            //float forceIntentsity = 100* sailGeom.transform.forward.y;

            //SailBone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            //Debug.Log(sailGeom.transform.forward);
            //Debug.Log(forceIntentsity);
            //Debug.Log(direction_sail);
            /*ConfigurableJointMotion Fixed_X_Motion = SailRigidBody.GetComponent<ConfigurableJoint>().xMotion;
            ConfigurableJointMotion Free_X_Motion = SailRigidBody.GetComponent<ConfigurableJoint>().xMotion;
            SailRigidBody.GetComponent<ConfigurableJoint>().angularXMotion= Free;*/
            //SailRigidBody.AddRelativeForce(forceIntentsity * direction_sail.x, 0.0f, forceIntentsity * direction_sail.z);

        }
	}

    // Update is called once per frame
    void Update()
    {
        Board_Speed = rbBoard.velocity.magnitude;

		if ((transform.parent.GetComponent<PlayerCollision>().isPlayer == true)&&(externalObjectsHandler.GetComponent<ExternalObjectsReference> ().UIControlData != null)) {
			externalObjectsHandler.GetComponent<ExternalObjectsReference> ().UIControlData.MetricsDisplay.GetComponent<UI_Metrics_Handler> ().speed = Board_Speed;
			externalObjectsHandler.GetComponent<ExternalObjectsReference> ().UIControlData.MetricsDisplay.GetComponent<UI_Metrics_Handler> ().angle = trueWindAngleLocal;
		}
        apparentWind = windData.localWindDirectionVector * windData.effectiveLocalWindForce - player.transform.forward * Board_Speed;
        //Debug.Log(" WindData : " + windData.localWindDirectionVector * windData.effectiveLocalWindForce);
        //Debug.Log(" Board Speed : "+player.transform.forward * Board_Speed);
        ApparentWindMagnitude = apparentWind.magnitude;
        //Debug.Log("ApparentWind Global Vector: " + apparentWind);
        apparentWindAngleLocal = Vector3.Angle(apparentWind, -1 * player.transform.forward);
        trueWindAngleLocal = Vector3.Angle(windData.localWindDirectionVector, -1 * player.transform.forward);

        twist_value = Mathf.Abs((Board_Speed / 50 + (5 * rbBoard.angularVelocity.y)) * Mathf.Sin(trueWindAngleLocal * Mathf.Deg2Rad)) * 100;
        if (sailTiltDir.y <= 0)
        {
            pressureOnStarboardSide = true;
        }
        else
        {
            pressureOnStarboardSide = false;
        }
        if (pressureOnStarboardSide)
        {
            SailGeomSkinnedMesh.SetBlendShapeWeight(0, 0);
            SailGeomSkinnedMesh.SetBlendShapeWeight(1, 0);
            SailGeomSkinnedMesh.SetBlendShapeWeight(2, 0);
            SailGeomSkinnedMesh.SetBlendShapeWeight(3, 100 - twist_value);
            SailGeomSkinnedMesh.SetBlendShapeWeight(4, 0);
            SailGeomSkinnedMesh.SetBlendShapeWeight(5, 100 - twist_value);
        }
        else
        {
            
            SailGeomSkinnedMesh.SetBlendShapeWeight(0, twist_value);
            SailGeomSkinnedMesh.SetBlendShapeWeight(1, twist_value);
            SailGeomSkinnedMesh.SetBlendShapeWeight(2, 100);
            SailGeomSkinnedMesh.SetBlendShapeWeight(3, 100);
            SailGeomSkinnedMesh.SetBlendShapeWeight(4, 100);
            SailGeomSkinnedMesh.SetBlendShapeWeight(5, 100);
        }


       

        sailTiltDir = Vector3.Cross(windData.localWindDirectionVector,player.transform.forward);

		if (sailTiltDir.y <= 0 )
		{
			//sailGeom.transform.localScale=new Vector3(1.0f,1.0f,1.0f);
			//this.transform.localScale=new Vector3(1.0f,1.0f,-1.0f);
			SetSailAngle = 1*apparentWindAngleLocal;
            


            if (SetSailAngle > 90)
			{
				SetSailAngle = 90;
			}
			
		}
		else
		{
            //sailGeom.transform.localScale=new Vector3(1.0f,1.0f,1.0f);
            //this.transform.localScale=new Vector3(1.0f,1.0f,1.0f);

            SetSailAngle = 1 * apparentWindAngleLocal;

            if (SetSailAngle > 90)
			{
				SetSailAngle = 90;
			}
			
		}
		
		if (apparentWindAngleLocal < 45)
		{
			
			braquingAngle= apparentWindAngleLocal *20 /45;
		}
		else 
		{
			if (apparentWindAngleLocal < 90)
			{
				braquingAngle= 20;
			}
			else
			{
				if (apparentWindAngleLocal < 135)
				{
					braquingAngle= (30/45)*(apparentWindAngleLocal - 90) + 20;
				}	
				else
				{
					braquingAngle= (30 + 20)/45 *(180 - apparentWindAngleLocal ) ;
				}
			}
		}
		float VertAngle=transform.eulerAngles.y;
        if (isFalling == false)
        {
            transform.eulerAngles = new Vector3(0.0f, VertAngle, 0.0f);
        }
	}
}
