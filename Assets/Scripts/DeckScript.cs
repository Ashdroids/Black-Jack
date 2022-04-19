using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;

    void Start() 
    {
        
    }

    void GetCardValues()
    {
        int num = 0;
        //Loop to assign values to cards
        for(int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            num %= 13;
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] =num++;
        }
        currentIndex = 1;
    }
    
}
