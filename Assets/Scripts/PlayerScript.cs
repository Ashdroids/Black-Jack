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
    int money = 1000;

    // Array of card objects on table
    public GameObject[] hand;
    // Index of next card to be turned over
    public int cardIndex = 0;

    // Tracking aces for 1 to 11 conversion
    List<CardScript> aceList = new List<CardScript>();

    public void StartHand()
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
        AceCheck();
        cardIndex++;
        return handValue;
    }

    public void AceCheck()
    {
        // foreach ace in the list (curently on the table)
        foreach(CardScript ace in aceList)
        {
            // if ace = 1 and adding 10 would be under 22
            if(handValue + 10 < 22 && ace.GetValueOfCard() == 1) 
            {
                ace.SetValue(11);
                handValue += 10;
            }
            // else if score is over 21 and ace = 11
            else if(handValue > 21 && ace.GetValueOfCard() == 11)
            {
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    public void AdjustMoney(int amount)
    {
        money += amount;
    }

// Output players current money amount
    public int GetMoney()
    {
        return money;
    }
}
