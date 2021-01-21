using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMessage : MonoBehaviour
{

    public GameObject uiObject;

    // Start is called before the first frame update
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
