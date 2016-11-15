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

    private AnimatorStateInfo currentBaseStateButton;
    
    // Use this for initialization
    void Start()
    {
        BackgroundExpansionAnimation = expandingBackground.GetComponent<Animator>();
        BackgroundExpansionAnimation.SetInteger("OpenPannelStatus", 0);
        foreach (Menu localMenu in MenuList)
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

        BackbuttonClick();
    }

    public void TabButton(int i)
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
        
    }

    public void ExpandMenu(int i)
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
    }


    public void BackbuttonClick()
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
    }
	// Update is called once per frame
	void Update () {
	
	}
}
[System.Serializable]
public class Menu
{
    public string menuName;
    public int menuOpenType;
    public GameObject buttonObject;
    public GameObject menuPanel;
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
