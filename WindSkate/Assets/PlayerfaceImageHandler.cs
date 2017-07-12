using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerfaceImageHandler : MonoBehaviour {
	public RenderTexture rt;
	public GameObject cam;
	public GameObject camReferencePosition;
	public GameObject camReferenceOrientation;
	public Vector3 cameraOffet;
	public Vector3 cameraOrientation;

	// Use this for initialization
	void Start () {
		rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
		cam.GetComponent<Camera> ().targetTexture = rt;
		this.GetComponent<RawImage> ().texture = rt;
	}
	
	// Update is called once per frame
	void Update () {
		//cam.transform.position = camReferencePosition.transform.position + cameraOffet;
		//cam.transform.eulerAngles = camReferenceOrientation.transform.eulerAngles + cameraOrientation;
	}
}
