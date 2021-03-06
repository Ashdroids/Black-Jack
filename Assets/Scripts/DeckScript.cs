using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    [SerializeField] Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;

    void Start() 
    {
        GetCardValues();
    }

    void GetCardValues()
    {
        int num = 0;
        //Loop to assign values to cards
        for(int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            //count up to amount of cards, 52
            num %= 13;
            // if there is a remainder after x/13, then the remainder
            // is used as the value, unless over 10, then use 10
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] =num++;
        }
    }

    public void Shuffle()
    {
        //Standard array data swapping technique
        // Iterate backwards to avoid Index 0 (Card Back)
        for (int i = cardSprites.Length -1; i > 0; --i)
        {
            int rnd = Mathf.FloorToInt(Random.Range(1f, 52f));
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[rnd];
            cardSprites[rnd] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues [rnd];
            cardValues[rnd] = value;
        } 
        currentIndex = 1;
    }

    public int DealCard(CardScript cardScript)
    {
        // Already shuffled so can use currentIndex
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex++]); // +1 to index after value set
        return cardScript.GetValueOfCard();
    }

    /*public Sprite GetCardBack()
    {
        return cardSprites[0];
    }*/
    
}
