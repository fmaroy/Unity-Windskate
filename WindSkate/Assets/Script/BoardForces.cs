using UnityEngine;
using System.Collections;


public class BoardForces : MonoBehaviour {
    private bool isPlayer;
    public bool localManualDrive;
	public float thrust_multiplier;
	public float torque_multiplier;
	public float rotationSpeed;
	public float angle_mutiplier;
	public float sailor_weight;
	private Rigidbody rb;
	
	private bool LeftButtonDownBool = false;
	private bool RightButtonDownBool = false;
	
	//for geom roation assignement
	public GameObject front_axis;
	public GameObject rear_axis;
	public GameObject boardObject;
	public GameObject wheelFrontLeft;
	private GameObject wheelFrontRight;
	private GameObject wheelRearLeft;
	private GameObject wheelRearRight;
	private WheelCollider wheelFrontLeftCollider;
	private WheelCollider wheelFrontRightCollider;
	private WheelCollider wheelRearLeftCollider;
	private WheelCollider wheelRearRightCollider;
	private float rotation;
	public float startTime;
    
    private PlayerCollision parentGameObjectData;

	public bool isStarting;

    public float crashingThreshold = 10.0f;

    // change on 18 may
    public float rotationToDirection = 0.0f;

    public void OnCollisionEnter(Collision Col)
    {

        if (Col.relativeVelocity.magnitude > crashingThreshold)
        {
            //Debug.Log("Collision Detected between" + this.gameObject.name + " and " + Col.gameObject.name);
            parentGameObjectData.playerCollisionHandling(this.gameObject, Col.gameObject);

        }
    }

    public GameObject recusiveSearch(string nameToFind, GameObject gameObjectToBrowse)
	{
        //Debug.Log("Looking for : " + nameToFind + " in : " + gameObjectToBrowse.name);
		foreach (Transform child in gameObjectToBrowse.transform)
		{
            //Debug.Log(child.gameObject.name);
            if (child.gameObject.name == nameToFind)
				{
					return child.gameObject;
				}
			else
			{
				foreach (Transform subchild in child)
				{
                    //Debug.Log(subchild.gameObject.name);
                    if (subchild.gameObject.name == nameToFind)
					{
						return subchild.gameObject;
					}
					else
					{
						foreach (Transform subsubchild in subchild)
						{
                            //Debug.Log(subsubchild.gameObject.name);
                            if (subsubchild.gameObject.name == nameToFind)
                            {
                                Debug.Log("Found : " + subsubchild.gameObject.name);
                                return subsubchild.gameObject;
                            }
						}
					}
				}
			}
		}
		return this.gameObject;
	}
	// Use this for initialization
	void Start ()
	{
        rb = GetComponent<Rigidbody> ();

        parentGameObjectData = this.gameObject.transform.parent.gameObject.GetComponent<PlayerCollision>();
        isPlayer = parentGameObjectData.isPlayer;
        localManualDrive = parentGameObjectData.ManualDrive;

        updateFrontWheelsControls(front_axis);
        updateRearWheelsControls(rear_axis);
    }
	
