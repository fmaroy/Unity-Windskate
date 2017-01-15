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
    private GameObject RaceData;
    public GameObject Ranking;


    void Start()
    {
        RaceData = GameObject.Find("RaceManager");
        //Upate Game Settings
        updateGameSettings();
        Scenemanager = GameObject.Find("Scene_Manager");
        if (Scenemanager != null)
        {
            ScenemanagerData = Scenemanager.GetComponent<SceneManagerScript>();
        }
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

    /*public static void ChangeScene(string SceneID)
   {
       lastScene = currentScene;
       currentScene = SceneID;
       SceneManager.LoadScene(currentScene);
       //Application.LoadLevel(currentScene);
   }*/



    public void QuitApplication()
	{
		 Application.Quit();
	}
	public void SlowMotionFadeIn(float duration)
	{
		/*float SlowMotionTime = 0.0f;
		while (SlowMotionTime < duration)
		{
			Time.timeScale = 1 - (SlowMotionTime/duration);
			Debug.Log(Time.timeScale);
			SlowMotionTime=SlowMotionTime+Time.deltaTime;
		}*/
		Time.timeScale = 0.0f;
	}
	public void SlowMotionFadeOut(float duration)
	{
		/*float SlowMotionTime = 0.0f;
		while (SlowMotionTime < duration)
		{
			Time.timeScale = SlowMotionTime/duration;
			Debug.Log(Time.timeScale);
			SlowMotionTime=SlowMotionTime+Time.deltaTime;
		}*/
		Time.timeScale = 1.0f;
	}
	
	
	public void PauseGame()
	{
		PauseButton.SetActive(false);
		PauseImage.SetActive(true);
        Time.timeScale = 0.0f;
        /*fadingIn = true;
        currentSlowmotionTime = 0.0f;*/
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
