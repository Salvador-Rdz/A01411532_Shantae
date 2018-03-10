using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour {
    //Used for spriteswitching from an array of available heart sprites
    public Sprite[] hearts;
    public int hp = 5;
    
    //Decreases the hp of the heart and updates sprite.
    public void takeDMG(int damage)
    {
        hp -= damage;
        if (hp != 0) GetComponent<Image>().sprite = hearts[hp];
        else setEmpty();
    }
    public int getCurrentHP()
    {
        return hp;
    }
    //sets the heart to an empty sprite.
    public void setEmpty()
    {
        GetComponent<Image>().sprite = hearts[0];
    }
}
