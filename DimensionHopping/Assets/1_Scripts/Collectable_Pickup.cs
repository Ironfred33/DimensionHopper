using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Regelt das Aufheben von Collectibles
public class Collectable_Pickup : MonoBehaviour
{
    public CollectablesUI collectableUI;

    public int collectibleOrder;

    public AudioSource collectSound;

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            collectSound.Play();
            collectableUI.FoundCollectable(collectibleOrder);
            this.gameObject.SetActive(false);
            Debug.Log("Collected disk");

        }
        
    }        

}
