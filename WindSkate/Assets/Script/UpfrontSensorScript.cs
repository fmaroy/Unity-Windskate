using UnityEngine;
using System.Collections;

public class UpfrontSensorScript : MonoBehaviour {

    public GameObject Board;
    private Rigidbody RbBoard;
    public float SensorMulitplier = 0.5f;

	// Use this for initialization
	void Start () {
        RbBoard = Board.GetComponent<Rigidbody>();
        SensorMulitplier = 0.5f;

    }
	
	// Update is called once per frame
	void Update () {

        transform.position = Board.transform.position + RbBoard.velocity * SensorMulitplier;

    }
}
