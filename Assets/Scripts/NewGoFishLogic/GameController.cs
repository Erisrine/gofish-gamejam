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

    void InitializeDeck()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            deck.Add(sprites[i].name);
        }
        ShuffleDeck(150);
    }

    void ShuffleDeck(int shuffleTimes)
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

    void DrawCard(Hand hand, int count)
    {
        for (int i = 0; i < count; i++)
        {
            hand.cards.Add(deck[0]);

            InstantiateCard(deck[0], hand);

            deck.RemoveAt(0);
        }
    }

    void RequestCard()
    {
        if (playerTurn)
        {
            bool hit = false;
            GameObject button = EventSystem.current.currentSelectedGameObject;
            string rank = GetRank(button.name);

            for (int i = 0; i < opponentHand.cards.Count; i++)
            {
                if (GetRank(opponentHand.cards[i]) == rank)
                {
                    hit = true;
                    playerHand.cards.Add(opponentHand.cards[i]);
                    DeInstantiateCard(opponentHand.cards[i]);
                    InstantiateCard(opponentHand.cards[i], playerHand);
                    opponentHand.cards.RemoveAt(i);
                }
            }
            if (hit == false)
            {
                DrawCard(playerHand, 1);
                playerTurn = false;
            }
        }
    }

    string GetRank(string card)
    {
        return card.Substring(0, card.LastIndexOf('-'));
    }

    void InstantiateCard(string cardToInst, Hand hand)
    {
        GameObject card = Instantiate(hand.cardPrefab, hand.handGroup);
        if(card.GetComponentInChildren<Text>() != null)
            card.GetComponentInChildren<Text>().text = cardToInst;
        card.name = cardToInst;
        if(card.GetComponent<Button>() != null)
            card.GetComponent<Button>().onClick.AddListener(RequestCard);

        Check4(hand);
    }

    void DeInstantiateCard(string cardToDeinst)
    {
        Destroy(GameObject.Find(cardToDeinst));
    }

    void Check4(Hand hand)
    {
        Dictionary<string, int> ranks = new Dictionary<string, int>();

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
                    Debug.Log(hand.cards[i]);
                    if (GetRank(hand.cards[i]) == pair.Key)
                    {
                        Debug.Log(hand.cards[i] + "*");
                        DeInstantiateCard(hand.cards[i]);
                        hand.cards.RemoveAt(i);
                        hand.score++;
                        i--;
                    }
                }
            }
        }
    }

    void OpponentTurnLevel1()
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
                InstantiateCard(playerHand.cards[i], opponentHand);
                playerHand.cards.RemoveAt(i);
                Debug.Log("Opponent got it!");
            }
        }
        if(hit == false)
        {
            DrawCard(opponentHand, 1);
            playerTurn = true;
            Debug.Log("Opponent goes fishing!");
        }
        else
        {
            OpponentTurnLevel1();
        }
    }

    void Update()
    {
        if (playerTurn == false)
        {
            OpponentTurnLevel1();
        }
    }
}
