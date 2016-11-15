using UnityEngine;
using System.Collections;

[System.Serializable]

public class GesturePanelScript : MonoBehaviour
{
    
    public float currentCameraRotationOffset;
    public float CamControlSensitivity;
    private Vector3 CamMouseReference;
    private Vector3 CamMouseOffset;
    public float CamRotation;
    public bool isMoving;
    public GameObject currentCamera;
    private GameObject cameraTarget;
    //private Camera_track currentCameraData;

    public void touchCamDetected()
    {
        isMoving = true;
        CamMouseReference = Input.mousePosition;
        
        //Debug.Log("PointerDown");
    }
    public void touchCamFinished()
    {
        isMoving = false;
        currentCamera.GetComponent<CameraControlScript>().resetViewpoint();
        //currentCamera.GetComponent<CameraControlScript>().CamRotShiftTarget(0.0f);
        //currentCameraData.CamBackToPos();
    }

    void Start()
    {
        //CamControlSensitivity = 1000f;
        CamRotation = 0f;
        cameraTarget = currentCamera.GetComponent<CameraControlScript>().CameraTarget;
        //currentCameraData = currentCamera.GetComponent<Camera_track>();
        //Debug.Log(Screen.width);

    }
    
    // Update is called once per frame
    void Update()
    {
        float prevCamOrient = cameraTarget.GetComponent<CameraTargetScript>().offsetOrientation.y;
        //offset
        if (isMoving == true)
        {
            CamMouseOffset = (Input.mousePosition - CamMouseReference) / Screen.width; 
            //Debug.Log(CamMouseOffset.x);
        }
        else
        {
            CamMouseOffset = Vector3.zero;
        }
        
        CamRotation = CamMouseOffset.x * CamControlSensitivity;
        CamMouseReference = Input.mousePosition;
        if (isMoving == true)
        {
            currentCamera.GetComponent<CameraControlScript>().CamRotShiftTarget(prevCamOrient + CamRotation);
        }
        
    }

   
}
