using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
        
    }

    void StandClicked()
    {
        
    }

    //void DoubleClicked(){}
   
}
