//Written by Erisrine! Enjoy the Spaghetti!
//Written by Erisrine! Enjoy the Spaghetti!
//Written by Erisrine! Enjoy the Spaghetti!
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
	[SerializeField] private TMP_Text dialogueText;
	[SerializeField] private float textSpeed = 0.05f;
	[SerializeField] private TMP_Text charName;
	[SerializeField] private AK.Wwise.Event nonSpeech;
	//[SerializeField] private TMP_Sprite charSprite;
	//private bool readyForNextText = true;
	private bool dialogueReveal;
	private bool skipDialogue;
	private bool canRunDialogue;
	public static bool isInDialogueRange;
	private bool hasFinishedList;
	public static bool playerControl = true;
	private List<string> dialogueList;
	private List<string> firstDialogue;
	private List<string> secondDialogue;
	private int currentDialogueIndex = 4;
	[SerializeField] private GameObject dialogueCanvas;
	
	public static DialogueSystem Instance {get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else 
		{
			Instance = this;
		}
	}
		
	public void GetList(List<string> firstDialogueIn, List<string> secondDialogueIn)
	{
		firstDialogue = firstDialogueIn;
		secondDialogue = secondDialogueIn;
		dialogueList = firstDialogue;
		//hasFinishedList = false;
		currentDialogueIndex = 0;
		dialogueReveal = true;
		canRunDialogue = true;
		//SetDialogueList(1);
		ShowDialogue(dialogueList[currentDialogueIndex]);
	
	}
	
	public void SetDialogueList(int n)
	{
		if (n == 1){ dialogueList = firstDialogue; }
		else if(n == 2){ dialogueList = secondDialogue; }
	}
	
	
	public void ShowDialogue(string dialogue)
	{
		InterpretText(dialogue);
	}
	
	private void DialogueIndexHandler()
	{
		Debug.Log("Line Done");
		dialogueReveal = true;
		
		if (currentDialogueIndex < dialogueList.Count)
		{
			currentDialogueIndex += 1;
			Debug.Log("Number was less than index count. +1");
			hasFinishedList = false;
		}
		if(currentDialogueIndex == dialogueList.Count)
		{
			Debug.Log("Number was equal than index count. Has reached the end of list");
			hasFinishedList = true;
			SetDialogueList(2);
			currentDialogueIndex = 0;
		}
	}
	
	private void ResetDialogueIndex()
	{
		currentDialogueIndex = 0;
	}
	
	private void Update()
	{
		if(isInDialogueRange)
		{
			if(canRunDialogue)
			{	
				dialogueCanvas.SetActive(true);
				//Dialogue Skipper
				if (Input.GetKeyDown(KeyCode.A) && !dialogueReveal)
				{
					skipDialogue = true;
				}
				
				//Dialogue Handling
				string nextDialogue = "";
				
				if(dialogueList != null)
				{
				nextDialogue = dialogueList[currentDialogueIndex];
				}

				if(dialogueReveal)
				{
					if (Input.GetKeyDown(KeyCode.Space) && !hasFinishedList)
					{
						//ShowDialogue();
						Debug.LogWarning("NextDialogue!");
						ShowDialogue(nextDialogue);
					}
					if(Input.GetKeyDown(KeyCode.Space) && hasFinishedList)
					{
						canRunDialogue = false;
						ResetDialogueIndex();
					}
				}
			}
			else if(!canRunDialogue)
			{
				dialogueCanvas.SetActive(false);
				if(Input.GetKeyDown(KeyCode.Space))
				{
					canRunDialogue = true;
					ShowDialogue(dialogueList[currentDialogueIndex]);
					hasFinishedList = false;
					dialogueCanvas.SetActive(true);
				}
			}
		}
		else
		{
			dialogueCanvas.SetActive(false);
		}
	}

	private void InterpretText(string dialogue)
	{
		// Find the position of the opening and closing brackets
		int openingBracketIndex = dialogue.IndexOf('[');
		int closingBracketIndex = dialogue.IndexOf(']');

		if (openingBracketIndex != -1 && closingBracketIndex != -1)
		{
			// Extract the text between the brackets
			string name = dialogue.Substring(openingBracketIndex + 1, closingBracketIndex - openingBracketIndex - 1);

			// Extract the text after the closing bracket
			string text = dialogue.Substring(closingBracketIndex + 1);

			// Output the results
			if(text != null && name != null)
			{
				StartCoroutine(TextReveal(text, name));
			}
			else
			{
				Debug.LogError("Invalid Dialogue String!");
			}
		}		
	}

	IEnumerator TextReveal(string text, string name)
	{
		dialogueReveal = false;
		playerControl = false;
		
		// Assign name
		charName.text = name;
		
		
		dialogueText.text = "";
		for (int i = 0; i < text.Length + 1; i++)
		{
			string subText = text.Substring(0, i);
			dialogueText.text = subText;
			
			nonSpeech.Post(gameObject);
			
			if (skipDialogue)
			{
				nonSpeech.Post(gameObject);
				
				dialogueText.text = text;
				skipDialogue = false;
				break;		
			}
			yield return null;
			yield return new WaitForSeconds(textSpeed);
		}
			yield return null;
			playerControl = true;
			DialogueIndexHandler();
	}
}

