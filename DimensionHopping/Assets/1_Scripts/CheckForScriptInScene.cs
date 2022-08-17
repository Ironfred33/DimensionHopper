using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForScriptInScene : MonoBehaviour
{
    
    // Hier Skript einfügen, das gesucht werden soll
    


    void Start()
    {
        StateBar[] scriptsOnScene = FindObjectsOfType<StateBar>();

        if(scriptsOnScene == null) Debug.Log("No Script in Scene");

        else {

            foreach(StateBar s in scriptsOnScene)
            {
                Debug.Log("Script on the GameObject: " + s.gameObject.name);
            }
        } 

        
    }
}

   
