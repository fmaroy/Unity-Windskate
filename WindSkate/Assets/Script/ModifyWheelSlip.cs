using UnityEngine;
using System.Collections;

public class ModifyWheelSlip : MonoBehaviour {
    public GameObject LeftWheel;
    public GameObject RightWheel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //var surfaceIndex = TerrainSurface.GetMainTexture(transform.position);
        var surfaceMix = TerrainSurface.GetTextureMix(transform.position);
        Debug.Log(surfaceMix);
        var sandiness = surfaceMix[1];
        Debug.Log(sandiness);

    }
}
