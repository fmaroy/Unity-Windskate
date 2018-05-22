using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    static string lastScene;
    static string currentScene;
	public string MainMenuName;
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

	public void LoadMainScene()
	{
		lastScene = currentScene;
		currentScene = MainMenuName;
		SceneManager.LoadScene(MainMenuName);
		Time.timeScale = 1.0f;
	}

    public void LoadScene(string level)
    {
        lastScene = currentScene;
        currentScene = level;

        //Added to try to optmize the perfo of the newely loaded scene
        Resources.UnloadUnusedAssets();

        Time.timeScale = 1.0f;

        SceneManager.LoadScene(level);
        Debug.Log("unloading the previous scene");
        SceneManager.UnloadSceneAsync(lastScene);

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
        SceneManager.UnloadSceneAsync(level);
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
