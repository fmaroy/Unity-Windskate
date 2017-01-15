using UnityEngine;
using System.Collections;

public class Camera_track : MonoBehaviour {
	public GameObject board;
    public GameObject SailSystem;
    private Sail_System_Control SailSystemData;
    public Vector3 Camera_position_offset;
	public Vector3 Camera_Orient;
	public float CamDistance;
    public int cameraType = 0;
    public GameObject rotationControl;
    private GesturePanelScript rotationControlData;

    private float timeToGoBackToPosition = 0.5f;
    private float timer;

    private float CamRotationRef = 0.0f;
    public float CamRotation = 0f;

    //public GameObject Player;

    // Use this for initialization
    void Start () {
        SailSystemData = SailSystem.GetComponent<Sail_System_Control>();
        rotationControlData = rotationControl.GetComponent<GesturePanelScript>();
        
    }
	
	// Update is called once per frame
	
    public void CameraChange()
    {
        cameraType = cameraType + 1;
        if (cameraType > 1)
        {
            cameraType = 0 ;
        }
    }

   
    public void CamBackToPos()
    {
        CamRotationRef = CamRotation;
        timer = timeToGoBackToPosition;


}

void LateUpdate () {
        
        if (rotationControlData.isMoving == true)
        {
             CamRotation = CamRotation + rotationControlData.CamRotation;
             CamRotationRef = CamRotation;
        }
        else
        {
            timer = timer - Time.deltaTime;
            if (timer > 0)
            {
                    CamRotation = CamRotation - CamRotationRef * Time.deltaTime / timeToGoBackToPosition;
            }
            else
            {
                CamRotation = 0f;
            }
                
         }
            
            
        Vector3 CamRotationVectx = new Vector3 (board.transform.forward.x * (-1.0f+ 1* Mathf.Cos(CamRotation * Mathf.Deg2Rad)),0.0f, board.transform.forward.z* (-1.0f + 1 * Mathf.Cos(CamRotation * Mathf.Deg2Rad)));
        Vector3 CamRotationVecty = new Vector3(-1* board.transform.forward.z * Mathf.Sin(CamRotation * Mathf.Deg2Rad), 0.0f, board.transform.forward.x * Mathf.Sin(CamRotation * Mathf.Deg2Rad));

        Vector3 PlayerPos = new Vector3(board.transform.position.x, board.transform.position.y, board.transform.position.z);

        if (cameraType == 0)
        {
            CamDistance = 20;
            
            Vector3 BaseCamOrient = new Vector3(-1 * board.transform.forward.x, Camera_position_offset.y , -1 * board.transform.forward.z );
            
            //transform.position = new Vector3(board.transform.position.x - 1 * board.transform.forward.x * CamDistance , Camera_position_offset.y, Camera_position_offset.z + board.transform.position.z - 1 * board.transform.forward.z * CamDistance);
            transform.position = PlayerPos + (BaseCamOrient - CamRotationVectx + CamRotationVecty) * CamDistance;
            transform.eulerAngles = new Vector3(Camera_Orient.x, board.transform.eulerAngles.y + Camera_Orient.y + CamRotation, Camera_Orient.z);
        }
        
        if (cameraType == 1)
        { 
            float CamDistFactor = SailSystemData.Board_Speed / 40;
            if (CamDistFactor >= 1) { CamDistFactor = 1; }

            CamDistance = 18 + CamDistFactor * 3;

            float camAngleChangeSin = Mathf.Sin(SailSystemData.trueWindAngleLocal * Mathf.Deg2Rad);
            //float camAngleChangeCos = Mathf.Cos(SailSystemData.trueWindAngleLocal * Mathf.Deg2Rad);
            //float camAngleChange = 1.0f;
            float camAngleChangeDirection = camAngleChangeSin;
            if (SailSystemData.sailTiltDir.y < 0)
            {
                camAngleChangeDirection = -1 * camAngleChangeSin;
            }

            //Debug.Log(camAngleChangeSin);

            
            Vector3 BaseCamOrient = new Vector3(-1 * board.transform.forward.x  + board.transform.forward.z * camAngleChangeDirection  / 5.0f, Camera_position_offset.y, - 1 * board.transform.forward.z - board.transform.forward.x * camAngleChangeDirection  / 5.0f);

            transform.position = PlayerPos + (BaseCamOrient - CamRotationVectx + CamRotationVecty) * CamDistance;
            //transform.position = new Vector3(board.transform.position.x - 1 * board.transform.forward.x * CamDistance + board.transform.forward.z * camAngleChangeDirection * CamDistance / 5.0f, Camera_position_offset.y, Camera_position_offset.z + board.transform.position.z - 1 * board.transform.forward.z * CamDistance - board.transform.forward.x * camAngleChangeDirection * CamDistance / 5.0f);
            transform.eulerAngles = new Vector3(Camera_Orient.x, board.transform.eulerAngles.y + Camera_Orient.y - camAngleChangeDirection * 10 + CamRotation, Camera_Orient.z);
            //transform.eulerAngles = new Vector3(Camera_Orient.x, board.transform.eulerAngles.y + Camera_Orient.y , Camera_Orient.z);
        }
    }
}
