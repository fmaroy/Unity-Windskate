using UnityEngine;
using System.Collections;

public class UI_True_Wind_Direction : MonoBehaviour {
	public GameObject windSource;
	private windEffector windData;
	public GameObject player;
	private float windUIScale;
	private float initialWindForce;
	private SpriteRenderer UIRenderer;
	public GameObject SailOrient;
	private Sail_System_Control SailOrientData;
    public GameObject Broad_Sector_Highlight;
    public GameObject Narrow_Sector_Highlight;
    public GameObject Best_Sector_Highlight;
    public bool bestAngleFlag = false;
    public bool enableSectors = true;
    private float windAngle;
    private int bestSectorInt = 0;
	public GameObject circleObj;
    

	// Use this for initialization
	void Start () {
		
        bestAngleFlag = false;
        //windSource = GameObject.Find("Wind");
        windData = windSource.GetComponent<windEffector>();
		windUIScale = 1.0f;
        //initialWindForce= windData.effectiveLocalWindForce;
        //SailOrient = GameObject.Find("Sail");
        SailOrientData = SailOrient.GetComponent<Sail_System_Control>();
		if (gameObject.GetComponent<SpriteRenderer> () != null) {
			UIRenderer = gameObject.GetComponent<SpriteRenderer> ();
		}
        initialWindForce = GameObject.Find("WindGusts").GetComponent<WindGustsBehavior>().initWindForce;
        windAngle = player.GetComponent<Follow_track>().angleBoardToWind;
        bestSectorInt = 0;

    }

    // Update is called once per frame
	void LateUpdate () {
        bestSectorInt = player.transform.parent.GetComponent<tricksHandlingScript>().activeSector;
        windAngle = player.GetComponent<Follow_track>().angleBoardToWind;
        windUIScale = windData.effectiveLocalWindForce / initialWindForce;
		transform.eulerAngles = new Vector3 (90.0f,0.0f,-1*windData.localWindDirection + 180f);
		transform.localPosition = new Vector3 (7.5f*Mathf.Sin(1*windData.localWindDirection*Mathf.Deg2Rad), 7.5f*Mathf.Cos(1*windData.localWindDirection*Mathf.Deg2Rad),0.2f);
		transform.localScale = new Vector3 (windUIScale , 1.0f, 1.0f);

		int coursetype = transform.parent.GetComponent<CircleIndicators> ().typeOfCourse;

		if (gameObject.GetComponent<SpriteRenderer> () != null) {
			UIRenderer.color = transform.parent.GetComponent<CircleIndicators> ().getColorForWindAngle (coursetype, SailOrientData.trueWindAngleLocal);
		}
		if (gameObject.GetComponent<ParticleSystem> () != null) {
			gameObject.GetComponent<ParticleSystem>().startColor = transform.parent.GetComponent<CircleIndicators> ().getColorForWindAngle (coursetype, SailOrientData.trueWindAngleLocal);
		}
        /*if (bestSectorInt == 0)
        {
            Best_Sector_Highlight.SetActive(false);
        }
        else
        {
            Best_Sector_Highlight.SetActive(true);
        }

        if (bestSectorInt == 1)
        {
            Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = 45.0f;
        }
        if (bestSectorInt == 2)
        {
            Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = -45.0f;
        }
        if (bestSectorInt == 3)
        {
            Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = 135.0f;
        }
        if (bestSectorInt == 4)
        {
            Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = -135.0f;
        }*/

        /*if ((SailOrientData.trueWindAngleLocal > 40 && SailOrientData.trueWindAngleLocal < 50)||(SailOrientData.trueWindAngleLocal > 130 && SailOrientData.trueWindAngleLocal < 140))
        {

            Best_Sector_Highlight.SetActive(true);
            if (SailOrientData.trueWindAngleLocal > 40 && SailOrientData.trueWindAngleLocal < 50)
            {
                if (windAngle < 0)
                {
                    Debug.Log("Offset : 45");
                    Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = 45.0f;
                }
                else
                {
                    Debug.Log("Offset : -45");
                    Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = -45.0f;
                }
            }
            if (SailOrientData.trueWindAngleLocal > 130 && SailOrientData.trueWindAngleLocal < 140)
            {
                if (windAngle < 0)
                {
                    Debug.Log("Offset : 135");
                    Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = 135.0f;
                }
                else
                {
                    Debug.Log("Offset : -135");
                    Best_Sector_Highlight.GetComponent<UIWindCircleScript>().rotationOffset = -135.0f;
                }
            }
        }
        else
        {
            Best_Sector_Highlight.SetActive(false);
        }
        */

    }

}
