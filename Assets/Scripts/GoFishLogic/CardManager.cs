using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	
	public Dictionary<string, Sprite> cardSprites = new Dictionary<string, Sprite>();
	public Dictionary<string, AnimationClip> cardAnims = new Dictionary<string, AnimationClip>();
	
	public Sprite defaultCardSprite;
	
	AnimationClip defaultAnim;
	
	void Start()
	{
		
		/*
		foreach (var rank in ranks)
		{
			foreach (var seed in seeds)
			{
				string cardName = rank + "-" + seed;
				AnimationClip cardAnim = Resources.Load<AnimationClip>("CardAnims/" + cardName);
				
				if(cardAnim != null)
				{
					cardAnims.Add(cardName, cardAnim);
				}
				else
				{
					cardAnims.Add(cardName, defaultAnim);
				}
			}
		}*/
		
		
			
		
	}
	
	public void InitializeDictionary()
	{
		string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
		string[] seeds = { "Hearts", "Diamonds", "Clubs", "Spades" };

		foreach (var rank in ranks)
		{
			foreach (var seed in seeds)
			{
				string cardName = rank + "-" + seed + "8";
				Sprite cardSprite = Resources.Load<Sprite>(cardName) as Sprite;
				
				if(cardSprite != null)
				{
					cardSprites.Add(cardName, cardSprite);
					Debug.Log("Loaded sprite for: " + cardName);
				}
				else
				{
					cardSprites.Add(cardName, defaultCardSprite);
					Debug.LogError("Sprite not found for: " + cardName);
				}
			}
		}
	}
	
	public Sprite GetCardSprite(string rank, string seed)
	{
		string cardName = rank + "-" + seed + "8";
			
		if(cardSprites.ContainsKey(cardName))
		{
			Debug.Log("Getting sprite for card: " + cardName);
			return cardSprites[cardName];
		}
		else
		{
			Debug.LogError("Sprite not found. Returning default sprite.");
			return defaultCardSprite;
		}
	}
}
