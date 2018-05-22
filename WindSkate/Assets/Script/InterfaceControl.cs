﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class InterfaceControl : MonoBehaviour {

	public int UIComplexityLevel = 0;
    private GameObject Scenemanager;
    private SceneManagerScript ScenemanagerData;
    public GameObject LoadingImage;
    public GameObject PauseImage;
    public GameObject PauseButton;
	public GameObject CameraButton;
	public GameObject FinishImage;
    private GameObject RaceData;
    public GameObject Ranking;
	public GameObject Player;
	public GameObject PlayerBoard;
	public GameObject SliderContainer;
	public GameObject WindIndicator;
	public GameObject TurnLeftButton;
	public GameObject TurnRightButton;
	public GameObject ManoeuvreLeftButton;
	public GameObject ManoeuvreRightButton;
	public GameObject MessageText;
	public GameObject MetricsDisplay;
	public GameObject IntroImage;
	public GameObject GesturePanel;
	public GameObject StartLights;
	public GameObject SkipIntoButton;

	//private AnimatorStateInfo reachedEndAnimationState;


	public void initControls (GameObject player)
	{
		Player = player;
		Start ();

	}

	public void typeOfControlsHandler(int i)
	{
		if (i == 0) {
			// control is by buttons
			TurnLeftButton.SetActive(true);
			TurnRightButton.SetActive(true);
		}
		if (i == 1) {
			// control is by tilt
			TurnLeftButton.SetActive(false);
			TurnRightButton.SetActive(false);
		}
	}

	public void controlsVisibiltyHandler (bool vis)
	{
		int level = UIComplexityLevel;
		GameObject[] PlayerControls = {
			TurnLeftButton, TurnRightButton,ManoeuvreLeftButton, ManoeuvreRightButton
		};
		GameObject[] GameControls = {
			PauseButton, CameraButton, GesturePanel
		};
		GameObject[] PlayerInfo = {
			SliderContainer, WindIndicator
		};
		GameObject[] GameInfo = {
			Ranking, MessageText, MetricsDisplay
		};

		if (!vis) {
			foreach (GameObject obj in PlayerControls)
			{
				obj.SetActive (false);
			}
			foreach (GameObject obj in GameControls)
			{
				obj.SetActive (false);
			}
			foreach (GameObject obj in PlayerInfo)
			{
				obj.SetActive (false);
			}
			foreach (GameObject obj in GameInfo)
			{
				obj.SetActive (false);
			}
		} 
		else {
			int i = 0;
			foreach (GameObject obj in PlayerInfo)
			{
				if (i < level) {
					obj.SetActive (true);

				} 
				else {
					obj.SetActive (false);
				}
				i++;
			}
			foreach (GameObject obj in PlayerControls)
			{
				obj.SetActive (true);
			}
			foreach (GameObject obj in GameControls)
			{
				obj.SetActive (true);
			}

			foreach (GameObject obj in GameInfo)
			{
				obj.SetActive (true);
			}
			typeOfControlsHandler (RaceData.GetComponent<UserPreferenceScript>().typeOfControls);
		}
	}

    void Start()
    {
        RaceData = GameObject.Find("RaceManager");
		PlayerBoard = RaceData.GetComponent<UserPreferenceScript>().PlayerBoard;
        //Upate Game Settings
        //updateGameSettings();
        Scenemanager = GameObject.Find("Scene_Manager");
        if (Scenemanager != null)
        {
            ScenemanagerData = Scenemanager.GetComponent<SceneManagerScript>();

        }
		SkipIntoButton.SetActive (false);

    }

	public void triggerManoeuvreListener ()
	{
		PlayerBoard.GetComponent<Follow_track> ().triggeredManoeuvre ();
	}

	public void leftTurnButtonDownListener ()
	{
		PlayerBoard.GetComponent<BoardForces> ().LeftTurnButtonDown ();
	}
	public void rightTurnButtonDownListener ()
	{
		PlayerBoard.GetComponent<BoardForces> ().RightTurnButtonDown ();
	}

	public void leftTurnButtonUpListener ()
	{
		PlayerBoard.GetComponent<BoardForces> ().LeftTurnButtonUp ();
	}
	public void rightTurnButtonUpListener ()
	{
		PlayerBoard.GetComponent<BoardForces> ().RightTurnButtonUp ();
	}

    public void updateRanking()
    {
        string rankingText = "Position: " + (RaceData.GetComponent<PlayersTrackOnRacetrack>().rankingList[0] + 1).ToString() + "/" + RaceData.GetComponent<PlayersTrackOnRacetrack>().rankingList.Count.ToString();
        Ranking.GetComponentInChildren<Text>().text = rankingText;
    }

    public void updateGameSettings()
    {
        
        RaceData.GetComponent<UserPreferenceScript>().updateSettings();
    }

    public void LoadMainMenu()
    {
        Scenemanager = GameObject.Find("Scene_Manager");
        ScenemanagerData = Scenemanager.GetComponent<SceneManagerScript>();
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.01f * Time.timeScale;
        ScenemanagerData.UnloadScene(SceneManager.GetActiveScene().name);
		Scenemanager.GetComponent<SceneManagerScript>().LoadMainScene();

        //
    }

    public void LoadSettings()
    {
        Scenemanager = GameObject.Find("Scene_Manager");
        Scenemanager.GetComponent<SceneManagerScript>().LoadSceneAdditive("Settings_page");
    }
    public void ReloadLevel()
    {
        Scenemanager = GameObject.Find("Scene_Manager");
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.01f * Time.timeScale;
        ScenemanagerData.UnloadScene(SceneManager.GetActiveScene().name);
        Scenemanager.GetComponent<SceneManagerScript>().LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void LoadScene(string Scene)
    {
        Scenemanager = GameObject.Find("Scene_Manager");
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.01f * Time.timeScale;
        ScenemanagerData.UnloadScene(SceneManager.GetActiveScene().name);
        Scenemanager.GetComponent<SceneManagerScript>().LoadScene(Scene);

    }

    public void QuitApplication()
	{
		 Application.Quit();
	}
	public void SlowMotionFadeIn(float duration)
	{
		Time.timeScale = 0.0f;
	}
	public void SlowMotionFadeOut(float duration)
	{
		Time.timeScale = 1.0f;
	}
	
	
	public void PauseGame()
	{
		PauseButton.SetActive(false);
		PauseImage.SetActive(true);
        Time.timeScale = 0.0f;
		Time.fixedDeltaTime = 0.01f * Time.timeScale;
    }
    public void ResumeGame()
	{
		PauseButton.SetActive(true);
		PauseImage.SetActive(false);
        Time.timeScale = 1.0f;
		Time.fixedDeltaTime = 0.01f * Time.timeScale;
    }

    void Update()
    {
        updateRanking();
    }
}
