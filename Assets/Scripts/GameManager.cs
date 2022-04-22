using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    int standClicks = 0;
    int pot = 0;
    int betAmount = 20;
    bool atMaxBet;
    bool atMinBet = true;
    bool doubleClicked;
    Renderer hideCardRenderer;
    

    [Header ("Game Buttons")]
    [SerializeField] Button dealBtn;
    [SerializeField] Button hitBtn;
    [SerializeField] Button standBtn;
    [SerializeField] Button doubleBtn;
    [SerializeField] Button betBtn;
    [SerializeField] Button increaseBetBtn;
    [SerializeField] Button decreaseBetBtn;

    [Header ("Other scripts")]
    [SerializeField] PlayerScript playerScript;
    [SerializeField] PlayerScript dealerScript;
    [SerializeField] DeckScript deckScript;

    [Header ("Text to update HUD")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI dealerScoreText;
    [SerializeField] TextMeshProUGUI betsText;
    [SerializeField] TextMeshProUGUI cashText;
    [SerializeField] TextMeshProUGUI mainText;
    [SerializeField] TextMeshProUGUI changeBetText;
    [SerializeField] Text standBtnText;
    [SerializeField] Text betBtnText;

    [Header ("Card Hiding dealers card")]
    [SerializeField] GameObject hideCard;

    [Header ("Animations")]
    [SerializeField] GameObject winAnimation;

    

    void Start()
    {
        hideCardRenderer = hideCard.GetComponent<Renderer>();
        AddButtonListeners();
        StartCoroutine(PlaceBets(0f));
    }

    void AddButtonListeners()
    {
        // change to call from buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        betBtn.onClick.AddListener(() => BetClicked());
        doubleBtn.onClick.AddListener(() => DoubleClicked());
        increaseBetBtn.onClick.AddListener(() => IncreaseBetClicked());
        decreaseBetBtn.onClick.AddListener(() => DecreaseBetClicked());
    }

    void DealClicked()
    {
        // Hide dealer hand score and main text at start of deal
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        // Shuffle and deal
        deckScript.Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update scores 
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // Hide one of dealers cards
        hideCardRenderer.enabled = true;
        // Adjust buttons visability
        dealBtn.gameObject.SetActive(false);
        betBtn.gameObject.SetActive(false);
        doubleBtn.gameObject.SetActive(true);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
        // Hide bet change options
        changeBetText.gameObject.SetActive(false);
        increaseBetBtn.gameObject.SetActive(false);
        decreaseBetBtn.gameObject.SetActive(false);

        // if 2 cards of same value are dealt, show split button
        // Create SplitClicked() to play 2 hands
        
    }

    void HitClicked()
    {
        //check there is still room on the table
        if(playerScript.cardIndex <= 10)
        {
            // Remove double button
            doubleBtn.gameObject.SetActive(false);
            // Deal card
            playerScript.GetCard();
            // Update score
            scoreText.text ="Hand: " + playerScript.handValue.ToString();
            // check if roundOver
            if(playerScript.handValue > 20) RoundOver();
        }
    }

    void StandClicked()
    {
        // remove double button
        doubleBtn.gameObject.SetActive(false);
        // increase standClicks and end round if over 1
        standClicks++;
        if(standClicks > 1)  RoundOver();
        // deal card to dealer and change stand text to call 
        HitDealer();
        standBtnText.text = "Call";
    }

        
    void BetClicked()
    {   // if not enough money to bet, don't allow
        if(playerScript.GetMoney() < betAmount) {return;}
        // take bet from money and add double that to pot
        playerScript.AdjustMoney(-betAmount);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (betAmount*2);
        betsText.text = "Pot: $" + pot.ToString();
        // show deal button
        dealBtn.gameObject.SetActive(true);
        
    }

    void DoubleClicked()
    {
        // remove half pot from money then double pot
        playerScript.AdjustMoney(-pot/2);
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += pot;
        betsText.text = "Pot: $" + pot.ToString();
        // Adjust buttons visability
        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);
        doubleBtn.gameObject.SetActive(false);
        // Deal cards
        playerScript.GetCard();
        scoreText.text ="Hand: " + playerScript.handValue.ToString();
        while (dealerScript.handValue < 16)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        }
        // End Round
        doubleClicked = true;
        RoundOver();
    }

    void IncreaseBetClicked()
    {
        if(betAmount == 20)
        {
            betAmount = 50;
            decreaseBetBtn.gameObject.SetActive(true);
            atMinBet = false;
        }
        else if(betAmount == 50)
        {
            betAmount = 100;
            increaseBetBtn.gameObject.SetActive(false);
            atMaxBet = true;
        }
        betBtnText.text = betAmount.ToString();
    }

    void DecreaseBetClicked()
    {
        if(betAmount == 100)
        {
            betAmount = 50;
            increaseBetBtn.gameObject.SetActive(true);
            atMaxBet = false;
        }
        else if(betAmount == 50)
        {
            betAmount = 20;
            decreaseBetBtn.gameObject.SetActive(false);
            atMinBet = true;
        }
        betBtnText.text = betAmount.ToString();
    }

    void HitDealer()
    {
        // deal card if dealers hand value is < 16 or losing to player & there's room on the table
        while (dealerScript.handValue < 16 || dealerScript.handValue < playerScript.handValue && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            // end round if dealer bust
            if(dealerScript.handValue > 20) RoundOver();
        }
    }

    // Check for winner and loser, hand is over
    void RoundOver()
    {
        // bools for bust and blackjack
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        // If stand has been clicked less than twice & no 21's or busts, quit function
        if(standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21 && !doubleClicked) {return;}

        bool roundOver = true;
        //Reveal dealers card & show main text
        hideCardRenderer.enabled = false;
        mainText.gameObject.SetActive(true);
        
        //All bust, bets returned
        if(playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot/2);
        }
        //if player busts but dealer didn't, or if dealer has higher score, dealer wins
        else if(playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer Wins!";
        }
        // if dealer busts and player didn't, or player has more points, player wins
        else if(dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You Win!";
            playerScript.AdjustMoney(pot);
            winAnimation.SetActive(true);
        }
        // check for tie, return bets
        else if(playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Tie: Bets returned";
            playerScript.AdjustMoney(pot/2);
        }
        else
        {
            roundOver = false;
        }
        
        if(roundOver)
        {
            // Set UI up for next hand/turn
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            hideCardRenderer.enabled = false;
            dealerScoreText.gameObject.SetActive(true);
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
            // Go to place bets stage after 3 seconds
            StartCoroutine(PlaceBets(3f));
        } 
    }


    IEnumerator PlaceBets(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Reset Round, hide text, prep for new hand
        playerScript.ResetHand();
        dealerScript.ResetHand();
        // Reset Buttons
        betBtn.gameObject.SetActive(true);
        doubleBtn.gameObject.SetActive(false);
        // adjust text
        mainText.text = "Place your bets";
        dealerScoreText.gameObject.SetActive(false);
        scoreText.text ="Hand: ";
        // Reset pot 
        pot = 0;
        betsText.text = "Pot: $0";
        cashText.text = "$" + playerScript.GetMoney().ToString();
        // Show betting options
        changeBetText.gameObject.SetActive(true);
        if(!atMaxBet && !atMinBet)
        {
            increaseBetBtn.gameObject.SetActive(true);
            decreaseBetBtn.gameObject.SetActive(true);
        }
        else if (atMinBet)
        {
            increaseBetBtn.gameObject.SetActive(true);
        }
        else if (atMaxBet)
        {
            decreaseBetBtn.gameObject.SetActive(true);
        }
        // Reset animations
        winAnimation.SetActive(false);
    }
    
}
