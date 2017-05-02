using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIndicators : MonoBehaviour {

    public GameObject playerObj;
    public Follow_track followTrackData;
    public GameObject trackDirectionIndicator;
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

    // Update is called once per frame
    void Update() {

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
			placeOnCircle (trackDirectionTickIcon, 6f, correctAngle, -3.67f);
		}

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

		placeOnCircle (trackDirectionIndicator, 30f, correctAngle, -10f);

		float srtDistToMark = Mathf.Pow(directionToMark.x, 2) + Mathf.Pow(directionToMark.z, 2);
        float fadeFactor = Mathf.InverseLerp(500, 8000, srtDistToMark);
        //Debug.Log("FadeFactor " + fadeFactor);
        Color col = trackDirectionIndicator.GetComponent<Projector>().material.GetColor("_Color");
        for (int i = 0; i < 3; i++)
        {
            col[i] = trackDirColor[i] * fadeFactor;
        }
        trackDirectionIndicator.GetComponent<Projector>().material.SetColor("_Color", col);

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
            opponentTickList[i].GetComponent<SpriteRenderer>().enabled = false;
            opponentTickList[i].transform.GetChild(0).gameObject.SetActive(false);
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
				opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[2];
                opponentTickList[i].GetComponent<SpriteRenderer>().enabled = true;
                //opponentTickList[i].transform.GetChild(0).gameObject.SetActive(true);
                if (opponentDistSqList[closestOpponentId] < 4000f)
                {
                    opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[1];
                    //opponentTickList[i].transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "!";
                }
				if (opponentDistSqList[closestOpponentId] < 1000f)
                {
                    opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[0];
                    //opponentTickList[i].transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "!!";
                }
            }
            else
            {
                opponentTickList[i].GetComponent<SpriteRenderer>().enabled = false;
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

