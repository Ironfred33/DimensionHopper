using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialToolTips : MonoBehaviour
{
    // Start is called before the first frame update

    private bool _insideCollider;
    public GameObject textToShow;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            Debug.Log("Triggered!");

            EnableObject(textToShow);

        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            DisableObject(textToShow);

            Debug.Log("Exit!");

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
