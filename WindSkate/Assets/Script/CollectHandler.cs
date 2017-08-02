using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectHandler : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Collectable")) {
			float currentEnergy = this.transform.parent.GetComponent<tricksHandlingScript> ().currentEnergyLevel;
			currentEnergy = currentEnergy + other.gameObject.GetComponent<CollectableHandler> ().energyBoost;
			this.transform.parent.GetComponent<tricksHandlingScript> ().currentEnergyLevel = currentEnergy;
			Destroy(other.gameObject, 0.0f);
		}
	}


	// Update is called once per frame
	void Update () {
		
	}
}
