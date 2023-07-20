using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Works with Hearts as a UI element to provide a visual representation of player's current and
maximum hitpoints
*/

public class Health : MonoBehaviour
{
    public Player player;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {    
        if (player.hitpoint > numOfHearts) 
        {
            player.hitpoint = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < player.hitpoint)
            {
                hearts[i].sprite = fullHeart;
            } else {
                
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else 
            {
                hearts[i].enabled = true;
            }
        }
    }
}
