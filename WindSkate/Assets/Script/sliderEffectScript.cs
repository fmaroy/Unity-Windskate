using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderEffectScript : MonoBehaviour {

	private Slider sliderObj;
	private GameObject sliderTrail;
	public float sliderWidth;
	public bool animated;
	// Use this for initialization
	void Start () {
		sliderObj = this.gameObject.GetComponent<Slider>();
		foreach (Transform child in this.gameObject.transform) {
			if (child.gameObject.name == "sliderTrail") {
				sliderTrail = child.gameObject;
				sliderTrail.SetActive (false);
			}
		}
		sliderWidth = this.gameObject.GetComponent<RectTransform>().rect.width;
		animated = false;
	}

	public void sliderDecreaseAnimation(float currentValue, float targetValue)
	{
		animated = true;
		if (currentValue > targetValue) {
			Debug.Log ("target not reached yet");
			sliderTrail.SetActive (true);
			//this.transform.localScale = new Vector3 (sliderWidth * currentValue / sliderObj.maxValue, this.gameObject.GetComponent<RectTransform>().rect.height);
			RectTransform rt = this.gameObject.GetComponent<RectTransform>();
			rt.sizeDelta = new Vector2 ( sliderWidth * currentValue / sliderObj.maxValue,this.gameObject.GetComponent<RectTransform>().rect.height);
			Debug.Log ("Right before slider decrease coroutine");
			StartCoroutine (animateDecreaseSlider(currentValue, targetValue, 2.0f));
			//animated = false;
			//sliderTrail.SetActive (false);
		}
	}

	IEnumerator animateDecreaseSlider(float val, float target, float duration)
	{
		Debug.Log("coroutine reduce slider");
		float initValue = val;
		float initDiffValues = target - initValue;
		float steps = initDiffValues / (duration * 25);
		while (val >= target) {
			val = val - steps;
			sliderObj.value = val;
			yield return new WaitForSeconds (1 / (duration * 25));
		}
		animated = false;
		sliderTrail.SetActive (false);
		Debug.Log ("target reached");
		yield return null;
			//WaitForSeconds (1 / (duration * 25));

	}

	// Update is called once per frame
	void Update () {
		
	}
}
