using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayersTrackOnRacetrack : MonoBehaviour
{

    private GameObject trackMarksContainerObject;
    private Setup_track trackDefinition;

    public List<int> currentMarkList = new List<int>();
    public List<float> positionBetweenMarks = new List<float>();
    //Ranking per player
    public List<int> rankingList = new List<int>();

    public List<GameObject> PlayersList = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        trackMarksContainerObject = GameObject.Find("Track_Marks");
        trackDefinition = trackMarksContainerObject.GetComponent<Setup_track>();

        //Initialization of all the lists;
        currentMarkList = new List<int>();
        positionBetweenMarks = new List<float>();
        rankingList = new List<int>();
        
        PlayersList.Add(this.gameObject.GetComponent<RaceManagerScript>().PlayerObject);
        rankingList.Add(0);
        currentMarkList.Add(0);
        positionBetweenMarks.Add(0.0f);
        foreach (GameObject op in this.gameObject.GetComponent<RaceManagerScript>().OpponenentObjectsList)
        {
            PlayersList.Add(op);
            rankingList.Add(0);
            currentMarkList.Add(0);
            positionBetweenMarks.Add(0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*currentMarkList = new List<int>();
        positionBetweenMarks = new List<float>();
        rankingList = new List<int>();

        //build list of player + opponenents
        List<GameObject> PlayersList = new List<GameObject>();
        //first value is the player
        PlayersList.Add(this.gameObject.GetComponent<RaceManagerScript>().PlayerObject);
        rankingList.Add(0);
        foreach (GameObject op in this.gameObject.GetComponent<RaceManagerScript>().OpponenentObjectsList)
        {
            PlayersList.Add(op);
            //Initialize ranking list at each frame
            rankingList.Add(0);
        }*/

        //Create and update list of position of player on track (in between which mark each is)
        int i = 0;
        foreach (GameObject player in PlayersList)
        {
            rankingList[i] = 0;
            try
            {
                currentMarkList[i] = player.GetComponentInChildren<Follow_track>().currentMarkId;
                //test
                Vector3 PlayerPos = player.GetComponentInChildren<Follow_track>().gameObject.transform.position;
                Vector3 NextMarkPos = GameObject.Find(trackDefinition.markSequence[currentMarkList[i]]).transform.position;
                Vector3 PreviousMarkPos;
                // need to create special case for first mark since there is no previous mark
                if (currentMarkList[i] == 0)
                {
                    PreviousMarkPos = PlayersList[0].transform.position;
                }
                else
                {
                    PreviousMarkPos = GameObject.Find(trackDefinition.markSequence[currentMarkList[i] - 1]).transform.position;
                }
                float squarePlayerDistToPrevMark = (PlayerPos.x - PreviousMarkPos.x) * (PlayerPos.x - PreviousMarkPos.x) + (PlayerPos.z - PreviousMarkPos.z) * (PlayerPos.z - PreviousMarkPos.z);
                float squareDistBetweenMark = (NextMarkPos.x - PreviousMarkPos.x) * (NextMarkPos.x - PreviousMarkPos.x) + (NextMarkPos.z - PreviousMarkPos.z) * (NextMarkPos.z - PreviousMarkPos.z);
                positionBetweenMarks[i] = squarePlayerDistToPrevMark / squareDistBetweenMark;

            }
            catch 
            {
                Debug.Log("Can't grab currentMarkId of : " + PlayersList[i].name);
            }

            i++;
        }
        //Get positon of each player between the marks, the higher the more advanced
        /*
        i = 0;
        foreach (GameObject player in PlayersList)
        {
            // next is a trick to get the object board assembly
            //Debug.Log(player.GetComponentInChildren<Follow_track>().gameObject.name);
            Vector3 PlayerPos = player.GetComponentInChildren<Follow_track>().gameObject.transform.position;
            // get next trackpos for the given player
            //Debug.Log("currentMarkId : " + currentMarkList[i]);
            //Debug.Log("currentMark : " + GameObject.Find(trackDefinition.markSequence[currentMarkList[i]]).name);
            Vector3 NextMarkPos = GameObject.Find(trackDefinition.markSequence[currentMarkList[i]]).transform.position;
            Vector3 PreviousMarkPos;
            // need to create special case for first mark since there is no previous mark
            if (currentMarkList[i] == 0)
            {
                PreviousMarkPos = PlayersList[0].transform.position;
            }
            else
            {
                PreviousMarkPos = GameObject.Find(trackDefinition.markSequence[currentMarkList[i] - 1]).transform.position;
            }
            float squarePlayerDistToPrevMark = (PlayerPos.x - PreviousMarkPos.x) * (PlayerPos.x - PreviousMarkPos.x) + (PlayerPos.z - PreviousMarkPos.z) * (PlayerPos.z - PreviousMarkPos.z);
            float squareDistBetweenMark = (NextMarkPos.x - PreviousMarkPos.x) * (NextMarkPos.x - PreviousMarkPos.x) + (NextMarkPos.z - PreviousMarkPos.z) * (NextMarkPos.z - PreviousMarkPos.z);
            positionBetweenMarks[i] = squarePlayerDistToPrevMark / squareDistBetweenMark;
            
            i++;
        }
        */
        //Debug.Log("Next Track: " + currentMarkList[0] + "Position between tracks: " + positionBetweenMarks[0]);

        //trying to make a single list ocmbining the currentMarkList *10 and positionBetweenMarks values
        List<float> TempRankingList = new List<float>();
        List<int> alreadyFoundIDs = new List<int>();
        for (i = 0; i < rankingList.Count; i++)
        {
            TempRankingList.Add(currentMarkList[i]*10 + positionBetweenMarks[i]);
        }
        // to achieve the ranking we cycle through TempRankingList, check the current value against the other value. each time an other value is lower that the current one, the corresponding rankingList gets a +1
        for (i = 0; i < rankingList.Count; i++)
        {
            //float currentPlayerPos = 0.0f;
            //if (i != 0) { currentPlayerPos = rankingList[i-1]; }
            int otherID = 0;
            foreach (float otherpos in TempRankingList)
            {
                if(TempRankingList[i] > otherpos)
                {
                    rankingList[otherID] = rankingList[otherID] + 1;
                }
                otherID++;
            }
        }
    }

}
