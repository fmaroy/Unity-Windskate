using UnityEngine;
using System.Collections;

public class UIWindCircleScript : MonoBehaviour {
    public GameObject player;
    public float rotationOffset;
	// Use this for initialization
	void Start () {
        rotationOffset = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        //transform.eulerAngles = new Vector3(0.0f, player.transform.eulerAngles.y, 0.0f);
        transform.localEulerAngles = new Vector3(0.0f, 0.0f, -1*player.transform.eulerAngles.y + 90f + rotationOffset);
    }
}
