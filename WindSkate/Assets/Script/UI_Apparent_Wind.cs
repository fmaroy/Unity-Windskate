using UnityEngine;
using System.Collections;

public class UI_Apparent_Wind : MonoBehaviour {
	public GameObject windSource;
	private windEffector windData;
	public GameObject SailOrient;
	private Sail_System_Control SailOrientData;
	public GameObject player;
	public Rigidbody rbBoard;
	private Vector3 apparentWind;
	private float windUIScale;
	private float initialWindForce;
    //private WindGustsBehavior WindGustsData;


    // Use this for initialization
    void Start () {
		//windSource = GameObject.Find("Board_assembly");
		windData= windSource.GetComponent<windEffector>();
		//SailOrient = GameObject.Find("Sail");
		SailOrientData= SailOrient.GetComponent<Sail_System_Control>();
		windUIScale = 1.0f;
        //print(windData.effectiveLocalWindForce);

        initialWindForce = GameObject.Find("WindGusts").GetComponent<WindGustsBehavior>().initWindForce;	
	}
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.transform.parent.gameObject.name + " Initial Wind force : " + initialWindForce);
        windUIScale = windData.effectiveLocalWindForce / initialWindForce;
        transform.localScale = new Vector3(windUIScale, 1.0f, 1.0f);
    }
    void LateUpdate()
    { 
        float apparentWindAngleCircle = Vector3.Angle(new Vector3(1.0f,0.0f,0.0f), SailOrientData.apparentWind);
		transform.eulerAngles = new Vector3 (90.0f,0.0f, apparentWindAngleCircle);
		transform.localPosition = new Vector3 (6.5f*Mathf.Sin(apparentWindAngleCircle * Mathf.Deg2Rad), -6.5f*Mathf.Cos(apparentWindAngleCircle * Mathf.Deg2Rad),0.0f);
	}
}
