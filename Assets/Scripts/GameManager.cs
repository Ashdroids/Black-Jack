using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    int standClicks = 0;
    // How much is Bet
    int pot = 0;
    int betAmount = 20;
    

    [Header ("Game Buttons")]
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button doubleBtn;
    public Button betBtn;

    [Header ("Player/dealer's script")]
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    [Header ("Text to update HUD")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI dealerScoreText;
    public TextMeshProUGUI betsText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI mainText;
    public Text standBtnText;

    [Header ("Card Hiding dealers card")]
    public GameObject hideCard;

    

    void Start()
    {
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        betBtn.onClick.AddListener(() => BetClicked());
        //doubleBtn.onClick.AddListener(() => DoubleClicked());
    }


    void DealClicked()
    {
        //Reset Round, hide text, prep for new hand
        playerScript.ResetHand();
        dealerScript.ResetHand();
        // Hide dealer hand score and main text at start of deal
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update scores displayed
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // Hide one of dealers cards
        hideCard.GetComponent<Renderer>().enabled = true;
        //Adjust buttons visability
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
        // Set standard pot size
        pot = 40;
        betsText.text = pot.ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = playerScript.GetMoney().ToString();
    }

    void HitClicked()
    {
        //check there is still room on the table
        if(playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text ="Hand: " + playerScript.handValue.ToString();
            if(playerScript.handValue > 20) RoundOver();
        }
    }

    void StandClicked()
    {
        standClicks++;
        if(standClicks > 1)  RoundOver();
        HitDealer();
        standBtnText.text = "Call";
    }

    //void DoubleClicked(){}

    void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
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
        if(standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) {return;}

        bool roundOver = true;
        //Reveal dealers card
        hideCard.SetActive(false);
        
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
        // Set UI up for next hand/turn
        if(roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            dealerScoreText.gameObject.SetActive(true);
            cashText.text = playerScript.GetMoney().ToString();
            standClicks = 0;
        } 
    }

    //Add money to pot if bet clicked
    void BetClicked()
    {
        //Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        // Adds text of bet button so bet amount can later be updated
        // convert to int
        playerScript.AdjustMoney(-betAmount);
        cashText.text = playerScript.GetMoney().ToString();
        pot += (betAmount*2);
        betsText.text = pot.ToString();

    }
   
}
