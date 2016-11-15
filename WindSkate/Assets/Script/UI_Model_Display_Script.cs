using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Model_Display_Script : MonoBehaviour {

    public List<UI3DModelDisplay> modelDisplay = new List<UI3DModelDisplay>(); 

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
[System.Serializable]
public class UI3DModelDisplay
{
    public int id;
    public GameObject Light1;
    public GameObject Light2;
    public GameObject Model;
    public GameObject CameraUI;
    

    public UI3DModelDisplay(int i, GameObject l1, GameObject l2, GameObject m, GameObject c)
    {
        int id = i;
        GameObject Lights1 = l1;
        GameObject Lights2 = l2;
        GameObject Model = m;
        GameObject CameraUI = c;
    }
}
