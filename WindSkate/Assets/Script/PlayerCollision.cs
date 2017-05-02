using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCollision : MonoBehaviour
{
    //public PlayerProperties thisPlayerProps;
    public int opponentId = 0;
    public bool colllisonFlag = false;
    public GameObject Board;
    private Follow_track BoardFollowTrack;
    public GameObject SailSystem;
    private Sail_System_Control SailSystemData;
    private Animator Sail_System_Anim;
    public GameObject Character;
    private CharacterBehavior CharacterData;
    public GameObject NavmeshAgent;
    public float timeBeforeReset = 2.0f;
    private float crashTimer;
    public Vector3 playerPositionAtCrash;
    public float playerOrientationAtCrash;
    public Vector3 playerOrientationVectorAtCrash;
    public bool isDrivingStarboardAtCrash;
    public int inTargetMarkAtCrash;
    public List<GameObject> playerRecoveryObjectsList = new List<GameObject>();
    public GameObject playerRecoveryObject;
    public Vector3 playerRecoveryPosition;
    public float playerRecoveryOrientation;
    public bool isCrashed;
    public windEffector windData;

    public bool isPlayer = false;
    private bool prevIsPlayer = false;
    public bool ManualDrive;
    private bool prevManualDrive;

    private GameObject currentPlayer;
    private bool playCollision = false;
    private bool isInResettingState = false;
    private float resettingTimer = 0.0f;
    private Vector3 previousVelocity = new Vector3(0.0f,0.0f,0.0f);

    private Vector3 velocityBeforeCrash = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 angularVelocityBeforeCrash = new Vector3(0.0f, 0.0f, 0.0f);

    private float lowSpeedTimer ;
    private bool lowSpeedFlag = false;

    public List<Rigidbody> rigidBodiesList = new List<Rigidbody>();
    public List<Rigidbody> rigidBodiesListDynamics = new List<Rigidbody>();
    public List<Collider> colliderList = new List<Collider>();
    public List<GameObject> objectList = new List<GameObject>();
    public List<MeshRenderer> meshRendererList = new List<MeshRenderer>();
    public int currentFrame = 0;
    public bool resetting = false;
    private ConfigurableJoint sailJoint;
    private Vector3 sailJointAnchor;
    private Vector3 sailJointConnectedAnchor;

    public GameObject crashFxObj;
    private float crashFxtimer = 0.0f;
    private float crashFxTimeDelay = 1.0f;
    

    // Use this for initialization
    void Start()
    {
        prevIsPlayer = isPlayer;
        ManualDrive = isPlayer;
        prevManualDrive = ManualDrive;

        CharacterData = Character.GetComponent<CharacterBehavior>();
        SailSystemData = SailSystem.GetComponent<Sail_System_Control>();
        Sail_System_Anim = SailSystem.GetComponent<Animator>();
        windData = SailSystem.GetComponent<windEffector>();
        //Next is because this script call a function requiring windData in Follow_track
        Board.GetComponent<Follow_track>().windData = windData;

        BoardFollowTrack = Board.GetComponent<Follow_track>();
        CharacterData.DisableRagdoll();
        Sail_System_Anim.enabled = true;
        SailSystemData.isFalling = false;
        crashTimer = 0.0f;

        currentPlayer = this.gameObject;
        isInResettingState = false;

        updateManualDrive();

        colliderList = getPlayerCollider(currentPlayer);
        //rigidBodiesList = new List<Rigidbody>();
        rigidBodiesList = getPlayerRigidBody(currentPlayer);
        rigidBodiesListDynamics = getPlayerRigidBodyDynamics(currentPlayer);
        meshRendererList = getPlayerMeshRenderer(currentPlayer);
        sailJoint = SailSystem.GetComponent<ConfigurableJoint>();
        sailJointAnchor = sailJoint.anchor;
        sailJointConnectedAnchor = sailJoint.connectedAnchor;
        SailSystem.GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = false;
        resetting = false;
        if (isPlayer == false)
        {
            foreach (Transform child in this.gameObject.transform)
                {
                if (child.gameObject.name.Contains("Canvas_WindCircle"))
                {
                    child.gameObject.SetActive(false);
                }

            }
        }

    }

    /// <summary>
    /// Makes sure everything work when ManualDrive is switched on and off
    /// </summary>
    public void updateManualDrive()
    {
        //Debug.Log("Update Manual Drive : " + ManualDrive);
        initiatePlayerAndOpponent();
        foreach(Transform child in this.gameObject.transform)
        {
            if (child.gameObject.GetComponent<Follow_track>() != null)
            {
                child.gameObject.GetComponent<Follow_track>().updateManualDriveFollowTrack();
                child.gameObject.GetComponent<BoardForces>().localManualDrive = ManualDrive;
                //child.gameObject.GetComponent<BoardForces>().localManualDrive = ManualDrive;
            }
        }
        
        prevManualDrive = ManualDrive;
    }
    /// <summary>
    /// Based on the value Manual Drive, this turns on and off the Navmesh Obstacle and Agent. 
    /// Should not be used alone. Use Update Manual Drive instead.
    /// </summary>
    public void initiatePlayerAndOpponent()
    {
        if (ManualDrive == false)
        {
            NavmeshAgent.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
            NavmeshAgent.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        }
        else
        {

            NavmeshAgent.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            NavmeshAgent.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
        }
    }
    public List<Collider> getPlayerCollider(GameObject rootObject)
    {
        List<Collider> buildColliderList = new List<Collider>();
        List<GameObject> currentObjBranch = recursiveChildrenSearch(rootObject);
        List<GameObject> buildObjectList = new List<GameObject>();
        foreach (GameObject objToAdd in currentObjBranch)
        {
            buildObjectList.Add(objToAdd);
        }
        while (currentObjBranch != null)
        {
            foreach (GameObject obj in currentObjBranch)
            {
                // new List of Object to be recusively searched on next round;
                currentObjBranch = recursiveChildrenSearch(obj);
                // Retained list of objects
                if (currentObjBranch != null)
                {
                    foreach (GameObject objToAdd in currentObjBranch)
                    {
                        buildObjectList.Add(objToAdd);
                    }
                }
            }
        }
        foreach (GameObject currentObject in buildObjectList)
        {
            if (currentObject.GetComponent<Collider>() != null)
            {
                //Debug.Log("Object " + currentObject.name + ", " + currentObject.GetComponent<Collider>());
                buildColliderList.Add(currentObject.GetComponent<Collider>());
            }
        }
        return buildColliderList;
    }

    public List<MeshRenderer> getPlayerMeshRenderer(GameObject rootObject)
    {
        List<MeshRenderer> buildMeshRendererList = new List<MeshRenderer>();
        List<GameObject> currentObjBranch = recursiveChildrenSearch(rootObject);
        List<GameObject> buildObjectList = new List<GameObject>();
        foreach (GameObject objToAdd in currentObjBranch)
        {
            buildObjectList.Add(objToAdd);
        }
        while (currentObjBranch != null)
        {
            foreach (GameObject obj in currentObjBranch)
            {
                // new List of Object to be recusively searched on next round;
                currentObjBranch = recursiveChildrenSearch(obj);
                // Retained list of objects
                if (currentObjBranch != null)
                {
                    foreach (GameObject objToAdd in currentObjBranch)
                    {
                        buildObjectList.Add(objToAdd);
                    }
                }
            }
        }
        foreach (GameObject body in buildObjectList)
        {
            if (body.GetComponent<MeshRenderer>()!=null)
            {
                if (body.GetComponent<MeshRenderer>().enabled == true)
                {
                    buildMeshRendererList.Add(body.GetComponent<MeshRenderer>());
                }
            }
        }
        return buildMeshRendererList;
    }
    public List<Rigidbody> getPlayerRigidBodyDynamics(GameObject rootObject)
    {
        List<Rigidbody> buildRbList = new List<Rigidbody>();
        List<Rigidbody> RbListOnlyDynamics = new List<Rigidbody>();
        buildRbList = getPlayerRigidBody(rootObject);
        foreach (Rigidbody body in buildRbList)
        {
            if (body.isKinematic == false)
            {
                RbListOnlyDynamics.Add(body);
            }
        }
        return buildRbList;
    }
    public List<Rigidbody> getPlayerRigidBody(GameObject rootObject)
    {
        List<Rigidbody> buildRbList = new List<Rigidbody>();
        List<GameObject> currentObjBranch = recursiveChildrenSearch(rootObject);
        List<GameObject> buildObjectList = new List<GameObject>();
        foreach (GameObject objToAdd in currentObjBranch)
        {
            buildObjectList.Add(objToAdd);
        }
        while (currentObjBranch != null)
        {
            foreach (GameObject obj in currentObjBranch)
            {
                // new List of Object to be recusively searched on next round;
                currentObjBranch = recursiveChildrenSearch(obj);
                // Retained list of objects
                if (currentObjBranch != null)
                {
                    foreach (GameObject objToAdd in currentObjBranch)
                    {
                        buildObjectList.Add(objToAdd);
                    }
                }
            }
        }
        foreach (GameObject currentObject in buildObjectList)
        {
            if (currentObject.GetComponent<Rigidbody>() != null)
            {
                buildRbList.Add(currentObject.GetComponent<Rigidbody>());
            }
        }
        return buildRbList;
    }

    public List<GameObject> recursiveChildrenSearch (GameObject searchObject)
    {
        List<GameObject> childrenObject = new List<GameObject> ();
        foreach (Transform child in searchObject.transform)
        {
            //Debug.Log(child.gameObject.name);
            childrenObject.Add(child.gameObject);
        }
        if (childrenObject.Count == 0)
        {   
            childrenObject = null;
        }
        return childrenObject;
    }

    public GameObject recursiveParentSearch(GameObject searchObject)
    {
        GameObject parentObject = searchObject;
        if (searchObject.transform.parent != null)
        {
            parentObject = searchObject.transform.parent.gameObject;
            //Debug.Log("ParentObject: " + parentObject.name);
            return parentObject;
        }
        else
        {
            return null;
        }
    }

    public void playerCollisionHandling(GameObject callingObject, GameObject CollisionObject)
    {
        //Debug.Log("CallingObject : " + callingObject.name + ", CollisionObject : " + CollisionObject.name);
        GameObject rootObject = CollisionObject;
        playCollision = false;

        if (colllisonFlag == false)
        {
            //Check if Collider is not part of the current player

            while (CollisionObject != currentPlayer)
            {
                if (rootObject == null)
                {
                    //Debug.Log("CollisionObject is valid");
                    playCollision = true;
                    break;
                }
                //Debug.Log(rootObject.name);
                //Debug.Log(currentPlayer.name);
                if ((rootObject == currentPlayer) || (rootObject.CompareTag("Terrain") == true))
                {
                    //Debug.Log("Self Collision or Terrain collision");
                    break;
                }
                //Debug.Log("Searching Parent of : " + rootObject.name);
                rootObject = recursiveParentSearch(rootObject);
            }
            if (playCollision == true)
            {
                playerCrashed();
            }
            else
            {
                return;
            }
        }
    }


    IEnumerator waitBeforeDisablingFX(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    public void playerCrashed()
    {
        if (isPlayer == true)
        {
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.01f * Time.timeScale;
            SailSystem.GetComponent<SailAnimScript>().StartManoeuvreFX(0.5f);
			Camera.main.GetComponent<CameraControlScript> ().dimCameraEffect ();
			this.GetComponent<ExternalObjectsReference> ().UIControlData.MessageText.GetComponent<messageHandler> ().throwMessage (1);
        }
        SailSystem.GetComponent<windEffector>().resetWindModifier();
        // Trigger Crash Fx
        if (isPlayer == true)
        {
            crashFxObj.SetActive(false);
            crashFxObj.SetActive(true);
            StartCoroutine(waitBeforeDisablingFX(crashFxObj, 1.0f));
        }
       
        if (isPlayer == true)
        {
            ManualDrive = true;
            updateManualDrive();
        }
        CharacterData.EnableRagdoll(velocityBeforeCrash, angularVelocityBeforeCrash);
        colllisonFlag = true;
        Sail_System_Anim.enabled = false;
        SailSystemData.isFalling = true;
        
        SailSystemData.SailRigidBody.isKinematic = false;
        SailSystemData.SailRigidBody.velocity = velocityBeforeCrash;
        SailSystemData.SailRigidBody.angularVelocity = angularVelocityBeforeCrash;
        SailSystemData.SailBone.AddComponent<FixedJoint>();
        SailSystemData.SailBone.GetComponent<FixedJoint>().connectedBody = SailSystem.GetComponent<Rigidbody>();
        crashTimer = 0.0f;
        playerPositionAtCrash = Board.transform.position;
        playerOrientationAtCrash = Board.transform.eulerAngles.y;
        playerOrientationVectorAtCrash = Board.transform.forward;
        //Debug.Log("Board Orientation Vector saved : " + playerOrientationVectorAtCrash);
        isDrivingStarboardAtCrash = BoardFollowTrack.driveStarboard;
        inTargetMarkAtCrash = BoardFollowTrack.getRacerStatusOnTrack(BoardFollowTrack.angleMarkToWind);
        /*Debug.Log("Get Target Mark At Crash : " + inTargetMarkAtCrash);
        Debug.Log("Registered Position At Crash: " + playerPositionAtCrash);
        Debug.Log("Registered Orientation At Crash: " + playerOrientationAtCrash);*/

        

    }

    /*public void playerReposition()
    {
        playerPositionAtCrash = Board.transform.position;
        playerOrientationAtCrash = Board.transform.eulerAngles.y;
        isDrivingStarboardAtCrash = BoardFollowTrack.driveStarboard;
        inTargetMarkAtCrash = BoardFollowTrack.getRacerStatusOnTrack(BoardFollowTrack.angleMarkToWind);
    }*/

    public int getMinValueInList (List<float> floatList, int grade)
    {
        int index;
        float currentValue = 0.0f;
        // only way to initialize the list and breaking the link to the list floatList
        List<float> tempList = new List<float>();
        foreach (float val in floatList) { tempList.Add(val); }
        int i = 0;
        // as long as the value i doesn't reach "grade" remove the minimum value from the temp list
        while (i <= grade)
        {
            int j = 0;
            foreach (float value in tempList)
            {
                //set currentValue for the first cycle
                if (j == 0) { currentValue = value; }
                
                if (value < currentValue)
                {
                    currentValue = value;
                }
                j++;
            }
            // at this point "current value" is the smallest value of the temp list
            // removing the min value "currentValue" from the list
            tempList.RemoveAt(tempList.IndexOf(currentValue));
            
            i++;
        }
        // all the samllest value defined by "grade" has been removed
        //retrieve the index of the last smallest value "currentValue"
        index = floatList.IndexOf(currentValue);
        return index;
    }

    void getPositionAfterCrash()
    {
        resetting = true;
        //Debug.Log("Get Position After Crash new");
        GameObject targetedMark = BoardFollowTrack.currentMark;
        //Debug.Log("Targeted Mark : " + targetedMark);
        playerRecoveryObjectsList = new List<GameObject>();
        //get next target mark if no current Mark is found
        if (targetedMark.GetComponent<Mark>().ResetPositionParent == null)
        {
            //get next mark in track
            targetedMark = GameObject.Find(BoardFollowTrack.trackData.markSequence[BoardFollowTrack.currentMarkId + 1]);
        }

        //get vector from player to reposition object
        Vector3 VectorDist = playerPositionAtCrash - targetedMark.GetComponent<Mark>().ResetPositionsList[0].transform.position;
        // get square distance from player to reposition object
        float SquareDist = VectorDist.x * VectorDist.x + VectorDist.z * VectorDist.z;
        // browse each available reposition object
        List<float> distanceToReposObjectList = new List<float>();
        foreach (GameObject resetPosObj in targetedMark.GetComponent<Mark>().ResetPositionsList)
        {
            //calculate distance for each reposition object
            VectorDist = playerPositionAtCrash - resetPosObj.transform.position;
            float SquareDist_temp = VectorDist.x * VectorDist.x + VectorDist.z * VectorDist.z;
            distanceToReposObjectList.Add(SquareDist_temp);
        }
        // find minimum distance object index in list
        List<GameObject> localObjList = targetedMark.GetComponent<Mark>().ResetPositionsList;
        int shortestDist = getMinValueInList(distanceToReposObjectList,0);
        int secondShortDist = getMinValueInList(distanceToReposObjectList, 1);
        // add the 2 closest objets to the list of reset positions
        playerRecoveryObjectsList = new List<GameObject>();
        playerRecoveryObjectsList.Add(localObjList[shortestDist]);
        playerRecoveryObjectsList.Add(localObjList[secondShortDist]);
        
        ////Debug.Log(playerRecoveryObjectsList[0].name);
        Debug.Log(playerRecoveryObjectsList[1].name);

        playerRecoveryPosition = playerRecoveryObjectsList[0].transform.position;


        checkPlayerRepositioningArea();

        Vector3 nextTarget = new Vector3 (1.0f, 0.0f, 0.0f);

        if (isPlayer)
        {
            ManualDrive = false;
        }
        updateManualDrive();

   
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.gameObject.GetComponent<Follow_track>() != null)
            {
                child.gameObject.GetComponent<Follow_track>().updatePlayerBoard(playerRecoveryPosition);
                int intNextMark = child.gameObject.GetComponent<Follow_track>().isNextTargetMark;

                nextTarget = child.gameObject.GetComponent<Follow_track>().NavMeshHandling(intNextMark, child.gameObject.GetComponent<Follow_track>().driveStarboard, playerRecoveryPosition);
            }
        }
        
        if (isPlayer)
        {
            ManualDrive = true;
        }
        updateManualDrive();

        Vector3 diffVector = playerRecoveryPosition - nextTarget;
        float boardAngle = Vector3.Angle(diffVector, Vector3.right);
        if (Vector3.Cross(Vector3.right, diffVector).y < 0)
        {
            boardAngle = -1 * boardAngle;
        }
        
        playerRecoveryOrientation =  boardAngle + 180;
        //Debug.Log("Board Direction to set : " + playerRecoveryOrientation);
        SailSystem.GetComponent<SailAnimScript>().EndManoeuvreFX();

    }

    public void checkPlayerRepositioningArea()
    {
        List<GameObject> otherPlayerObj = new List<GameObject>();
        List<Vector3> otherPlayerPos = new List<Vector3>();
        RaceManagerScript raceData = GameObject.Find("RaceManager").GetComponent<RaceManagerScript>();
        //Build list of other player game object
        foreach (Transform obj in raceData.OpponentContainerObject.transform)
        {
            if (obj.gameObject != this.gameObject)
            {
                otherPlayerObj.Add(obj.gameObject);
            }
        }
        if (isPlayer == false)
        {
            otherPlayerObj.Add(raceData.PlayerObject);
        }
        //Debug.Log("List of other players (" +this.gameObject.name+ ") : " + otherPlayerObj.Count);

        foreach (GameObject obj in otherPlayerObj)
        {
            //Check the player that are being repositioned
            if (obj.GetComponent<PlayerCollision>().playerRecoveryPosition != new Vector3(0.0f, 0.0f, 0.0f))
            {
                otherPlayerPos.Add(obj.GetComponent<PlayerCollision>().playerRecoveryPosition);
            }
            else
            {
                //get actual position of other players that are not being repositioned
                //To be refine with predicted direction
                otherPlayerPos.Add(obj.GetComponent<PlayerCollision>().playerRecoveryPosition);
            }
        }
        //browse each position between recovery object position A and B
        for (float i = 0.0f; i < 1 ; i = i + 0.1f)
        {
            //get position between the 2 Object recovery objects
            Vector3 currentPos = Vector3.Lerp(playerRecoveryObjectsList[0].transform.position, playerRecoveryObjectsList[1].transform.position, i);

            //get dist to each other player
            bool posAlreadyTaken = false;
            foreach (Vector3 pos in otherPlayerPos)
            {
                //get distance between checking position and the other players
                Vector3 vectDistToOtherPlayer = currentPos - pos;
                float sqDistToOtherPlayer = vectDistToOtherPlayer.x * vectDistToOtherPlayer.x + vectDistToOtherPlayer.z * vectDistToOtherPlayer.z;
                //check if other player are too close to curretn checking position
                if (sqDistToOtherPlayer < 8.0f)
                {
                    posAlreadyTaken = true;
                    // Not necessary to look further, checking the next position
                    break;
                }
            }
            // current spot id free!!
            if (posAlreadyTaken == false)
            {
                // Updating current recovery position
                playerRecoveryPosition = currentPos;
                break;
            }
        }
    }

    /*IEnumerator resetAfterCrashPart2(Rigidbody Rb)
    {
        yield return new WaitForSeconds(0.02f);
        Rb.isKinematic = false;
        foreach (Rigidbody rb in rigidBodiesListDynamics)
        {
            rb.isKinematic = false;
        }
        foreach (Collider col in colliderList)
        {
            col.enabled = true;
        }
        foreach (MeshRenderer mesh in meshRendererList)
        {
            mesh.enabled = true;
        }
        // Test : Moved from resetAfterCrashPart

        CharacterData.DisableRagdoll();
        Sail_System_Anim.enabled = true;
        SailSystemData.SailRigidBody.isKinematic = true;
        SailSystemData.SailBone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        SailSystemData.isFalling = false;
    }*/

    /*IEnumerator resetAfterCrash()
    {
        yield return new WaitForSeconds(0.02f);

        colllisonFlag = false;
        //Debug.Log("Reset");
        

        float trackLeftSideBoundary = BoardFollowTrack.currentLeftSideBoundary;
        float trackRightSideBoundary = BoardFollowTrack.currentRightSideBoundary;

        getPositionAfterCrash();

        playerOrientationAtCrash = playerRecoveryOrientation;

        foreach (Rigidbody rb in rigidBodiesListDynamics)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.inertiaTensorRotation = Quaternion.identity;
            rb.ResetInertiaTensor();
            rb.isKinematic = true;
        }

        foreach (Collider col in colliderList)
        {
            col.enabled = false;
        }
        foreach (MeshRenderer mesh in meshRendererList)
        {
            mesh.enabled = false;
        }

        Board.transform.position = playerRecoveryPosition + new Vector3(0.0f, 2.0f, 0.0f);
        Board.transform.eulerAngles = new Vector3 (0.0f, playerOrientationAtCrash, 0.0f);

        StartCoroutine(resetAfterCrashPart2(Board.GetComponent<Rigidbody>()));

        Destroy( SailSystemData.SailBone.GetComponent<FixedJoint>() );
        
        crashTimer = 0.0f;
        colllisonFlag = false;
        isInResettingState = true;
        resettingTimer = 0.0f;
        previousVelocity = Board.GetComponent<Rigidbody>().velocity;

        SailSystem.GetComponent<windEffector>().resetWindModifier();

    }*/

    void lowSpeedDetected()
    {
        lowSpeedFlag = true;
        lowSpeedTimer = 0.0f;
    }

    void LateUpdate()
    {
        velocityBeforeCrash = Board.GetComponent<Rigidbody>().velocity;
        angularVelocityBeforeCrash = Board.GetComponent<Rigidbody>().angularVelocity;
        prevIsPlayer = isPlayer;
        prevManualDrive = ManualDrive;
    }
    void repositionPlayerDisabledRb()
    {
		
		this.GetComponent<ExternalObjectsReference>().currentCamera.GetComponent<CameraControlScript> ().resetVignetteCameraEffect ();

        getPositionAfterCrash();
        colllisonFlag = false;
        float trackLeftSideBoundary = BoardFollowTrack.currentLeftSideBoundary;
        float trackRightSideBoundary = BoardFollowTrack.currentRightSideBoundary;
        //Get previous player orientation
        playerOrientationAtCrash = playerRecoveryOrientation;

        Destroy(SailSystemData.SailBone.GetComponent<FixedJoint>());
        //Destroy(SailSystem.GetComponent<ConfigurableJoint>());
        foreach (Rigidbody rb in rigidBodiesListDynamics)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.gameObject.SetActive(false);
        }
        currentFrame = 0;
        resetting = true;
        /*foreach (MeshRenderer mesh in meshRendererList)
        {
            mesh.enabled = false;
        }
        */
        StartCoroutine(repositionPlayerEnableRb());
    }

    IEnumerator repositionPlayerEnableRb()
    {
        yield return new WaitForSeconds(0.02f);
        //reset Objects
        
        
        SailSystemData.SailRigidBody.isKinematic = true;
        SailSystemData.SailBone.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        SailSystemData.SailBone.transform.eulerAngles = new Vector3(0.0f, 1.0f, 0.0f);
        
        Board.transform.position = playerRecoveryPosition + new Vector3(0.0f, 2.0f, 0.0f);
        //Debug.Log("Board orient before : " + Board.transform.localEulerAngles);
        Board.transform.localEulerAngles = new Vector3(0.0f, playerOrientationAtCrash, 0.0f);
        //Debug.Log("Board orient after : " + Board.transform.localEulerAngles);
        SailSystem.transform.position = Board.transform.position + new Vector3 (0.0f, 0.8f, 0.1699f);
        SailSystem.transform.eulerAngles = Board.transform.eulerAngles + new Vector3(0.0f, 90.0f, 0.0f);

        foreach (Rigidbody rb in rigidBodiesListDynamics)
        {
            rb.gameObject.SetActive(true);
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        SailSystemData.isFalling = false;
        CharacterData.DisableRagdoll();

        //SailSystem.AddComponent<ConfigurableJoint>();
        SailSystem.GetComponent<ConfigurableJoint>().autoConfigureConnectedAnchor = false;
        SailSystem.GetComponent<ConfigurableJoint>().anchor = sailJointAnchor;
        SailSystem.GetComponent<ConfigurableJoint>().connectedAnchor = sailJointConnectedAnchor;

        Sail_System_Anim.enabled = true;
        

        foreach (Collider col in colliderList)
        {
            col.enabled = true;
        }
        currentFrame = 0;
        resetting = false;
        //Debug.Log("Board orient end : " + Board.transform.localEulerAngles);
    }
		
    void Update()
    {
        if (prevManualDrive != ManualDrive)
        {
            updateManualDrive();
        }

        if (colllisonFlag == true)
        {
            crashTimer = crashTimer + Time.deltaTime;
            if (crashTimer > timeBeforeReset)
            {
                //StartCoroutine(resetAfterCrash());
                repositionPlayerDisabledRb();
            }
        }
        
        //if (SailSystemData.rbBoard.velocity.x < 0f)

        if (lowSpeedFlag == false)
        {
            if (SailSystemData.Board_Speed < 4.0f)
            {
                lowSpeedDetected();
            }
        }
        else
        { 
            
            if (lowSpeedTimer > 2.0f)
            {
                resetting = false;
                if (SailSystemData.Board_Speed < 4.0f)
                {
                    //StartCoroutine(resetAfterCrash());
                    repositionPlayerDisabledRb();
                    lowSpeedTimer = 0.0f;
                    lowSpeedFlag = false;
                }
                else
                {
                    lowSpeedTimer = 0.0f;
                    lowSpeedFlag = false;
                }
            }
            else
            {
                lowSpeedTimer = lowSpeedTimer + Time.deltaTime;
            }
        }

 

        if (isInResettingState == true)
        {
            if (resettingTimer < 1.0f)
            {
                if (Board.GetComponent<Rigidbody>().velocity.y > 5)
                {
                    //Debug.Log("Reset because y velocity = " + Board.GetComponent<Rigidbody>().velocity.y);
                    repositionPlayerDisabledRb();
                }
                
                resettingTimer = resettingTimer + Time.deltaTime;


            }
            else
            {
                //reseting parameters after crash
                isInResettingState = false;
                playerRecoveryPosition = new Vector3(0.0f, 0.0f, 0.0f);
                if (previousVelocity.magnitude + 1 > Board.GetComponent<Rigidbody>().velocity.magnitude)
                {
                    ///Debug.Log("Reset because velocity magnitude after 0.5 s = " + Board.GetComponent<Rigidbody>().velocity.magnitude + 2 + "vs previous vel = " + previousVelocity.magnitude);
                    repositionPlayerDisabledRb();
                }
            }
        }
    }
}
