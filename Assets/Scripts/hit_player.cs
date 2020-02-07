using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit_player : deckManager
{
    public deckManager deck;

    void Awake()
    {
        deck = GameObject.Find("GameManager").GetComponent<deckManager>();
    }

    public void OnClick()
    {
        deck.HitPlayer();
    }
}
