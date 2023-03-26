using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lässt am Ende der Playtest-Session Nachricht auftauchen
public class FinishMessage : MonoBehaviour
{
    public GameObject uiObject;

    void Start()
    {
        uiObject.SetActive(false);
        
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            uiObject.SetActive(true);
        }
    }



    
}
