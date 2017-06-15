using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager : MonoBehaviour {

	public GameObject EnergyCanPrefab;
	public GameObject collectablesContainerObj;
	public List<GameObject> relocalizationPositionObjects = new List<GameObject>();
	public List<GameObject> relocalizationPositions = new List<GameObject>();
	public List<GameObject> alreadyUsedPositionsObj = new List<GameObject>();
	public float timeIntervals = 2f;
	public float time;
	public float randomFactor = 0.5f;

	// Use this for initialization
	void Start () {
		time = 0f;
		foreach (GameObject relocObjContainers in relocalizationPositionObjects) {
			foreach (Transform child in relocObjContainers.transform) {
				relocalizationPositions.Add (child.gameObject);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		time = time + Time.deltaTime;
		if (time >= timeIntervals) {
			time = 0f;
			if (Random.value > randomFactor) {
				int id = Random.Range(0, relocalizationPositions.Count);
				//Debug.Log (id);
				GameObject temp = (GameObject)Instantiate(EnergyCanPrefab,relocalizationPositions[id].transform.position,relocalizationPositions[id].transform.rotation);
				temp.transform.parent = collectablesContainerObj.transform;
			}
		}
	}
}
