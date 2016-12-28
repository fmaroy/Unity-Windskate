using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFxHandler : MonoBehaviour {
    public GameObject SpeedFx;
    public Vector2 speedFxRange;
    public Sail_System_Control sailData;
    public float speed;

	// Use this for initialization
	void Start () {
        foreach (Transform obj in this.gameObject.transform)
        {
            if (obj.gameObject.GetComponent<Sail_System_Control>()!= null)
            {
                sailData = obj.gameObject.GetComponent<Sail_System_Control>();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        speed = sailData.Board_Speed;

        if (speed > speedFxRange[0])
        {
            foreach (Transform particleObj in SpeedFx.transform)
            {
                particleObj.gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            foreach (Transform particleObj in SpeedFx.transform)
            {
                particleObj.gameObject.GetComponent<ParticleSystem>().Stop();
            }
        }
	}
}
