using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPathHandler : MonoBehaviour
{
    //WeatherData
    public SailingProps SailingData;


    public float rayDistance = 2000f;
    public float testAngle = 0f;
    public Vector3 currentPos;

    //testRaySettings
    public float testTrailSpeed = 500f;
    public GameObject AITrailRenderer;


    void Start()
    {
        SailingData = GetComponent<SailingProps>();
        currentPos = transform.position;
    }

    void Update()
    {
        currentPos = transform.position;

    }


    /// <summary>
    /// Starts the upwind path.
    /// </summary>
    /// <param name="angle">angle is the optimal angle, for example 45 for upwind</param>
    public void startUpwindPath(float angle)
    {
        
        // Create 2 traces for bordId = -1 and 1
        for (int bordId = -1; bordId <= 1; bordId = bordId + 2)
        {
            Debug.Log(bordId);
            float pathAngle = SailingData.WindAngle + bordId * angle;
            Debug.Log(pathAngle);

            CreatePathInDirection(currentPos, pathAngle, rayDistance);
        }
    }

    public void testpath()
    {
        CreatePathInDirection(currentPos, testAngle, rayDistance);
    }

    public void testray()
    {
        startUpwindPath(45);
    }



    /// <summary>
    /// Calculates path with corner in a direction
    /// </summary>
    /// <param name="startPos">Start position.</param>
    /// <param name="angle">Angle.</param>
    /// <param name="distance">Distance.</param>
    public void CreatePathInDirection(Vector3 startPos, float angle, float distance)
    {
        NavMeshPath thisPath = new NavMeshPath();

        Debug.Log("Create Path from :");
        Debug.Log(startPos);
        List<Vector3> targetVectorList = new List<Vector3>();

        Vector3 target = startPos + new Vector3(distance * Mathf.Cos(angle * Mathf.Deg2Rad), 10f, distance * Mathf.Sin(angle * Mathf.Deg2Rad));

        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, thisPath);

        //Debug.Log("calculated path");
        float thisWindAngle = SailingData.WindAngle; //retrieving the Wind Angle
        Vector3 windVector = new Vector3(-1 * Mathf.Cos(thisWindAngle * Mathf.Deg2Rad), 0f, -1 * Mathf.Sin(thisWindAngle * Mathf.Deg2Rad)); //the Vector representation of the wind
        Debug.Log(windVector);
        float nextSegmentAngle = Vector3.Angle((thisPath.corners[0]-thisPath.corners[1]), windVector);
        Debug.Log("First Segment Angle : " + nextSegmentAngle);
        for (int i = 0; i < thisPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(thisPath.corners[i], thisPath.corners[i + 1], Color.red, 5, true);
        }

        RenderPathTrail(thisPath);

    }

    void RenderPathTrail(NavMeshPath pathToRender)
    {
        //Debug.Log("Rendering path");

        Vector3 startPoint = pathToRender.corners[0];
        GameObject thisTrailObj = Instantiate(AITrailRenderer, startPoint, Quaternion.identity);
        thisTrailObj.transform.position = startPoint;

        //Debug.Log("test");
        StartCoroutine (moveTrailObjOnPath(pathToRender, testTrailSpeed, thisTrailObj));
    }

    /// <summary>
    /// This function cast rays in a direction and provides with a list of corner points 
    /// </summary>
    public void ShootRaycastInDirection(Vector3 startPos, float angle, float distance)
    {
        //Debug.Log("launching Ray from :");
        Debug.Log(startPos);
        List<Vector3> targetVectorList = new List<Vector3>();

        Vector3 target = startPos + new Vector3(distance * Mathf.Cos(angle*Mathf.Deg2Rad), 10f, distance * Mathf.Sin(angle * Mathf.Deg2Rad));

        UnityEngine.AI.NavMeshHit hit;
        bool blocked = false;

        blocked = UnityEngine.AI.NavMesh.Raycast(startPos, target, out hit, UnityEngine.AI.NavMesh.AllAreas);
        Debug.DrawLine(startPos, target, blocked ? Color.blue : Color.green, 5f);

        if (blocked)
        {
            //Debug.Log("is blocked");
            Debug.DrawRay(hit.position, 500*Vector3.up, Color.red, 5, true);
        }
        rayRendering(startPos, hit.position, testTrailSpeed, AITrailRenderer);
    }



    IEnumerator moveTrailObjOnPath(NavMeshPath thisPath, float speed, GameObject objToMove)
    {
        //Debug.Log("test2");
        float i = 0f;
        //float time = 0f;
        //Debug.Log(thisPath.corners.Length);
        for (int cornerid = 0; cornerid <= thisPath.corners.Length-1; cornerid++)
        {
            //Debug.Log("rendering new path corner");
            //Debug.Log("corner id : " + cornerid);
            Vector3 startPoint = thisPath.corners[cornerid];
            Vector3 finishPoint = thisPath.corners[cornerid + 1];
            //Debug.Log("From to ");
            //Debug.Log(startPoint);
            yield return StartCoroutine(moveTrailObj(startPoint, finishPoint, speed, objToMove));
            /*i = 0;
            
            time = Vector3.Distance(startPoint, finishPoint) / speed;

            while (i <= 1)
            {
                objToMove.transform.position = Vector3.Lerp(startPoint, finishPoint, i);
                i += 0.05f;
                yield return new WaitForSeconds(time / 100f);

            }
            */
        }
    }

    /// <summary>
    /// Handle the spanwing and moving the trail to highlinght the trace of the path
    /// </summary>
    /// <param name="startPoint">Start point.</param>
    /// <param name="finishPoint">Finish point.</param>
    /// <param name="speed">Speed.</param>
    /// <param name="renderingObj">Rendering object.</param>
    public void rayRendering(Vector3 startPoint, Vector3 finishPoint, float speed, GameObject renderingObj)
    {
        Debug.Log(finishPoint);
        GameObject thisTrailObj = Instantiate(renderingObj, startPoint, Quaternion.identity);
        thisTrailObj.transform.position = startPoint;

        StartCoroutine(moveTrailObj(startPoint, finishPoint, speed, thisTrailObj));
    }

    IEnumerator moveTrailObj(Vector3 startPoint, Vector3 finishPoint, float speed, GameObject objToMove)
    {
        //Debug.Log("Rendering trace from to:");
        //Debug.Log(startPoint);
        //Debug.Log(finishPoint);
        //GameObject thisTrailObj = Instantiate(objToMove, startPoint, Quaternion.identity);
        //thisTrailObj.transform.position = startPoint;*/

        float i = 0;
        float time = Vector3.Distance(startPoint, finishPoint) / speed;
        while (i <= 1)
        {
            objToMove.transform.position = Vector3.Lerp(startPoint, finishPoint, i);
            i += 0.05f;
            yield return new WaitForSeconds(time / 100f);

        }
        objToMove.transform.position = finishPoint;
        //Debug.Log("Finished to render Trace");
        yield return null;
    }
}
[System.Serializable]
public class AISailPathBranch
{
    public float cost;
    public List<Vector3> branch;


    public AISailPathBranch(List<Vector3> p)
    {
        branch = p;
    }
}