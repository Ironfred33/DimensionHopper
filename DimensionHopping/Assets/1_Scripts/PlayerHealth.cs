using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// UI der Herzen
public class PlayerHealth : MonoBehaviour
{
    public EVPlayer externalPlayer;
    public int currentHearts;

    public Image[] hearts;
    [SerializeField] private Sprite _fullHeart;
    [SerializeField] private Sprite _emptyHeart;

   

    // Update is called once per frame
    void Update()
    {
        if(currentHearts > externalPlayer.maxHearts)
        {
            currentHearts = externalPlayer.maxHearts;
        }
        
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < currentHearts)
            {
                hearts[i].sprite = _fullHeart;
            }
            else
            {
                hearts[i].sprite = _emptyHeart;
            }
            
            if(i < externalPlayer.maxHearts)
            {
                hearts[i].enabled = true;
            }

            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
