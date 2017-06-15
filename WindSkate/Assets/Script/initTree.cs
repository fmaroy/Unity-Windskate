using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initTree : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Transform child in this.transform) {
			if (child.gameObject.CompareTag ("WindEffects") == true) {
				child.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
