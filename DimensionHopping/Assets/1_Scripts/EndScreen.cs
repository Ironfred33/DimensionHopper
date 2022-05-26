using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Steuert den Endscreen
public class EndScreen : MonoBehaviour
{
  

    public void BackToMenu()
    {
        Debug.Log("CLICK");
        SceneManager.LoadScene(0);
        
        }

    
}
