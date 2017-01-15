using UnityEngine;
using System.Collections;


public class Board_control_opponent : MonoBehaviour {
	
	public float thrust_multiplier;
	public float torque_multiplier;
	public float rotationSpeed;
	public float angle_mutiplier;
	public float sailor_weight;
	private Rigidbody rb;
	//public Rigidbody boardForces;
	//public GameObject Sail;
	//private GameObject windSource;
	//private windEffector windData;
	
	//for geom roation assignement
	public GameObject front_axis;
	public GameObject rear_axis;
	public GameObject boardObject;
	private GameObject wheelFrontLeft;
	private GameObject wheelFrontRight;
	private GameObject wheelRearLeft;
	private GameObject wheelRearRight;
	private WheelCollider wheelFrontLeftCollider;
	private WheelCollider wheelFrontRightCollider;
	private WheelCollider wheelRearLeftCollider;
	private WheelCollider wheelRearRightCollider;
	private float rotation;
	public float startTime;
	public float rotationToDirection = 0.0f ;
    private PlayerCollision parentGameObjectData;
    public float crashingThreshold = 10.0f;

    /*void OnCollisionEnter(Collision Col)
    {
        if (Col.relativeVelocity.magnitude > crashingThreshold)
        {
            Debug.Log("Collision Detected between" + this.gameObject.name + " and " + Col.gameObject.name);
            
            parentGameObjectData.playerCollisionHandling(gameObject, Col.gameObject);
        }
    }*/

    public GameObject recusiveSearch(string nameToFind, GameObject gameObjectToBrowse)
    {

        foreach (Transform child in gameObjectToBrowse.transform)
        {
            if (child.gameObject.name == nameToFind)
            {
                return child.gameObject;
            }
            else
            {
                foreach (Transform subchild in child)
                {
                    if (subchild.gameObject.name == nameToFind)
                    {
                        return subchild.gameObject;
                    }
                    else
                    {
                        foreach (Transform subsubchild in subchild)
                        {

                            if (subsubchild.gameObject.name == nameToFind)
                            {
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
		//windSource = GameObject.Find("Wind");
		//windData= windSource.GetComponent<windEffector>();
		wheelFrontLeft = recusiveSearch("Wheel_FL", this.gameObject);
		wheelFrontRight = recusiveSearch("Wheel_FR", this.gameObject);
		wheelRearLeft = recusiveSearch("Wheel_RL", this.gameObject);
		wheelRearRight = recusiveSearch("Wheel_RR", this.gameObject);
		wheelFrontLeftCollider= wheelFrontLeft.GetComponent<WheelCollider>();
		wheelFrontRightCollider= wheelFrontRight.GetComponent<WheelCollider>();
		wheelRearLeftCollider= wheelRearLeft.GetComponent<WheelCollider>();
		wheelRearRightCollider= wheelRearRight.GetComponent<WheelCollider>();

        rotationToDirection = 0.0f;

        parentGameObjectData = this.gameObject.transform.parent.gameObject.GetComponent<PlayerCollision>();

    }
	
	void Update()
	{
        front_axis.transform.localEulerAngles = new Vector3(270.0f, 1 * angle_mutiplier * rotation, 0.0f);
        rear_axis.transform.localEulerAngles = new Vector3(270.0f, -1 * angle_mutiplier * rotation, 0.0f);
        wheelFrontLeftCollider.steerAngle = 1 * angle_mutiplier * rotation;
        wheelFrontRightCollider.steerAngle = 1 * angle_mutiplier * rotation;
        wheelRearLeftCollider.steerAngle = -1 * angle_mutiplier * rotation;
        wheelRearRightCollider.steerAngle = -1 * angle_mutiplier * rotation;
        boardObject.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -1 * angle_mutiplier * rotation);

    }

    void FixedUpdate ()
	{
        rotation = rotationToDirection * rotationSpeed;
        rotation *= Time.deltaTime;
        
        rb.AddForce( new Vector3 (0.0f,-1 *sailor_weight,0.0f));

	}
	

}
