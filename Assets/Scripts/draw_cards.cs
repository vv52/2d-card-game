﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw_cards : deckManager
{
    public deckManager deck;

    void Awake()
    {
        deck = GameObject.Find("GameManager").GetComponent<deckManager>();
    }

    public void OnClick()
    {
        deck.DealCards();
    }
}
