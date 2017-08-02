using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow_Face : MonoBehaviour {

	public GameObject trackedObject;
	//public GameObject ReferenceObjectPosition;
	public Vector3 PositionShift;
	//public SmoothFollow_Fab smoothfollow;

	[SerializeField]
	public Transform target;
	[SerializeField]
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	[SerializeField]
	public float height = 5.0f;
	[SerializeField]
	public float rotationDamping;
	[SerializeField]
	public float heightDamping;

	// Use this for initialization
	void Start () {
		//smoothfollow = this.GetComponent<SmoothFollow_Fab> ();
		//smoothfollow.target = trackedObject.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		// Calculate the current rotation angles
		var wantedRotationAngle = target.eulerAngles.y +180;
		var wantedHeight = target.position.y + height;

		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;

		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

		// Always look at the target
		transform.LookAt(target.transform.position);
	}
}
