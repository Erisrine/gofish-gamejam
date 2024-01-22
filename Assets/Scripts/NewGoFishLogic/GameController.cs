using AK.Wwise;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameController : MonoBehaviour
{
    public Sprite[] sprites;

    public List<string> deck;

    public Hand playerHand;
    public Hand opponentHand;

    bool playerTurn = true;

    void Start()
    {
        InitializeDeck();
        DrawCard(playerHand, 5);
        DrawCard(opponentHand, 5);
    }

    public void InitializeDeck()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            deck.Add(sprites[i].name);
        }
        ShuffleDeck(150);
    }

    public void ShuffleDeck(int shuffleTimes)
    {
        if(deck.Count > 1)
        {
            for (int i = 0; i < shuffleTimes; i++)
            {
                string tmp;
                int rng2 = -1;
                int rng1 = Random.Range(0, deck.Count);
                do
                {
                    rng2 = Random.Range(0, deck.Count);
                }
                while(rng1==rng2);
                tmp = deck[rng1];
                deck[rng1] = deck[rng2];
                deck[rng2] = tmp;
            }
        }
    }

    public void DrawCard(Hand hand, int count)
    {
        for (int i = 0; i < count; i++)
        {
            hand.cards.Add(deck[0]);

            hand.ranks[RankToInt(GetRank(deck[0]))]++;
            InstantiateCard(deck[0], hand);

            deck.RemoveAt(0);
        }
    }
    public void DrawCard(Hand hand, string cardName)
    {
        if (deck.Contains(cardName))
        {
            hand.cards.Add(cardName);
            hand.ranks[RankToInt(GetRank(cardName))]++;
            InstantiateCard(cardName, hand);
            deck.Remove(cardName);
        }
    }

    public void RequestCard()
    {
        if (playerTurn)
        {
            bool hit = false;
            GameObject button = EventSystem.current.currentSelectedGameObject;
            string rank = GetRank(button.name);
            Debug.Log("Player asked for " + rank);
            for (int i = 0; i < opponentHand.cards.Count; i++)
            {
                if (GetRank(opponentHand.cards[i]) == rank)
                {
                    Debug.Log("Player got it!");
                    hit = true;
                    string card = opponentHand.cards[i];
                    playerHand.cards.Add(card);
                    DeInstantiateCard(card);
                    opponentHand.cards.RemoveAt(i);
                    opponentHand.ranks[RankToInt(GetRank(card))]--;
                    playerHand.ranks[RankToInt(GetRank(card))]++;
                    InstantiateCard(card, playerHand);
                    i--;
                }
            }
            if (hit == false)
            {
                Debug.Log("Player goes fishing!");
                string topCard = deck[0];
                Debug.Log("Player drew " + playerHand.cards[playerHand.cards.Count - 1]);
                DrawCard(playerHand, 1);
                if (GetRank(playerHand.cards[playerHand.cards.Count - 1]) == GetRank(topCard) && GetRank(playerHand.cards[playerHand.cards.Count - 1]) != rank)
                {
                    playerTurn = false;
                }
                else
                    Debug.Log("Player Catch!");
            }
        }
    }

    public string GetRank(string card)
    {
        return card.Substring(0, card.LastIndexOf('-'));
    }

    public void InstantiateCard(string cardToInst, Hand hand)
    {
        GameObject card = Instantiate(hand.cardPrefab, hand.handGroup);
        if(card.GetComponentInChildren<Text>() != null)
            card.GetComponentInChildren<Text>().text = cardToInst;
        card.name = cardToInst;
        if(card.GetComponent<Button>() != null)
            card.GetComponent<Button>().onClick.AddListener(RequestCard);
        Check4(hand);
    }

    public void DeInstantiateCard(string cardToDeinst)
    {
        Destroy(GameObject.Find(cardToDeinst));
    }

    public void Check4(Hand hand)
    {
        for (int i = 0; i < hand.ranks.Length; i++)
        {
            if (hand.ranks[i] == 4)
            {
                for (int t = 0; t < hand.cards.Count; t++)
                {
                    if (RankToInt(GetRank(hand.cards[t])) == i)
                    {
                        Debug.Log("Score " + hand.cards[t]);
                        hand.cards.RemoveAt(t);
                        hand.score++;
                        t--;
                    }
                }
                for (int t = 0; t < hand.handGroup.childCount; t++)
                {
                    if (RankToInt(GetRank(hand.handGroup.GetChild(t).name)) == i)
                    {
                        Destroy(hand.handGroup.GetChild(t).gameObject);
                    }
                }
            }
        }
        /*Dictionary<string, int> ranks = new Dictionary<string, int>();

        for (int i = 0; i < hand.cards.Count; i++)
        {
            if (ranks.ContainsKey(GetRank(hand.cards[i]))){
                ranks[GetRank(hand.cards[i])]++;
            }
            else
            {
                ranks.Add(GetRank(hand.cards[i]), 1);
            }
        }
        foreach (KeyValuePair<string,int> pair in ranks)
        {
            if(pair.Value == 4)
            {
                for (int i = 0; i < hand.cards.Count; i++)
                {
                    if (GetRank(hand.cards[i]) == pair.Key)
                    {
                        Debug.Log(hand.cards[i] + "Removed");
                        DeInstantiateCard(hand.cards[i]);
                        hand.cards.RemoveAt(i);
                        hand.score++;
                        i--;
                    }
                }
            }
        }*/
    }

    public void OpponentTurnLevel1()
    {
        bool hit = false;
        int rndCardId = Random.Range(0, opponentHand.cards.Count);
        Debug.Log("Opponent asked for: " + GetRank(opponentHand.cards[rndCardId]));
        for (int i = 0; i < playerHand.cards.Count; i++)
        {
            if (GetRank(playerHand.cards[i]) == GetRank(opponentHand.cards[rndCardId]))
            {
                hit = true;
                opponentHand.cards.Add(playerHand.cards[i]);
                DeInstantiateCard(playerHand.cards[i]);
                playerHand.ranks[RankToInt(GetRank(playerHand.cards[i]))]--;
                opponentHand.ranks[RankToInt(GetRank(playerHand.cards[i]))]++;
                InstantiateCard(playerHand.cards[i], opponentHand);
                playerHand.cards.RemoveAt(i);
                Debug.Log("Opponent got it!");
                i--;
            }
        }
        if(hit == false)
        {
            Debug.Log("Opponent goes fishing!");
            DrawCard(opponentHand, 1);
            Debug.Log("Opponent Drew " + opponentHand.cards[opponentHand.cards.Count-1]);
            string topCard = deck[0];
            if (GetRank(opponentHand.cards[opponentHand.cards.Count - 1]) == GetRank(topCard) && GetRank(opponentHand.cards[opponentHand.cards.Count - 1]) != GetRank(opponentHand.cards[rndCardId]))
                playerTurn = true;
            else
            {
                Debug.Log("Opponent Catch");
                OpponentTurnLevel1();
            }
        }
        else
        {
            OpponentTurnLevel1();
        }
    }

    int RankToInt(string rank)
    {
        int returnValue = 0;
        switch(rank)
        {
            case "2":
                returnValue = 0;
                break;
            case "3":
                returnValue = 1;
                break;
            case "4":
                returnValue = 2;
                break;
            case "5":
                returnValue = 3;
                break;
            case "6":
                returnValue = 4;
                break;
            case "7":
                returnValue = 5;
                break;
            case "8":
                returnValue = 6;
                break;
            case "9":
                returnValue = 7;
                break;
            case "10":
                returnValue = 8;
                break;
            case "Jack":
                returnValue = 9;
                break;
            case "Queen":
                returnValue = 10;
                break;
            case "King":
                returnValue = 11;
                break;
            case "Ace":
                returnValue = 12;
                break;
        }
        return returnValue;
    }

    void Update()
    {
        if (playerTurn == false)
        {
            OpponentTurnLevel1();
        }
    }
}
