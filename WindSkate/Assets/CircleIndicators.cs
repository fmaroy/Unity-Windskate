using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleIndicators : MonoBehaviour {

    public GameObject playerObj;
    public Follow_track followTrackData;
    public GameObject trackDirectionIndicator;
    public Color trackDirColor;
    public GameObject nextTrackMark;
    public float angleBoardToMark;
    public RaceManagerScript RaceData;
    public GameObject opponentTicksContainer;
    public List<GameObject> opponentTickList = new List<GameObject>();
    public List<Color> CloseOpponent = new List<Color>();
	public GameObject apparentWindArrow;
	public GameObject trueWindArrow;

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
        //trackDirColor = trackDirectionIndicator.GetComponent<Projector>().material.GetColor("_Color");
        foreach (Transform child in opponentTicksContainer.transform)
        {
            opponentTickList.Add(child.gameObject);
        }
        RaceData = GameObject.Find("RaceManager").GetComponent<RaceManagerScript>();
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

    // Update is called once per frame
    void Update() {

        ///Handling the track direction
        nextTrackMark = followTrackData.currentMark;

        Vector3 directionToMark = nextTrackMark.transform.position - transform.position;
        float angleToMark = getOrientationAngle(nextTrackMark.transform.position);
        /*float angleToMark = Vector3.Angle(Vector3.forward,directionToMark);
        if (Vector3.Cross(Vector3.forward, directionToMark).y > 0)
        {
            angleToMark = -1 * angleToMark;
        }*/

        /*angleBoardToMark = Vector3.Angle(directionToMark, playerObj.transform.forward);
        if (Vector3.Cross(directionToMark, playerObj.transform.forward).y > 0)
        {
            angleBoardToMark = -1 * angleBoardToMark;
        }*/
        trackDirectionIndicator.transform.eulerAngles = new Vector3(90.0f, 0, angleToMark);
        float correctAngle = -1 * angleToMark - 90f;
        trackDirectionIndicator.transform.localPosition = new Vector3(30f * Mathf.Sin(correctAngle * Mathf.Deg2Rad), 30f * Mathf.Cos(correctAngle * Mathf.Deg2Rad), -10.0f);
        float srtDistToMark = Mathf.Pow(directionToMark.x, 2) + Mathf.Pow(directionToMark.z, 2);
        //Debug.Log("Dist to mark " + srtDistToMark);
        float fadeFactor = Mathf.InverseLerp(500, 8000, srtDistToMark);
        //Debug.Log("FadeFactor " + fadeFactor);
        Color col = trackDirectionIndicator.GetComponent<Projector>().material.GetColor("_Color");
        for (int i = 0; i < 3; i++)
        {
            col[i] = trackDirColor[i] * fadeFactor;
        }
        trackDirectionIndicator.GetComponent<Projector>().material.SetColor("_Color", col);

        ///Hanlding the Opoonent indicator

        //Getting the position of all the opponents 

        //List<GameObject> Opponents = RaceData.OpponenentObjectsList;
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

            opponentTickList[i].transform.localPosition = new Vector3(-6f * Mathf.Cos(angleToOpponent * Mathf.Deg2Rad), 2f, -6f * Mathf.Sin(angleToOpponent * Mathf.Deg2Rad));
            if (opponentDistSqList[closestOpponentId] < 2000f)
            {
                opponentTickList[i].GetComponent<SpriteRenderer>().enabled = true;
                opponentTickList[i].transform.GetChild(0).gameObject.SetActive(true);
                if (opponentDistSqList[closestOpponentId] > 500f)
                {
                    opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[1];
                    opponentTickList[i].transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "!";
                }
                else
                {
                    opponentTickList[i].GetComponent<SpriteRenderer>().color = CloseOpponent[0];
                    opponentTickList[i].transform.GetChild(0).gameObject.GetComponent<TextMesh>().text = "!!";
                }
            }
            else
            {
                opponentTickList[i].GetComponent<SpriteRenderer>().enabled = false;
                opponentTickList[i].transform.GetChild(0).gameObject.SetActive(false);
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

