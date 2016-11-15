using UnityEngine;
using System.Collections;

public class Set_canevas_Postion : MonoBehaviour {
	public GameObject board;
	public Vector3 Canevas_offset;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	transform.position = board.transform.position + Canevas_offset;
	}
}
