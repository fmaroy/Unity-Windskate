using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    static string lastScene;
    static string currentScene;
    private GameObject MainMenu;

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }
    public void LoadLastScene()
    {
        string last = lastScene;
        lastScene = currentScene;
        currentScene = last;
        SceneManager.LoadScene(currentScene);
        //Application.LoadLevel(currentScene);
        
    }
    public void displayMainMenu()
    {
        MainMenu = GameObject.Find("Main Menu");
        MainMenu.SetActive(true);
    }

    public void LoadScene(string level)
    {
        lastScene = currentScene;
        currentScene = level;

        SceneManager.LoadScene(level);

        Time.timeScale = 1.0f;
    }
    public void LoadSceneAdditive(string level)
    {
        lastScene = currentScene;
        currentScene = level;

        SceneManager.LoadScene(level, LoadSceneMode.Additive);
        //Time.timeScale = 1.0f;
    }
    public void UnloadScene(string level)
    {
        SceneManager.UnloadScene(level);
        //Time.timeScale = 1.0f;
    }
	public void exitGame()
	{
		Application.Quit ();
	}

    // Use this for initialization
    void Start ()
    {
        currentScene = SceneManager.GetActiveScene().name;
        //MainMenu = GameObject.Find("Main Menu");
        //MainMenu.SetActive(true);
        //GameObject RaceMenu = GameObject.Find("Race Selection");
        //RaceMenu.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
