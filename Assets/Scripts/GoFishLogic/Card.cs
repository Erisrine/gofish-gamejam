using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string suit { get; private set; }
    public string rank { get; private set; }

    public Card(string _suits, string _ranks)
    {
        suit = _suits;
        rank = _ranks;
    }
}



