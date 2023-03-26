using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert Tooltips im Tutorial
public class TutorialToolTips : MonoBehaviour
{
    public GameObject textToShow;

    private bool _insideCollider;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            EnableObject(textToShow);

        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DisableObject(textToShow);


        }


    }

    void DisableObject(GameObject obj)
    {

        obj.SetActive(false);

    }

    void EnableObject(GameObject obj)
    {

        obj.SetActive(true);
    }



}
