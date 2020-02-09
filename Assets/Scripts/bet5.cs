using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bet5 : MonoBehaviour
{
    public deckManager deck;

    void Awake()
    {
        deck = GameObject.Find("GameManager").GetComponent<deckManager>();
    }

    public void OnClick()
    {
        deck.Bet(5);
    }
}
