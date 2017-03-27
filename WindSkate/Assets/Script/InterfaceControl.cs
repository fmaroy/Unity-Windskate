using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceControl : MonoBehaviour {

    private GameObject Scenemanager;
    private SceneManagerScript ScenemanagerData;
    public GameObject LoadingImage;
    public GameObject PauseImage;
    public GameObject PauseButton;
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


	public void initControls (GameObject player)
	{
		Player = player;
		Start ();
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

        Scenemanager.GetComponent<SceneManagerScript>().LoadScene("Main_menu");

        ScenemanagerData.UnloadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSettings()
    {
        Scenemanager = GameObject.Find("Scene_Manager");
        Scenemanager.GetComponent<SceneManagerScript>().LoadSceneAdditive("Settings_page");
    }
    public void ReloadLevel()
    {
        Scenemanager = GameObject.Find("Scene_Manager");
        Scenemanager.GetComponent<SceneManagerScript>().LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void LoadScene(string Scene)
    {
        Scenemanager = GameObject.Find("Scene_Manager");
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
    }
    public void ResumeGame()
	{
		PauseButton.SetActive(true);
		PauseImage.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        updateRanking();
    }
}
