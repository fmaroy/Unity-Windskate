using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class messageHandler : MonoBehaviour {

	public IEnumerator currentMessageCoroutine;
	static int reachedEndAnimationState;
	public List<WarningMessage> MessageList = new List<WarningMessage>();
	public int currentMessage;
	private Animator textAnim;

	// Use this for initialization
	void Start () {
		textAnim = this.GetComponent<Animator> ();
		currentMessage = -1;
	}

	public void allowMessage(int messageId)
	{
		MessageList [messageId].alreadyDisplayed = false;
	}

	public void interruptMessage(int id)
	{
		StopCoroutine (currentMessageCoroutine);
		//MessageList [id].alreadyDisplayed = false;
		if (MessageList[id].blockAfter == false) // when message is set as blockAfter true, the message needs to be rest with function allowMessage(messageId) to play again
		{
			MessageList [id].alreadyDisplayed = false; //registers the fact that this message has been already displayed
		}
		currentMessageCoroutine = null; //allows new message to be thrown by cleaning up coroutine
		currentMessage = -1;

		//textAnim.SetTrigger ("Interrupt");
	}

	public void throwMessage(int messageId)
	{
		if (MessageList [messageId].alreadyDisplayed == true){
			return;
		}
		if (currentMessageCoroutine != null) 
		{
			if (MessageList[messageId].interrupting == true)
			{
				interruptMessage (currentMessage);
			}
			else
			{
			return;
			}
		}

		//Debug.Log ("display message");
		MessageList [messageId].alreadyDisplayed = true;
		currentMessage = messageId;
		string textMessage = MessageList [messageId].displayTextList[Random.Range(0, MessageList [messageId].displayTextList.Capacity)]; 
		currentMessageCoroutine = messageAnimator (messageId ,textMessage, MessageList[messageId].warningLevel); //prepare coroutine
		StartCoroutine (currentMessageCoroutine); // run the coroutine to run animation and wait it is over

	}

	IEnumerator messageAnimator(int id, string textMessage, int WarningLevel)
	{
		this.GetComponent<TextMeshProUGUI> ().SetText(textMessage);
		textAnim.SetTrigger ("Enable");
		//Debug.Log ("triggered animation");
		yield return new WaitForSeconds(1.5f);

		if (MessageList[id].blockAfter == false) // when message is set as blockAfter true, the message needs to be rest with function allowMessage(messageId) to play again
		{
			MessageList [id].alreadyDisplayed = false; //registers the fact that this message has been already displayed
		}
		currentMessageCoroutine = null; //allows new message to be thrown by cleaning up coroutine
		currentMessage = -1;
	}

	// Update is called once per frame
	void Update () {
		
	}

}
[System.Serializable]
public class WarningMessage
{
	public string name;
	public int warningLevel;
	public bool alreadyDisplayed;
	public bool interrupting;
	public bool blockAfter;
	public List<string> displayTextList;
	public WarningMessage(string n, bool b, int i)
	{
		name = n;
		alreadyDisplayed = b;
		warningLevel = i;
	}
}

