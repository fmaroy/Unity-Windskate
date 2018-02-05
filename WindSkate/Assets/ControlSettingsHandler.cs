using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSettingsHandler : MonoBehaviour {

	public List<ControlSettings> ListOfControlSettings = new List<ControlSettings> ();

	public int currentlySelected = 0;

	public PersistentParameters param;

	public Slider tiltSensitivitySlider;
	public float tiltSensitivity = 0f;
		
	// Use this for initialization
	void Start () {
		GameObject raceManagerObject = GameObject.Find ("RaceManager");
		param = raceManagerObject.GetComponent<UserPreferenceScript> ().PersistentParameterData;
		currentlySelected = param.typeOfControl;
		tiltSensitivity = param.tiltSensitivity;
		int i = 0;
		foreach (ControlSettings setting in ListOfControlSettings) 
		{
			if (i == currentlySelected) 
			{
				setting.Button.GetComponent<Button> ().Select ();
                Debug.Log("Selected Button : " + setting.Button.name);
			}
            i++;
		}
	}

	/// <summary>
	/// displays the correct frame based on the selected button.
	/// </summary>
	/// <param name="i">The index.</param>
	public void selectedButton(int i)
	{
		int j = 0;
		foreach (ControlSettings control in ListOfControlSettings) {
			if (j == i) {
				control.Frame.SetActive (true);
				currentlySelected = i;
			} 
			else {
				control.Frame.SetActive (false);
			}
			j++;
		}
		param.typeOfControl = currentlySelected;
		param.tiltSensitivity = tiltSensitivity;
        PlayerPrefs.SetInt("TypeOfControls", param.typeOfControl);
        Debug.Log("PlayerPrefs control : " + param.typeOfControl);
	}

	public void onTiltSensitivitySliderChange()
	{
		tiltSensitivity = tiltSensitivitySlider.value;
	}

	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class ControlSettings
{
	public string name;
	public GameObject Button;
	public GameObject Frame;

	public ControlSettings(string n, GameObject b, GameObject f)
	{
		name = n;
		Button = b;
		Frame = f;
	}
}
