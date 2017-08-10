using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleIndicators : MonoBehaviour {

    public GameObject playerObj;
    public Follow_track followTrackData;
    public GameObject trackDirectionIndicator;
	public GameObject trackDirectionLine;
	public GameObject trackDirectionTickArrow;
	public GameObject trackDirectionTickIcon;
    public Color trackDirColor;
    public GameObject nextTrackMark;
    public float angleBoardToMark;
    public RaceManagerScript RaceData;
    public GameObject opponentTicksContainer;
    public List<GameObject> opponentTickList = new List<GameObject>();
    public List<Color> CloseOpponent = new List<Color>();
	public GameObject apparentWindArrow;
	public GameObject trueWindArrow;
	private IEnumerator messageCoroutine;
	//private IEnumerator interruptMessage;
	public bool turnAroundMessageAlreadyThrown;
	public GameObject WindArrow;
	public float windAngle;
	public Color debugColor;
	public int typeOfCourse = 0; //0 for upwind, 1 if Downwind, 2 if direct to mark 

	public Color[] WindAnglesColors = new Color[3];

	public List<WindAnglesClass> listOfWindAngles = new List<WindAnglesClass> ();

	void find2ClosestAnglesinList (float val, List<float> list, out float firstClosest, out float secondClosest)
	{
		firstClosest = 180;
		secondClosest = 180;

		foreach (float a in list) 
		{
			float diffAngle = Mathf.Abs(Mathf.DeltaAngle (a, val));
			if (firstClosest > diffAngle) {firstClosest = diffAngle;}
		}
		//find second one
		foreach (float a in list) 
		{
			float diffAngle = Mathf.Abs(Mathf.DeltaAngle (a, val));
			if ((secondClosest > diffAngle) && (diffAngle > firstClosest)) {secondClosest = diffAngle;}
		}
		//Debug.Log ("found first closest " + firstClosest + ", second closest " + secondClosest);
	}

	public Color getColorForWindAngle (int courseID, float angle)
	{
		Color windColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);;
		float bestAngleFactor;

		float[] closestBestAngles = new float[2];
		float[] closestWorstAngles = new float[2];
		float closestAnglefactor;
		float closestAngleDiff;
		float secondAngleFactor;
		float secondAngleDiff;

		find2ClosestAnglesinList (angle, listOfWindAngles [courseID].bestAngles, out closestBestAngles [0], out closestBestAngles [1]);
		find2ClosestAnglesinList (angle, listOfWindAngles [courseID].worstAngles, out closestWorstAngles [0], out closestWorstAngles [1]);

		/*Debug.Log ("Best Closest : " + closestBestAngles [0]);
		Debug.Log ("Worst Closest : " + closestWorstAngles [0]);
		Debug.Log ("Best Second : " + closestBestAngles [1]);
		Debug.Log ("Worst Second : " + closestWorstAngles [1]);*/

		// get factor of closest angle
		if (closestBestAngles [0] < closestWorstAngles [0]) {
			closestAnglefactor = 1f;
			closestAngleDiff = closestBestAngles [0];
			//Debug.Log ("closest angle is a best one");
			if (closestWorstAngles [0] < Mathf.Min (closestBestAngles [1], closestWorstAngles [1])) // check if the second closest value is the first of the worst of part of the second choince in best and wors
			{ 
				secondAngleFactor = 0.0f;
				secondAngleDiff = closestWorstAngles [0];
			} 
			else // now we have to search for the minimal value of the second best and worst
			{ 
				if (closestBestAngles [1] < closestWorstAngles [1]) { // the second closest is the second best position
					secondAngleFactor = 1.0f;
					secondAngleDiff = closestBestAngles [1];
				} 
				else 
				{
					secondAngleFactor = 0.0f;
					secondAngleDiff = closestWorstAngles [1];
				}
			}

		} else {
			//Debug.Log ("closest angle is a worst one");
			closestAnglefactor = 0f;
			closestAngleDiff = closestWorstAngles [0];

			if (closestBestAngles [0] < Mathf.Min (closestBestAngles [1], closestWorstAngles [1])) // check if the second closest value is the first of the worst of part of the second choince in best and wors
			{ 
				
				secondAngleFactor = 1.0f;
				secondAngleDiff = closestBestAngles [0];
			} 
			else // now we have to search for the minimal value of the second best and worst
			{ 
				if (closestBestAngles [1] < closestWorstAngles [1]) { // the second closest is the second best position
					secondAngleFactor = 1.0f;
					secondAngleDiff = closestBestAngles [1];
				} 
				else 
				{
					secondAngleFactor = 0.0f;
					secondAngleDiff = closestWorstAngles [1];
				}
			}
		}

		//Debug.Log ("closest is " + closestAnglefactor + " at " + closestAngleDiff + ", second closest is " + secondAngleFactor + " at " + secondAngleDiff);
		float resultFactor = 0f;
		if (closestAnglefactor != secondAngleFactor) 
		{
			resultFactor = ((secondAngleFactor * closestAngleDiff)  + ( closestAnglefactor * secondAngleDiff)) / (closestAngleDiff + secondAngleDiff);
			//Debug.Log ("temp factor calc : " + resultFactor);
		} 
		else 
		{
			resultFactor = closestAnglefactor;
		}
		if (resultFactor > 1) { resultFactor = 1;}
		if (resultFactor < 0) { resultFactor = 0;}

		//Debug.Log ("resultFactor is : " + resultFactor);

		float colorSectorwidth = 1 / (WindAnglesColors.Length - 1);

		if ((resultFactor < 1.0f)||(resultFactor > 0.0f)) {
			/*for (int i = 0; i < WindAnglesColors.Length - 1; i++) {
				if ((resultFactor > i * colorSectorwidth) && (resultFactor < (i + 1) * colorSectorwidth)) {
					float value = resultFactor - (colorSectorwidth * (WindAnglesColors.Length - 1)) * (WindAnglesColors.Length - 1);
					windColor = Color.Lerp (WindAnglesColors [i], WindAnglesColors [i + 1], resultFactor);
				}
			}*/

			float h0;
			float s0; 
			float v0;
			Color.RGBToHSV (WindAnglesColors [0], out h0, out s0, out v0);
			float h1;
			float s1; 
			float v1;
			Color.RGBToHSV (WindAnglesColors [1], out h1, out s1, out v1);
			float h = Mathf.Lerp(h0, h1, resultFactor);
			float s = Mathf.Lerp(s0, s1, resultFactor);
			float v = Mathf.Lerp(v0, v1, resultFactor);

			windColor = Color.HSVToRGB (h, s, v);
			//windColor = Color.Lerp (WindAnglesColors [0], WindAnglesColors [1], resultFactor);
		}
		if (resultFactor == 0.0f) {windColor = WindAnglesColors [0];}
		if (resultFactor == 1.0f) {windColor = WindAnglesColors [WindAnglesColors.Length-1];}

		//Debug.Log ("resultFactor is : " + resultFactor);

		debugColor = windColor;

		return windColor;
	}

    // Use this for initialization
    void Start()
    {
        foreach (Transform obj in this.gameObject.transform.parent)
        {
            if (obj.gameObject.GetComponent<Follow_track>() != null)
            {
                playerObj = obj.gameObject;
                followTrackData = obj.gameObject.GetComponent<Follow_track>();
            }
        }
 
        foreach (Transform child in opponentTicksContainer.transform)
        {
            opponentTickList.Add(child.gameObject);
        }
        RaceData = GameObject.Find("RaceManager").GetComponent<RaceManagerScript>();

		turnAroundMessageAlreadyThrown = false;
    }
	
    public float getOrientationAngle(Vector3 target_position)
    {
        float angle;
        Vector3 direction = target_position - transform.position;
        angle = Vector3.Angle(Vector3.forward, direction);
        if (Vector3.Cross(Vector3.forward, direction).y > 0)
        {
            angle = -1 * angle;
        }

        return angle;
    }

	public void placeOnCircle(GameObject obj, float dist, float angle, float height)
	{
		obj.transform.localPosition = new Vector3(dist * Mathf.Sin(angle * Mathf.Deg2Rad), dist * Mathf.Cos(angle * Mathf.Deg2Rad), height);
	}

	public void placeIconOnCircle(GameObject obj, float dist, float angle, float height)
	{
		obj.transform.localPosition = new Vector3(-dist * Mathf.Cos(angle * Mathf.Deg2Rad), height, -dist * Mathf.Sin(angle * Mathf.Deg2Rad));
	}

	public bool hasVisibileChildren(GameObject obj)
	{
		bool vis = false;
		if (obj.GetComponent<MeshRenderer> () != null) {
			if (obj.GetComponent<MeshRenderer> ().isVisible == true) {
				vis = true;
			}
		} 
		foreach (Transform child in obj.transform) {
			if (child.gameObject.GetComponent<MeshRenderer> () != null) {
				if (child.gameObject.GetComponent<MeshRenderer> ().isVisible == true) {
					vis = true;
				}
			}
		}
		return vis;
	}

	IEnumerator waitBeforeMessage (float timer)
	{
		while (turnAroundMessageAlreadyThrown) {
			yield return new WaitForSeconds(timer);
			transform.parent.gameObject.GetComponent<ExternalObjectsReference> ().UIControlData.MessageText.GetComponent<messageHandler> ().throwMessage (0);
		}
	}

	public void displayManagerForTick(GameObject tickObj, bool vis, GameObject targetPlayer, Color col)
	{
		foreach (Transform child in tickObj.transform) {
			child.GetComponent<SpriteRenderer> ().enabled = vis;
			child.GetComponent<SpriteRenderer> ().color = col;
			foreach (Transform subchild in child) {
				if(subchild.GetComponent<RawImage> () != null){
					subchild.GetComponent<RawImage> ().enabled = vis;
					if (targetPlayer != null){
						subchild.GetComponent<RawImage> ().texture = targetPlayer.GetComponentInChildren<PlayerfaceImageHandler> ().rt;
					}
				}
				if(subchild.GetComponent<TextMesh> () != null){
					//Debug.Log ("Text of tick found");
					subchild.gameObject.SetActive(vis);
					if (vis) {
						subchild.GetComponent<TextMesh> ().text = targetPlayer.GetComponent<playerInventory>().PlayerName;
					}
				}
			}
		}
	}

    // Update is called once per frame
    void Update() {

		typeOfCourse = followTrackData.isNextTargetMark;
		windAngle = playerObj.GetComponent<Follow_track>().angleBoardToWind;

        ///Handling the track direction
        nextTrackMark = followTrackData.currentMark;

        Vector3 directionToMark = nextTrackMark.transform.position - transform.position;
        float angleToMark = getOrientationAngle(nextTrackMark.transform.position);

        trackDirectionIndicator.transform.eulerAngles = new Vector3(90.0f, 0, angleToMark);
        float correctAngle = -1 * angleToMark - 90f; // World angle of the direction to the next mark
		angleBoardToMark = Mathf.DeltaAngle(playerObj.transform.eulerAngles.y -90 , correctAngle); // Angle between the board and the next direction

		if (hasVisibileChildren (followTrackData.currentMark) == true) {
			trackDirectionTickIcon.SetActive (false);
		} else {
			trackDirectionTickIcon.SetActive (true);
			//placeOnCircle (trackDirectionTickIcon, 6f, correctAngle, -3.67f);
			placeOnCircle (trackDirectionTickIcon, 8f, correctAngle, -2.2f);
		}

		placeOnCircle (trackDirectionLine, 30f, correctAngle, 0f);

		placeOnCircle (trackDirectionTickArrow, 8.2f, correctAngle, 0f);
		trackDirectionTickArrow.transform.localEulerAngles = new Vector3 (0.0f, 0.0f, angleToMark + 180);

		//messageWaitbefore = null;
		//interruptMessage = null;
		if (Mathf.Abs (angleBoardToMark) > 120f) {
			if (turnAroundMessageAlreadyThrown == false) {
				turnAroundMessageAlreadyThrown = true;
				messageCoroutine = waitBeforeMessage(5f); // this throws a message every 5 sec until
				StartCoroutine (messageCoroutine);
			}
		} else {
			if (turnAroundMessageAlreadyThrown == true) { // angle is less than 120deg and a message has been thrown already, needs to interrupt the message throwing
				StopCoroutine (messageCoroutine);
				//transform.parent.gameObject.GetComponent<ExternalObjectsReference> ().UIControlData.MessageText.GetComponent<messageHandler> ().interruptMessage(0);
				turnAroundMessageAlreadyThrown = false;
			}
		}

		/*placeOnCircle (trackDirectionIndicator, 30f, correctAngle, -10f); // old method with the projector

		float srtDistToMark = Mathf.Pow(directionToMark.x, 2) + Mathf.Pow(directionToMark.z, 2);
        float fadeFactor = Mathf.InverseLerp(500, 8000, srtDistToMark);
        //Debug.Log("FadeFactor " + fadeFactor);
        Color col = trackDirectionIndicator.GetComponent<Projector>().material.GetColor("_Color");
        for (int i = 0; i < 3; i++)
        {
            col[i] = trackDirColor[i] * fadeFactor;
        }
        trackDirectionIndicator.GetComponent<Projector>().material.SetColor("_Color", col);
		*/
        List<GameObject> Opponents = new List<GameObject>();
        List<float> opponentDistSqList = new List<float>();
        foreach (GameObject opponent in RaceData.OpponenentObjectsList)
        {
            GameObject board = opponent.GetComponent<playerInventory>().boardList[opponent.GetComponent<playerInventory>().currentBoard];
            if (board.GetComponent<MeshRenderer>().isVisible == false)
            {
                //Debug.Log(board.name + " Not Visible");
                Opponents.Add(opponent);
            }
            else
            {
                //Debug.Log(board.name + " Visible");
            }
        }
        //Debug.Log(Opponents.Count);

        foreach (GameObject opponent in Opponents)
        {
            GameObject opponentBoard = opponent.transform.GetChild(0).gameObject;
            float distsq = Mathf.Pow(opponentBoard.transform.position.x - transform.position.x, 2) + Mathf.Pow(opponentBoard.transform.position.z - transform.position.z, 2);
            opponentDistSqList.Add(distsq);
        }

        int numberOfVisibleOpponents = Opponents.Count;
        int numberOfTicksToHandle = 0;

        if (numberOfVisibleOpponents > 2)
        {
            numberOfTicksToHandle = 2;
        }
        else
        {
            numberOfTicksToHandle = numberOfVisibleOpponents;
        }
        //Debug.Log("Ticks to handle : "+ numberOfTicksToHandle);
        for (int i = 0; i < 2; i++)
        {
			displayManagerForTick (opponentTickList [i], false, null, CloseOpponent [2]);
            //opponentTickList[i].GetComponent<SpriteRenderer>().enabled = false;
            //opponentTickList[i].transform.GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < numberOfTicksToHandle; i++)
        {
            float closestOpponentDist = FindMinValueindexOfList(opponentDistSqList);
            int closestOpponentId = opponentDistSqList.IndexOf(closestOpponentDist);
            //Get orientation of the closest Opponent
            float angleToOpponent = getOrientationAngle(Opponents[closestOpponentId].transform.GetChild(0).position);
            //Debug.Log("To opponent to : " + closestOpponentId);
            //Debug.Log("Dist to opponent : " + opponentDistSqList[closestOpponentId]);

            //opponentTickList[i].transform.localPosition = new Vector3(-6f * Mathf.Cos(angleToOpponent * Mathf.Deg2Rad), 2f, -6f * Mathf.Sin(angleToOpponent * Mathf.Deg2Rad));
			//placeIconOnCircle(opponentTickList[i], 6f, angleToOpponent, 2f);
			placeIconOnCircle(opponentTickList[i], 8.2f, angleToOpponent, 0f);
			opponentTickList[i].transform.eulerAngles = new Vector3 (90.0f, 90.0f, angleToOpponent + 180);

			if (opponentDistSqList[closestOpponentId] < 15000f)
            {
				displayManagerForTick (opponentTickList [i], true, Opponents [closestOpponentId], CloseOpponent [2]);
				//opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[2];
                //opponentTickList[i].GetComponent<SpriteRenderer>().enabled = true;
                if (opponentDistSqList[closestOpponentId] < 4000f)
                {
					displayManagerForTick (opponentTickList [i], true, Opponents [closestOpponentId], CloseOpponent [1]);
                    //opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[1];
                }
				if (opponentDistSqList[closestOpponentId] < 1000f)
                {
					displayManagerForTick (opponentTickList [i], true, Opponents [closestOpponentId], CloseOpponent [0]);
                    /*opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[0];
                    //opponentTickList[i].transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "!!";
					this.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().color = CloseOpponent[0];

					foreach (Transform child in this.transform.GetChild(0).GetChild(0)) {
						child.gameObject.SetActive (true);
						if (child.gameObject.GetComponent<SpriteRenderer> () != null) {
							child.gameObject.GetComponent<SpriteRenderer> ().color = CloseOpponent [0];
							Debug.Log ("test0");
							Debug.Log (child.gameObject.name);
						}
						foreach (Transform subchild in child) {
							Debug.Log (subchild.gameObject.name);
							if (subchild.gameObject.GetComponent<RawImage> () != null) {
								Debug.Log ("test1");
								subchild.gameObject.GetComponent<RawImage> ().texture = Opponents [closestOpponentId].GetComponentInChildren<PlayerfaceImageHandler> ().rt;
						
							}
						}
					}*/
                }
            }
            else
            {
				displayManagerForTick (opponentTickList [i], false, null, CloseOpponent [0]);
                //opponentTickList[i].GetComponent<SpriteRenderer>().enabled = false;
                //opponentTickList[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            opponentDistSqList.RemoveAt(closestOpponentId);
        }
    }


    /// <summary>
    /// Returns the index of the min value of a List of floats
    /// </summary>
    /// <param name="myList"> List of floats </param>
    /// <returns>index of minimum value</returns>
    public float FindMinValueindexOfList(List<float> myList)
    {
        float minDistance = myList[0];

        for (int i = 0; i < myList.Count ; i++)
        {
            if (i == 0)
            {
                minDistance = myList[i];
            }
            if (myList[i] < minDistance)
            {
                minDistance = myList[i];
            }
        }
        return minDistance;
    }
}

/// <summary>
/// This class is used to define the list of best wind angle and worst wind angle for a certain sail ocnfiguration on a certain race direction (upwind, downwind od direct)
/// </summary>
[System.Serializable]
public class WindAnglesClass {
	public List<float> bestAngles;
	public List<float> worstAngles;

	public WindAnglesClass (List<float> b, List<float> w)
	{
		List<float> bestAngles = b;
		List<float> worstAngles = w;
	}
}
