using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deckManager : MonoBehaviour
{
    public GameObject[] Cards;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    public card_info cardInfo;
    public Image cardBack;
    public Image cardFace;

    private bool canDeal = false;
    private bool canHit = false;
    private bool canStay = false;
    private bool canBet = true;
    private bool playerBust = false;
    private bool dealerBust = false;
    private bool canRefresh = false;

    int playerScore = 0;
    int playerAceCount = 0;
    int playerMoney = 100;
    public Text playerMoneyText;
    public Text blackJackText;
    public Text bustText;

    int dealerScore = 0;
    int dealerAceCount = 0;

    float betValue = 0;

    public List<GameObject> playerCards;
    public List<GameObject> dealerCards;

    public List<GameObject> cardsInPlay = new List<GameObject>();

    void Awake()
    {
        cardBack.enabled = false;
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
            cardBack.enabled = true;
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
            }
            else if (playerScore > 21)
            {
                canHit = false;
                canStay = false;
                PlayerBust();
            }
        }
        else if (playerScore == 21)
        {
            canHit = false;
            canStay = false;
            PlayerBlackjack();
        }
        else
        {
            canHit = true;
            canStay = true;
        }
    }

    void PlayerBust()
    {
        cardBack.enabled = false;
        betValue = 0;
        playerBust = true;
        bustText.text = "Bust!";
        Invoke("DealerTurn", 3);
    }

    void PlayerBlackjack()
    {
        cardBack.enabled = false;
        blackJackText.text = "BlackJack!";
        Invoke("DealerTurn", 3);
    }

    void DealerTurn()
    {
        cardBack.enabled = false;
        bustText.text = "";
        blackJackText.text = "";
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
                DealerHit();
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
        if (dealerScore > 21)
        {
            dealerBust = true;
        }
        ResolveTurn();   
    }

    void DealerHit()
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
        dealerScore += cardInfo.cardValue;
        cardsInPlay.RemoveAt(temp);
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
            bustText.text = "Dealer win...";
        }
        else if (dealerBust)
        {
            double tempNum = betValue * 2;
            playerMoney += (int)tempNum;
            blackJackText.text = "You win!!";
        }
        else if (playerScore == dealerScore)
        {
            playerMoney += (int)betValue;
            bustText.text = "Draw...";
        }
        else if (playerScore == 21)
        {
            double tempNum = betValue * 2.5;
            playerMoney += (int)tempNum;
            blackJackText.text = "You win!!";
        }
        else if (playerScore > dealerScore)
        {
            double tempNum = betValue * 2;
            playerMoney += (int)tempNum;
            blackJackText.text = "You win!!";
        }
        else
        {
            playerMoney += 0;
            bustText.text = "Dealer win...";
        }
        playerMoneyText.text = "Money: " + playerMoney;
        Invoke("NextTurn", 3);
    }

    void NextTurn()
    {
        canRefresh = true;
        bustText.text = "";
        blackJackText.text = "";

        RefreshGameBoard();
    }

    void RemoveCards()
    {
        foreach (Transform child in PlayerArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        playerCards.Clear();

        foreach (Transform child in OpponentArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        dealerCards.Clear();
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
            dealerBust = false;
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
