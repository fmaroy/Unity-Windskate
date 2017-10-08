using UnityEngine;
using System.Collections;

[System.Serializable]

public class GesturePanelScript : MonoBehaviour
{
    
    public float currentCameraRotationOffset;
    public float CamControlSensitivityX;
	public float CamControlSensitivityY;

    private Vector3 CamMouseReference;
    private Vector3 CamMouseOffset;

    public float CamRotationX;
	public float CamRotationY;
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
		currentCamera.GetComponent<CinemachineControls>().disableOrbitCamera();
        //currentCamera.GetComponent<CameraControlScript>().CamRotShiftTarget(0.0f);
        //currentCameraData.CamBackToPos();
    }

    void Start()
    {
        //CamControlSensitivity = 1000f;
		currentCamera = Camera.main.gameObject;

        CamRotationX = 0f;
		CamRotationY = 0f;
        //cameraTarget = currentCamera.GetComponent<CameraControlScript>().CameraTarget;
        //currentCameraData = currentCamera.GetComponent<Camera_track>();
        //Debug.Log(Screen.width);

    }
    
    // Update is called once per frame
    void Update()
    {
        //float prevCamOrient = cameraTarget.GetComponent<CameraTargetScript>().offsetOrientation.y;

		float prevCamOrientX = currentCamera.GetComponent<CinemachineControls> ().OrbitCameraData.m_XAxis.Value;
		float prevCamOrientY = currentCamera.GetComponent<CinemachineControls> ().OrbitCameraData.m_YAxis.Value;

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
        
        CamRotationX = CamMouseOffset.x * CamControlSensitivityX;
		CamRotationY = -1* CamMouseOffset.y * CamControlSensitivityY;
        CamMouseReference = Input.mousePosition;
        if (isMoving == true)
        {
			currentCamera.GetComponent<CinemachineControls>().orbitCameraHandler(prevCamOrientX + CamRotationX, prevCamOrientY + CamRotationY);
        }
        
    }

   
}
