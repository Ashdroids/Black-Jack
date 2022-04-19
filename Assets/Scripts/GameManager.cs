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
    // public TextMeshProUGUI mainText;
    public Text standBtnText;

    [Header ("Card Hiding dealers card")]
    public GameObject hideCard;

    

    void Start()
    {
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        //doubleBtn.onClick.AddListener(() => DoubleClicked());
    }


    void DealClicked()
    {
        // Hide dealer hand score at start of deal
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Update score displayed
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
    }

    void HitClicked()
    {
        if(playerScript.GetCard() <= 10)
        {
            //check there is still room on the table
            playerScript.GetCard();
        }
    }

    void StandClicked()
    {
        standClicks++;
        if(standClicks > 1)  Debug.Log("end function");
        HitDealer();
        standBtnText.text = "Call";
    }

    //void DoubleClicked(){}

    void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            // Dealer score

        }
    }
   
}
