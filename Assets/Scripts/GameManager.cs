using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int standClicks = 0;
    public Text standBtnText;

    [Header ("Game Buttons")]
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button doubleBtn;
    public Button betBtn;

    [Header ("Player/dealer's script")]
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    void Start()
    {
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        //doubleBtn.onClick.AddListener(() => DoubleClicked());
    }


    void DealClicked()
    {
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
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
