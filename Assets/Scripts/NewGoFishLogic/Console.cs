using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public KeyCode consoleKey = KeyCode.F1;
    public GameObject consoleObject;
    TMP_InputField consoleInputField;
    GameController gc;

    void Start()
    {
        gc = GetComponent<GameController>();
        consoleInputField = consoleObject.GetComponent<TMP_InputField>();
    }

    void Update()
    {
        if(Input.GetKeyDown(consoleKey))
        {
            if (consoleObject.activeSelf)
            {
                consoleObject.SetActive(false);
            }
            else
                consoleObject.SetActive(true);
        }
    }

    public void CheckCommand()
    {
        string[] tokens = consoleInputField.text.Split(' ');

        if (tokens[0] == "check4")
        {
            if (tokens[1] == "player")
            {
                gc.Check4(gc.playerHand);
            }
            else if (tokens[1] == "opponent")
            {
                gc.Check4(gc.opponentHand);
            }
        }

        if (tokens[0] == "find")
        {
            bool check = GameObject.Find(tokens[1]);
            Debug.Log(check);
        }

        if (tokens[0] == "deinstantiate")
        {
            gc.DeInstantiateCard(tokens[1]);
        }

        if (tokens[0] == "drawName")
        {
            if (tokens[1] == "player")
            {
                gc.DrawCard(gc.playerHand, tokens[2]);
            }
            else if (tokens[1] == "opponent")
            {
                gc.DrawCard(gc.opponentHand, tokens[2]);
            }
        }

        if (tokens[0] == "draw")
        {
            if (tokens[1] == "player")
            {
                gc.DrawCard(gc.playerHand, Int32.Parse(tokens[2]));
            }
            else if (tokens[1] == "opponent")
            {
                gc.DrawCard(gc.opponentHand, Int32.Parse(tokens[2]));
            }
        }

        if (tokens[0] == "ping")
        {
            Debug.Log("Pong!");
        }

        consoleInputField.text = string.Empty;
    }
}
