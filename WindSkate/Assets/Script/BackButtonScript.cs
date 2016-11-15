using UnityEngine;
using System.Collections;

public class BackButtonScript : MonoBehaviour {

    private GameObject Scenemanager;
    private SceneManagerScript ScenemanagerData;
    // Use this for initialization
    void Start () {
        Scenemanager = GameObject.Find("Scene_Manager");
        ScenemanagerData = Scenemanager.GetComponent<SceneManagerScript>();

    }

	public void ExitSettings()
    {
        Scenemanager= GameObject.Find("Scene_Manager");
        ScenemanagerData.UnloadScene("Settings_page");
        ScenemanagerData.displayMainMenu();
    }

    public void ExitStore()
    {
        Scenemanager = GameObject.Find("Scene_Manager");
        ScenemanagerData.UnloadScene("Shop_page");
        ScenemanagerData.displayMainMenu();
    }
    // Update is called once per frame
    void Update () {
	
	}
}
