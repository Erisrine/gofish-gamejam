using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public List<Card> hand;
	
	public void AddToHand(Card card)
	{
		hand.Add(card);
		// Implement UI update for the player's hand
	}
	// Implement methods for checking pairs, asking opponents, etc.
	public void CheckForPairs()
	{
		Dictionary<string, List<Card>> rankGroups = new Dictionary<string, List<Card>>();

		// Group cards by rank
		foreach (Card card in hand)
		{
			if (!rankGroups.ContainsKey(card.rank))
			{
				rankGroups[card.rank] = new List<Card>();
			}

			rankGroups[card.rank].Add(card);
		}

		// Check for books (four of a kind)
		foreach (var pair in rankGroups)
		{
			int pairCount = pair.Value.Count;

			if (pairCount >= 4)
			{
				// Remove the set of four cards from the hand
				hand.RemoveAll(card => card.rank == pair.Key);

				// Score points for a book (four of a kind)
				Debug.Log("Scored 1 point for a book of rank " + pair.Key);
			}
		}

		// Implement any additional logic after checking for books
	}	

}
