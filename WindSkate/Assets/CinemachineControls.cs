using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineControls : MonoBehaviour {
	public bool isIntroScene;
	public int cameraId = 0;
	public float introCameraTimer = 5;
	private float timer = 0;
	public GameObject playerPosTarget;
	public GameObject playerOrientTarget;
	public GameObject playerPosObj;
	public GameObject playerOrientObj;

	public GameObject camFollowTraceObj;
	public GameObject traceCam;
	public List<GameObject> camFollowPlayerList = new List<GameObject> ();
	public List<GameObject> CrashCameras = new List<GameObject>();
	public GameObject OrbitCamera;
	public CinemachineFreeLook OrbitCameraData;

	public void initCamera()
	{
		OrbitCameraData = OrbitCamera.GetComponent<CinemachineFreeLook> ();
		setPlayerCamera (cameraId);
	}

	public void setPlayerCamera(int id)
	{
		for (int i = 0; i < camFollowPlayerList.Count; i++) {
			
			if (i == id) {
				camFollowPlayerList [i].SetActive (true);
			} else {
				camFollowPlayerList [i].SetActive (false);
			}
		}
	}

	public void setTraceCamera(bool vis)
	{
		traceCam.SetActive (vis);
	}

	public void setNextCamera()
	{
		cameraId = cameraId + 1;
		if (cameraId >= camFollowPlayerList.Count) 
		{
			cameraId = 0;
		}
		setPlayerCamera (cameraId);
	}

	public void disableOrbitCamera()
	{
		OrbitCamera.SetActive (false);
	}

	public void orbitCameraHandler(float dx, float dy)
	{
		OrbitCamera.SetActive (true);
		OrbitCameraData.m_XAxis.Value = dx;
		OrbitCameraData.m_YAxis.Value = dy;
	}

	public void CrashCameraHandler(bool isCrashing, int camId)
	{
		for (int i = 0; i < CrashCameras.Count; i++) 
		{
			if (isCrashing) {
				if (i == camId) {
					CrashCameras [i].SetActive (true);
				} else {
					CrashCameras [i].SetActive (false);
				}
			} else {
				CrashCameras [i].SetActive (false);
			}
		}
	}


	// Use this for initialization
	void Start () {
		timer = 0;
	}

	void Update(){
		if (isIntroScene) {
			timer = timer + Time.deltaTime;
			if (timer > introCameraTimer) {
				timer = 0;
				setNextCamera ();
			}
		}
	}

	void FixedUpdate () {
		playerPosObj.transform.position = playerPosTarget.transform.position;
		playerOrientObj.transform.position = playerOrientTarget.transform.position;
		playerOrientObj.transform.eulerAngles = new Vector3(0f, playerOrientTarget.transform.eulerAngles.y, 0f);

	}
}
