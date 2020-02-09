﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deckManager : MonoBehaviour
{
    public GameObject[] Cards;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    public card_info cardInfo;

    private bool canDeal = false;
    private bool canHit = false;
    private bool canStay = false;
    private bool canBet = true;
    private bool playerBust = false;
    private bool canRefresh = false;

    int playerScore = 0;
    int playerAceCount = 0;
    int playerMoney = 100;
    public Text playerMoneyText;

    int dealerScore = 0;
    int dealerAceCount = 0;

    float betValue = 0;

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
        if (canDeal == true)
        {
            for (var i = 0; i < 2; i++)
            {
                int temp = Random.Range(0, cardsInPlay.Count);
            
                GameObject playerCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                playerCard.transform.SetParent(PlayerArea.transform, false);
                playerCards.Add(cardsInPlay[temp]);
                cardInfo = cardsInPlay[temp].GetComponent<card_info>();
                if (cardInfo.isAce)
                {
                    playerAceCount++;
                }
                cardsInPlay.RemoveAt(temp);

                temp = Random.Range(0, cardsInPlay.Count);

                GameObject opponentCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                opponentCard.transform.SetParent(OpponentArea.transform, false);
                dealerCards.Add(cardsInPlay[temp]);
                cardInfo = cardsInPlay[temp].GetComponent<card_info>();
                if (cardInfo.isAce)
                {
                    dealerAceCount++;
                }
                cardsInPlay.RemoveAt(temp);
            }
            canDeal = false;
            canHit = true;
            canStay = true;
            CheckCardsValue(playerCards);
        }
    }

    public void HitPlayer()
    {
        if (canHit == true && canDeal == false)
        {
            int temp = Random.Range(0, cardsInPlay.Count);

            GameObject playerCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            playerCard.transform.SetParent(PlayerArea.transform, false);
            playerCards.Add(cardsInPlay[temp]);
            cardInfo = cardsInPlay[temp].GetComponent<card_info>();
            if (cardInfo.isAce)
            {
                playerAceCount++;
            }
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

    void PlayerBust()
    {
        betValue = 0;
        playerBust = true;
        DealerTurn();

        //TODO: add BUST text
    }

    void PlayerBlackjack()
    {
        //TODO: add BLACKJACK text
        DealerTurn();
    }

    void DealerTurn()
    {
        int tempScore = 0;
        for (int i = 0; i < dealerCards.Count; i++)
        {
            cardInfo = dealerCards[i].GetComponent<card_info>();
            tempScore += cardInfo.cardValue;
        }
        dealerScore = tempScore;

        if (dealerScore < 17)
        {
            while (dealerScore < 17)
            {
                int temp = 0;

                temp = Random.Range(0, cardsInPlay.Count);

                GameObject opponentCard = Instantiate(cardsInPlay[temp], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                opponentCard.transform.SetParent(OpponentArea.transform, false);
                dealerCards.Add(cardsInPlay[temp]);
                cardInfo = cardsInPlay[temp].GetComponent<card_info>();
                if (cardInfo.isAce)
                {
                    dealerAceCount++;
                }
                cardsInPlay.RemoveAt(temp);
            }
        }
        if (dealerScore > 21)
        {
            if (dealerAceCount > 0)
            {
                while (dealerScore > 21 && dealerAceCount > 0)
                {
                    dealerScore -= 10;
                    dealerAceCount--;
                }
            }
        }

        ResolveTurn();   
    }

    public void Bet(int bet)
    {
        if (canBet)
        {
            betValue = bet;
            playerMoney -= bet;
            canDeal = true;
            canBet = false;
            playerMoneyText.text = "Money: " + playerMoney;
        }
    }

    void ResolveTurn()
    {
        if (playerBust)
        {
            playerMoney += 0;
        }
        else if (playerScore == dealerScore)
        {
            playerMoney += (int)betValue;
        }
        else if (playerScore == 21)
        {
            double tempNum = betValue * 2.5;
            playerMoney += (int)tempNum;
        }
        else if (playerScore > dealerScore)
        {
            double tempNum = betValue * 2;
            playerMoney += (int)tempNum;
        }
        playerMoneyText.text = "Money: " + playerMoney;
        NextTurn();
    }

    void NextTurn()
    {
        canRefresh = true;
        
        //TODO: reset all bools and nums that need to be reset to run a turn cycle
                //add a loopable call for a new turn
                //add a new function that checks for playerMoney reaching 0 or below
                //add a function that serves as an end state if the above condition is met
    }

    void RemoveCards()
    {
        while (playerCards.Count > 0)
        {
            Destroy(playerCards[0]);
        }

        while (dealerCards.Count > 0)
        {
            Destroy(dealerCards[0]);
        }
    }

    public void RefreshGameBoard()
    {
        if (canRefresh)
        {
            RemoveCards();

            canHit = false;
            canStay = false;
            canDeal = false;
            canBet = true;
            playerBust = false;
            playerScore = 0;
            playerAceCount = 0;
            dealerScore = 0;
            dealerAceCount = 0;
            betValue = 0;
            canRefresh = false;
        }
    }
}
