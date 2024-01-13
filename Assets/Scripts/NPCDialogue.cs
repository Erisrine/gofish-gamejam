using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
	[SerializeField] private List<string> dialogue;
	[SerializeField] private List<string> dialogue2;
	[SerializeField] private bool enableSecondList = true;

	void OnTriggerEnter2D(Collider2D col)
	{
		
			DialogueSystem.Instance.SetDialogueList(1);
			SendDialogue();
			DialogueSystem.isInDialogueRange = true;
			Debug.Log("OnTriggerEnter2D");
			enableSecondList = false;
	}	
		
	void OnTriggerExit2D(Collider2D col)
	{
	 	DialogueSystem.isInDialogueRange = false;
		Debug.Log("OnTriggerExit2D");
	}
	 
	void SendDialogue()
	{
	 	DialogueSystem.Instance.GetList(dialogue, dialogue2);
	}
	
}
