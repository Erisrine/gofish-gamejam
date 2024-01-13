using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
	public List<Card> cards;
	
	public void InitializeDeck()
	{
		cards = new List<Card>();
		
		string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
		string [] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
		
		foreach (var suit in suits)
		{
			foreach (var rank in ranks)
			{
				cards.Add(new Card(suit, rank));
			}
		}
	}
	
	public void ShuffleDeck()
	{
		// Get the current UTC time
		// System.DateTime currentTime = System.DateTime.UtcNow;

		// Convert to Unix timestamp (seconds since the Unix epoch)
		// int randomSeed = (int)currentTime.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
		
		System.Random random = new System.Random();

		int n = cards.Count;
		while (n > 1)
		{
			n--;
			int k = random.Next(n + 1);
			Card temp = cards[k];
			cards[k] = cards[n];
			cards[n] = temp;
		}
	}
	
	public Card DrawCard()
	{
		if(cards.Count > 0)
		{
			Card drawnCard = cards[0];
			cards.RemoveAt(0);
			return drawnCard;
		}
		return null;
	}
}
