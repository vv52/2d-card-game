using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw_cards : MonoBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    public GameObject PlayerArea;
    public GameObject OpponentArea;

    List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    public void OnClick()
    {
        for (var i = 0; i < 5; i++)
        {
            GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.transform.SetParent(PlayerArea.transform, false);

            GameObject opponentCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            opponentCard.transform.SetParent(OpponentArea.transform, false);
        }
    }
}
