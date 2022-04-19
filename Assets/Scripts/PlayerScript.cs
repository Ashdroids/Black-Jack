using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // --- Script used for BOTH Player and Dealer

    public CardScript cardScript;
    public DeckScript deckScript;


    // Total value of player/dealer's hand
    public int handValue = 0;

    // Betting Money
    //int money = 1000;

    // Array of card objects on table
    public GameObject[] hand;
    // Index of next card to be turned over
    public int cardIndex = 0;

    // Tracking aces for 1 to 11 conversion
    List<CardScript> aceList = new List<CardScript>();

    void Start()
    {
        GetCard();
        GetCard();
    }

    // add a hand to the player/dealer's hand
    public int GetCard()
    {
        // Get a card, use deal card to assign sprite and value to card on table
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());
        // show card on game screen
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        // Add card value to running total of the hand
        handValue += cardValue;
        // If value is 1 it's an ace
        if(cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }
        // Check if we should use 1 or 11
        // AceCheck();
        cardIndex++;
        return handValue;
    }
}
