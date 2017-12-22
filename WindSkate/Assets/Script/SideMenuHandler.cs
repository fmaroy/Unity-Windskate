using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SideMenuHandler : MonoBehaviour {

    private Animator BackgroundExpansionAnimation;
    public GameObject expandingBackground;
    public int menuExpandedStatus;
    public List<Menu> MenuList = new List<Menu>();
    //public List<UITabs> TabsList = new List<UITabs>();
    public GameObject BackButton;
    public GameObject ActionButtons;
    private Animator ActionButtonsAnim;
    public int currentOpenedMenu = 0;
    private int buttonIdleState;
    private int buttonIdlePressed;
    private int buttonIdleHighlight;
    private int buttonIdleCollapse;
    private int buttonIdlePressedCollapse;
	public GameObject sideMenuBackgroundObj;

    private AnimatorStateInfo currentBaseStateButton;
	public Animation showBackButtonAnimation;
    
    // Use this for initialization
    void Start()
    {
        BackgroundExpansionAnimation = expandingBackground.GetComponent<Animator>();
        BackgroundExpansionAnimation.SetInteger("OpenPannelStatus", 0);

		/*foreach (Menu localMenu in MenuList)
        {
            localMenu.menuPanel.GetComponent<Animator>().SetInteger("OpenPannelStatus", 0);
            localMenu.openedTab = 0;
            int i = 0;
            foreach (UITabs tab in localMenu.TabsList)
            {
                if (i == tab.tabId)
                {
                    tab.Panel.SetActive(true);
                }
                else
                {
                    tab.Panel.SetActive(false);
                }
            }
        }

        ActionButtonsAnim = ActionButtons.GetComponent<Animator>();
        ActionButtonsAnim.SetInteger("ShowBackButton", 0);

        buttonIdleState = Animator.StringToHash("Base_Layer.Side_Button_Normal");
        buttonIdlePressed = Animator.StringToHash("Base_Layer.SideButtonPressed");
        buttonIdleHighlight = Animator.StringToHash("Base_Layer.SideButtonHighlight");
        buttonIdleCollapse = Animator.StringToHash("Base_Layer.SideButtonCollapse");
        buttonIdlePressedCollapse = Animator.StringToHash("Base_Layer.SideButtonCollapseSelected");

        currentBaseStateButton = ActionButtonsAnim.GetCurrentAnimatorStateInfo(0);
		*/
        //BackbuttonClick();

		ActionButtonsAnim = ActionButtons.GetComponent<Animator>();

		initMenu();
    }

	public void initMenu()
	{
		foreach (Menu currentMenu in MenuList) {
			currentMenu.menuPanel.SetActive (false); 
			if (currentMenu.landingPage != null) {
				currentMenu.landingPage.SetActive (false); 
			}

			foreach (UITabs tab in currentMenu.TabsList) {
				tab.Panel.SetActive (false);

			}
			currentOpenedMenu = -1;
			sideMenuDisplayHandler (true);
			ActionButtonsAnim.SetInteger("ShowBackButton", 0);
		}
	}

	/// <summary>
	/// deprecated
	/// </summary>
	/// <param name="i">The index.</param>
    /*public void TabButton(int i)
    {
        foreach (UITabs tab in MenuList[currentOpenedMenu].TabsList)
        {
            if (i == tab.tabId)
            {
                tab.Panel.SetActive(true);
                tab.Button.GetComponent<Animator>().SetTrigger("Pressed");
            }
            else
            {
                tab.Panel.SetActive(false);
                tab.Button.GetComponent<Animator>().SetTrigger("Normal");
            }
        }
    }*/

	public void backButtonHandler()
	{
		// the following helps decide if a landing page is opened or tab (tab is a sub menu of a lanfing page)
		if (currentOpenedMenu != -1) {
			if (MenuList [currentOpenedMenu].landingPage != null) {
				if (MenuList [currentOpenedMenu].landingPage.activeSelf == true) {
					// the landing page is opened
					MenuList [currentOpenedMenu].landingPage.SetActive (false);
					MenuList [currentOpenedMenu].menuPanel.SetActive (false);

					sideMenuDisplayHandler (true);

				} else {
					// a tab menu is currently opened
					Menu currentMenu = MenuList [currentOpenedMenu];
					currentMenu.landingPage.SetActive (true);
					currentMenu.TabsList [currentMenu.openedTab].Panel.SetActive (false);
				}
			}
		}
	}

	/// <summary>
	/// Display management of the side menu. hides the landing pages and the tabs
	/// </summary>
	/// <param name="display">If set to <c>true</c> display.</param>
	public void sideMenuDisplayHandler(bool display)
	{
		Debug.Log ("sidemenuDisplayhandler = " + display);
		foreach (Menu currentMenu in MenuList) {
			foreach (Transform child in currentMenu.buttonObject.transform) {
				if (child.gameObject.name == "ButtonBackground") {
					child.gameObject.SetActive (display);
				}
			}
		}
		sideMenuBackgroundObj.SetActive (display);
		if (display) {
			ActionButtonsAnim.SetInteger ("ShowBackButton", 0);
		} else {
			ActionButtonsAnim.SetInteger ("ShowBackButton", 1);
		}

		//if (display == true) {
		//	openLandingPage (-1);
		//}
	}

	/// <summary>
	/// Displays only the landing page i defined in the MenuList
	/// Hides all the tabs of the menu i
	/// </summary>
	/// <param name="i">The index in the MenuList</param>
	public void openLandingPage(int i)
	{
		Debug.Log ("Open Landing Page ID : " + i);
		int counter = 0;
		foreach (Menu currentMenu in MenuList) {
			if (i == counter) {
				
				currentMenu.menuPanel.SetActive (true);
				currentMenu.landingPage.SetActive (true);

				Debug.Log ("Landing Page Name : " + currentMenu.landingPage.name);

				foreach (UITabs tab in currentMenu.TabsList) {
					tab.Panel.SetActive (false);
				}
			}
			else {
				currentMenu.menuPanel.SetActive (false);
			}
			counter = counter + 1;
		}
		currentOpenedMenu = i;
		sideMenuDisplayHandler (false);

		Debug.Log (MenuList[currentOpenedMenu].TabsList.Count);
		if (MenuList[currentOpenedMenu].TabsList.Count == 1) {
			// since only one tab is available we skip the landing page
			openMenuTab (0);
		}
	}

	/// <summary>
	/// Displays the tab i of the current menu
	/// </summary>
	/// <param name="i">The index.</param>
	public void openMenuTab (int i)
	{
		int counter = 0;
		Menu currentMenu = MenuList [currentOpenedMenu];
		foreach (UITabs tab in currentMenu.TabsList) {
			if (i == counter) {
				tab.Panel.SetActive (true); 
				Debug.Log ("Tab Page Name : " + tab.Panel.name);
			}
			else {
				tab.Panel.SetActive (false);
			}
			counter = counter + 1;
			// hide landing page
			if (currentMenu.landingPage != null) {
				currentMenu.landingPage.SetActive (false);
			}
			currentMenu.openedTab = i;
		}
	}

	/// <summary>
	/// deprecated
	/// </summary>
	/// <param name="i">The index.</param>
    /*public void ExpandMenu(int i)
    {
        Menu selectedMenu = MenuList[i];
        Debug.Log(selectedMenu.menuName);
        //Animator currentMenuAnim = selectedMenu.menuPanel.GetComponent<Animator>();
        //currentMenuAnim.SetTrigger("Pressed");
        BackgroundExpansionAnimation.SetInteger("OpenPannelStatus", selectedMenu.menuOpenType);
        // reseting the other buttons
        foreach (Menu localMenu in MenuList)
        {
            
            Animator localMenuAnim = localMenu.buttonObject.GetComponent<Animator>();
            if (localMenu != selectedMenu)
            {
                localMenuAnim.SetTrigger("Normal");
                //localMenu.menuPanel.SetActive(true);
                localMenu.menuPanel.GetComponent<Animator>().SetInteger("OpenPannelStatus", 0);
                if (currentBaseStateButton.fullPathHash == buttonIdleState)
                {
                    Debug.Log("toto");
                }
            }
            else
            {
                localMenuAnim.SetTrigger("Pressed");
                localMenu.menuPanel.GetComponent<Animator>().SetInteger("OpenPannelStatus", selectedMenu.menuOpenType);
                TabButton(selectedMenu.openedTab);
            }
            localMenuAnim.SetInteger("SideMenuButtonCollapse",1);
            
        }
        menuExpandedStatus = selectedMenu.menuOpenType;
        ActionButtonsAnim.SetInteger("ShowBackButton",1);

        currentOpenedMenu = i;
        //TabButton(selectedMenu.openedTab);
    }*/

	/// <summary>
	/// deprecated
	/// </summary>
    /*public void BackbuttonClick()
    {
        foreach (Menu currentMenu in MenuList)
        {
            Animator currentMenuAnim = currentMenu.buttonObject.GetComponent<Animator>();
            currentMenuAnim.SetInteger("SideMenuButtonCollapse", 0);
            currentMenuAnim.SetTrigger("Normal");
            currentMenu.menuPanel.GetComponent<Animator>().SetInteger("OpenPannelStatus", 0);
            foreach (UITabs tab in currentMenu.TabsList)
            {
                    tab.Panel.SetActive(false);
                    tab.Button.GetComponent<Animator>().SetTrigger("Normal");
              
            }
        }
        ActionButtonsAnim.SetInteger("ShowBackButton", 0);
        BackgroundExpansionAnimation.SetInteger("OpenPannelStatus", 0);
        if (currentOpenedMenu == 1)
        {

        }
    }*/
}

[System.Serializable]
public class Menu
{
    public string menuName;
    public int menuOpenType;
    public GameObject buttonObject;
    public GameObject menuPanel;
	public GameObject landingPage;
    public int openedTab;
    public List<UITabs> TabsList;

    Menu(string n, int opentype, GameObject b,  GameObject g, int tab, List<UITabs> list)
    {
        menuName = n;
        menuOpenType = opentype;
        buttonObject = b;
        menuPanel = g;
        openedTab = tab;
        TabsList = list;
    }
}

[System.Serializable]
public class UITabs
{
    public string Name;
    public GameObject Button;
    public GameObject Panel;
    public int tabId;

    UITabs(string n, GameObject b, GameObject g, int i)
    {
        Name = n;
        Button = b;
        Panel = g;
        tabId = i;
    }
}
