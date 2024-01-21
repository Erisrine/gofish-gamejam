using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hand
{
    public List<string> cards;
    public Transform handGroup;
    public GameObject cardPrefab;
    public int[] ranks;
    public int score;
}
