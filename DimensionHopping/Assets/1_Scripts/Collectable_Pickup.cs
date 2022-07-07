using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Regelt das Aufheben von Collectibles
public class Collectable_Pickup : MonoBehaviour
{
    [SerializeField] private CollectablesUI _collectableUI;

    [SerializeField] private int _collectibleOrder;

    private AudioSource _collectSound;

    private void Start() 
    {
        _collectSound = GetComponent<AudioSource>();    
    }
    
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _collectSound.Play();
            _collectableUI.FoundCollectable(_collectibleOrder);
            this.gameObject.SetActive(false);
            Debug.Log("Collected disk");

        }
        
    }        

}