    public void updateWheelsControls(GameObject Obj)
    {
        wheelFrontLeft = recusiveSearch("Wheel_FL", Obj);
        wheelFrontRight = recusiveSearch("Wheel_FR", Obj);
        wheelRearLeft = recusiveSearch("Wheel_RL", Obj);
        wheelRearRight = recusiveSearch("Wheel_RR", Obj);

        wheelFrontLeftCollider = wheelFrontLeft.GetComponent<WheelCollider>();
        wheelFrontRightCollider = wheelFrontRight.GetComponent<WheelCollider>();
        wheelRearLeftCollider = wheelRearLeft.GetComponent<WheelCollider>();
        wheelRearRightCollider = wheelRearRight.GetComponent<WheelCollider>();
    }
    public void updateRearWheelsControls(GameObject Obj)
    {
        wheelRearLeft = recusiveSearch("Wheel_RL", Obj);
        wheelRearRight = recusiveSearch("Wheel_RR", Obj);

        wheelRearLeftCollider = wheelRearLeft.GetComponent<WheelCollider>();
        wheelRearRightCollider = wheelRearRight.GetComponent<WheelCollider>();
    }
    public void updateFrontWheelsControls(GameObject Obj)
    {
        wheelFrontLeft = recusiveSearch("Wheel_FL", Obj);
        wheelFrontRight = recusiveSearch("Wheel_FR", Obj);
        
        wheelFrontLeftCollider = wheelFrontLeft.GetComponent<WheelCollider>();
        wheelFrontRightCollider = wheelFrontRight.GetComponent<WheelCollider>();
        
    }
    //UI Controls handling
    public void LeftTurnButtonDown()
	{
       LeftButtonDownBool = true;   
	}
	public void RightTurnButtonDown()
	{
		RightButtonDownBool = true;
	}
	public void LeftTurnButtonUp()
	{
		LeftButtonDownBool = false;
	}
	public void RightTurnButtonUp()
	{
		RightButtonDownBool = false;
	}
	
	void Update()
	{
        if(localManualDrive == false)
        {
            front_axis.transform.localEulerAngles = new Vector3(270.0f, 1 * angle_mutiplier * rotation, 0.0f);
            rear_axis.transform.localEulerAngles = new Vector3(270.0f, 180 - 1* angle_mutiplier * rotation, 0.0f);
            wheelFrontLeftCollider.steerAngle = 1 * angle_mutiplier * rotation;
            wheelFrontRightCollider.steerAngle = 1 * angle_mutiplier * rotation;
            wheelRearLeftCollider.steerAngle = -1 * angle_mutiplier * rotation;
            wheelRearRightCollider.steerAngle = -1 * angle_mutiplier * rotation;
            boardObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -1 * angle_mutiplier * rotation);
        }
    }
	
	void FixedUpdate ()
	{
        if (localManualDrive == true)
        {
            rotation = Input.GetAxis("Horizontal") * rotationSpeed;
            if (LeftButtonDownBool == true)
            {
                rotation = -0.75f * rotationSpeed;
            }
            if (RightButtonDownBool == true)
            {
                rotation = 0.75f * rotationSpeed;
            }
            if (RightButtonDownBool == false && LeftButtonDownBool == false)
            {
                rotation = Input.GetAxis("Horizontal") * rotationSpeed;
            }
            rotation *= Time.fixedDeltaTime;


            //Add vertical force

            //sailor_weight = 200.0f * this.GetComponent<Rigidbody>().velocity.magnitude;
            rb.AddForce(new Vector3(0.0f, -1 * sailor_weight, 0.0f));

            //Steering: Transform Geometries
            front_axis.transform.localEulerAngles = new Vector3(270.0f, 1 * angle_mutiplier * rotation, 0.0f);
            rear_axis.transform.localEulerAngles = new Vector3(270.0f, -1 * angle_mutiplier * rotation, 0.0f);
            wheelFrontLeftCollider.steerAngle = 1 * angle_mutiplier * rotation;
            wheelFrontRightCollider.steerAngle = 1 * angle_mutiplier * rotation;
            wheelRearLeftCollider.steerAngle = -1 * angle_mutiplier * rotation;
            wheelRearRightCollider.steerAngle = -1 * angle_mutiplier * rotation;
            boardObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -1 * angle_mutiplier * rotation);

            //Temp : Manual thrust
            float thrust = Input.GetAxis("Vertical") * thrust_multiplier;

            //Add Thrust Force
			if (isStarting == false) {
				rb.AddRelativeForce (0.0f, 0.0f, thrust * thrust_multiplier);
			}
        }
        else
        {
            rotation = rotationToDirection * rotationSpeed * Time.fixedDeltaTime;
            rotation *= Time.fixedDeltaTime;
			if (isStarting == false) {
				rb.AddForce (new Vector3 (0.0f, -1 * sailor_weight, 0.0f));
			}
        }
		
		
	}
	

}
