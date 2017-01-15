using UnityEngine;
using System.Collections;

public class WindEffectsScript : MonoBehaviour {
    
    public GameObject Board;
    public GameObject SailOrient;
    private Sail_System_Control SailOrientData;
    public Vector3 scale = new Vector3(2.0f, 1.0f, 1.8f);

    // Use this for initialization
    void Start ()
    { 
        SailOrientData = SailOrient.GetComponent<Sail_System_Control>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float apparentWindAngle = Vector3.Angle(new Vector3(1.0f, 0.0f, 0.0f), SailOrientData.apparentWind);
        if (SailOrientData.sailTiltDir.y < 0)
        {
            transform.localScale = new Vector3 (scale.x, scale.y, scale.z);
        }
        else
        {
            transform.localScale = new Vector3(scale.x, scale.y, -1*scale.z);
        }
        if (SailOrientData.trueWindAngleLocal<70 && SailOrientData.trueWindAngleLocal > -70)
        {
            foreach (Transform child in transform)
            {
                //if (child.gameObject.name == "Wind_effect_positive" || child.gameObject.name == "Positive Effect")
                if (child.gameObject.name == "Positive Effect")
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                //if (child.gameObject.name == "Wind_effect_positive" || child.gameObject.name == "Positive Effect")
                if (child.gameObject.name == "Positive Effect")
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        transform.eulerAngles = new Vector3(0.0f, -1*apparentWindAngle+180, 0.0f);
        //transform.eulerAngles = new Vector3(0.0f, -1 * WindEffectorData.localWindDirection + 180f, 0.0f);
        transform.localPosition = new Vector3(Board.transform.localPosition.x, 0.0f, Board.transform.localPosition.z);

    }
}
