using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerSpawn : MonoBehaviour {

	public GameObject SceneManagerObjectPrefab;
	public GameObject SceneManagerObject;

	public GameObject spawnSceneManager()
	{
		GameObject temp = null;

		if (GameObject.FindObjectOfType<PersistentParameters> () == null) {
			temp = (GameObject)Instantiate (SceneManagerObjectPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity);
			temp.name = "Scene_Manager";
		}
		return temp;
	}

	// Use this for initialization
	void Start () {
		
		//Debug.Log(GameObject.Find ("Scene_Manager").name);
		if (GameObject.Find ("Scene_Manager") != null) {
			SceneManagerObject = GameObject.Find ("Scene_Manager"); // there is already a scene object loaded inthe tree : finding that object and assigning it to the SceneManagerObject
		} else {
			SceneManagerObject = spawnSceneManager (); // not scene object loaded in the tree yet : spawning one
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
