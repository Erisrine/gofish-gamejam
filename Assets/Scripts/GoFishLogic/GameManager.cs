using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Deck deck;
	public Player player;
	public Player opponent;
	
	[SerializeField]
	public CardManager cardManager;
	private bool isPlayerTurn;
	
	public Transform[] spawnPoints;
	
	public GameObject cardPrefab;
	
	void Start()
	{
		// Initialize the CardManager directly
		
		cardManager.InitializeDictionary();
		Debug.Log("Manager Start()");
		//deck = GetComponent<Deck>();
		//player = GetComponent<Player>();
		//opponent = GetComponent<Player>();
		
		deck.InitializeDeck();
		deck.ShuffleDeck();	
		StartGame();
	}
	
	void StartGame()
	{
		Debug.Log("StartGame()");
		 // Deal initial cards to players
		for (int i = 0; i < 5; i++)
		{
			DrawNewCard();
			opponent.AddToHand(deck.DrawCard()); //make better
		}
		for (int i = 0; i < opponent.hand.Count; i++)
        {
			Debug.Log(opponent.hand[i].rank);
        }

		StartPlayerTurn();
	}
	
	public void StartPlayerTurn()
	{
		isPlayerTurn = true;
		
	}
	
	public void StartOpponentTurn()
	{
		// Implement opponent turn logic (AI decision-making, asking player for cards, etc.)
		// After the opponent's turn, switch to the player's turn
		StartPlayerTurn();
	}
	
	public void DrawNewCard()
	{
		Debug.Log("DrawNewCard()");
		Card drawnCard = DrawCard();
		Debug.Log("Card Drawn");
		player.AddToHand(drawnCard);
		Debug.Log("Card Added to Player Hands");
		
		
		if (drawnCard != null)
		{
			player.AddToHand(drawnCard);
			Debug.Log("Drew a new card: " + drawnCard.rank + " of " + drawnCard.suit);
			// Add logic for handling the newly drawn card in the player's hand or other game actions
			SpawnCard(drawnCard);
		}
		else
		{
			Debug.Log("No more cards in the deck.");
			// Add logic for when the deck is empty (if needed)
		}
	}
	
	Card DrawCard()
	{
		Debug.Log("Card DrawCard()");
		return deck.DrawCard();
		
	}
	
	void SpawnCard(Card cardData)
	{
		foreach (Transform spawnPoint in spawnPoints)
		{
			if (!IsSpawnPointOccupied(spawnPoint))
			{
				Debug.Log("Spawning Card");
				GameObject newCardObject = Instantiate(cardPrefab, spawnPoint.position, Quaternion.identity);
				CardMonoBehaviour cardMonoBehaviour = newCardObject.GetComponent<CardMonoBehaviour>();
				
				// Set properties of the new card
				Sprite cardSprite = cardManager.GetCardSprite(cardData.rank, cardData.suit); // Assuming cardManager is a reference
				cardMonoBehaviour.Initialize(cardData, cardSprite);
				
				newCardObject.transform.parent = spawnPoint;
				
				return;
			}
		}
	}
	
	bool IsSpawnPointOccupied(Transform spawnPoint)
	{
		return spawnPoint.childCount > 0;
	}
	
	void Update()
	{
		if(isPlayerTurn)
		{
			HandlePlayerInput();
		}
		
		if(Input.GetKeyDown(KeyCode.Space)){DrawNewCard();}
		//Opponent AI?
	}
	void HandlePlayerInput()
	{
		// Example: Check for mouse click
		if (Input.GetMouseButtonDown(0))
		{
			// Raycast to determine which card the player clicked on
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				// Check if the hit object is a card in the player's hand
				Card clickedCard = hit.collider.GetComponent<CardMonoBehaviour>().CardData;
				if (clickedCard != null && player.hand.Contains(clickedCard))
				{
					// Perform actions based on the clicked card
					AskOpponentForCards(clickedCard);
				}
			}
		}
	}
	
	 void AskOpponentForCards(Card selectedCard)
	{
		// Implement logic for asking the opponent for cards based on the selected card
		string requestedRank = selectedCard.rank;
        Debug.Log("Asking opponent for a card: " + requestedRank);

        // Check if the opponent has cards of the requested rank
        List<Card> opponentCards = opponent.hand.FindAll(card => card.rank == requestedRank);

		if (opponentCards.Count > 0)
		{
			// Transfer the cards from the opponent to the player
			foreach (Card card in opponentCards)
			{
				opponent.hand.Remove(card);
				player.AddToHand(card);
			}

			// Implement UI or other feedback for successful request

			// Check for pairs in the player's hand (you can implement this method in the Player class)
			player.CheckForPairs();

			// Player gets another turn after a successful request
			// You might want to handle this based on your game rules
		}
		else
		{
			DrawNewCard();
			// Go Fish: Draw a card from the deck
			//Card drawnCard = deck.DrawCard();
			//player.AddToHand(drawnCard);

			// Implement UI or other feedback for "Go Fish"

			// Check for pairs in the player's hand after drawing a card
			//player.CheckForPairs();

			// Switch to the opponent's turn
			//isPlayerTurn = false;
			StartOpponentTurn();
		}
		
		
		// Update game state, UI, etc.
		// After the player's turn, switch to the opponent's turn
		isPlayerTurn = false;
		StartOpponentTurn();
	}
}
