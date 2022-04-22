using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Value of card, 2 of clubs = 2 etc
    int value = 0;

    void Start() 
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public int GetValueOfCard()
    {
        return value;
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }

    // For Debugging
    public string GetSpriteName()
    {
        return spriteRenderer.sprite.name;
    }

    public void SetSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }

    public void ResetCard()
    {
        value = 0;
    }
}
