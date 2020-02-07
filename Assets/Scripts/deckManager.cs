using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deckManager : MonoBehaviour
{
    public GameObject[] Cards;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    public card_info cardInfo;

    public bool canDeal = true;
    public bool canHit = true;
    public bool canStay = true;

    int playerScore = 0;
    int playerAceCount = 0;
    int playerMoney = 100;

    int dealerScore = 0;
    int dealerAceCount = 0;
    int dealerMoney = 100;

    int betValue = 0;

    public List<GameObject> playerCards;
    public List<GameObject> dealerCards;

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
                playerCards.Add(cardsInPlay[temp]);
                cardsInPlay.RemoveAt(temp);

                temp = Random.Range(0, cardsInPlay.Count);

                GameObject opponentCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                opponentCard.transform.SetParent(OpponentArea.transform, false);
                dealerCards.Add(cardsInPlay[temp]);
                cardsInPlay.RemoveAt(temp);
            }
            CheckCardsValue(playerCards);
        }
        canDeal = false;
    }

    public void HitPlayer()
    {
        if (canHit == true && canDeal == false)
        {
            int temp = Random.Range(0, cardsInPlay.Count);

            GameObject playerCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            playerCard.transform.SetParent(PlayerArea.transform, false);
            playerCards.Add(cardsInPlay[temp]);
            cardsInPlay.RemoveAt(temp);
        }
        CheckCardsValue(playerCards);
    }

    public void StayPlayer()
    {
        if (canStay)
        {
            canHit = false;
            canStay = false;
            DealerTurn();
        }
    }

    public void CheckCardsValue(List<GameObject> hand)
    {
        int tempScore = 0;
        for (int i = 0; i < hand.Count; i++)
        {
            cardInfo = hand[i].GetComponent<card_info>();
            tempScore += cardInfo.cardValue;
            if (cardInfo.isAce)
            {
                playerAceCount++;
            }
        }

        playerScore = tempScore;

        if (playerScore > 21 && playerAceCount == 0)
        {
            canHit = false;
            canStay = false;
            PlayerBust();
            DealerTurn();
        }
        else if (playerScore > 21)
        {
            while (playerScore > 21 && playerAceCount > 0)
            {
                playerScore -= 10;
                playerAceCount--;
            }
            if (playerScore == 21)
            {
                canHit = false;
                canStay = false;
                PlayerBlackjack();
                DealerTurn();
            }
            else if (playerScore > 21)
            {
                canHit = false;
                canStay = false;
                PlayerBust();
                DealerTurn();
            }
        }
        else if (playerScore == 21)
        {
            canHit = false;
            canStay = false;
            PlayerBlackjack();
            DealerTurn();
        }
        else
        {
            canHit = true;
            canStay = true;
        }
    }

    public void PlayerBust()
    {
        
    }

    public void PlayerBlackjack()
    {

    }

    public void DealerTurn()
    {
        
    }
}
