using UnityEngine;
using System.Collections;

public class UIStore_Models_Rotator : MonoBehaviour {
    public float RotationSpeed = 75.0f;
    private float x_EulerRotator = 0.0f;
    private float y_EulerRotator = 0.0f;
    // Use this for initialization
    void Start () {
        x_EulerRotator = transform.localEulerAngles.x;
        y_EulerRotator = transform.localEulerAngles.y;


    }
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles = new Vector3(x_EulerRotator, y_EulerRotator, Time.fixedTime* RotationSpeed);
	}
}
