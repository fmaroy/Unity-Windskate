using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerSpawn : MonoBehaviour {

	public GameObject SceneManagerObject;

	public void spawnSceneManager()
	{
		if (GameObject.FindObjectOfType<PersistentParameters> () == null) {
			GameObject temp = (GameObject)Instantiate (SceneManagerObject, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			temp.name = "Scene_Manager";
		}
	}

	// Use this for initialization
	void Start () {
		spawnSceneManager ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
