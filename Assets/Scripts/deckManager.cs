using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deckManager : MonoBehaviour
{
    public GameObject[] Cards;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    public bool canDeal = true;
    public bool canHit = true;
    public bool canStay = true;

    public List<GameObject> cardsInPlay = new List<GameObject>();

    void Awake()
    {
        foreach (var card in Cards)
        {
            cardsInPlay.Add(card);
        }
    }

    public void Reshuffle()
    {
        cardsInPlay.Clear();
        foreach (var card in Cards)
        {
            cardsInPlay.Add(card);
        }
    }

    public void DealCards()
    {
        if (canDeal)
        {
            for (var i = 0; i < 2; i++)
            {
                int temp = Random.Range(0, cardsInPlay.Count);
            
                GameObject playerCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                playerCard.transform.SetParent(PlayerArea.transform, false);
                cardsInPlay.RemoveAt(temp);

                temp = Random.Range(0, cardsInPlay.Count);

                GameObject opponentCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                opponentCard.transform.SetParent(OpponentArea.transform, false);
                cardsInPlay.RemoveAt(temp);
            }
        }
        
    }

    public void HitPlayer()
    {
        if (canHit)
        {
            int temp = Random.Range(0, cardsInPlay.Count);

            GameObject playerCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            playerCard.transform.SetParent(PlayerArea.transform, false);
            cardsInPlay.RemoveAt(temp);
        }
    }

    public void StayPlayer()
    {
        if (canStay)
        {
            DealerTurn();
        }
    }

    public void DealerTurn()
    {
        
    }
}
