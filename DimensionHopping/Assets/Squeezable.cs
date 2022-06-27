using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squeezable : MonoBehaviour
{

    public float maximumSqueeze;

    public float squeezeSpeed;
    public float squeezeAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other) {
        
        if(other.transform.tag == "Squeezer")
        {
            Debug.Log("SQUEEEZE");
        }
    }

    private void OnTriggerStay(Collider other) {

       if(other.transform.tag == "Squeezer")
       {

    
       } 
    }

    private void OnTriggerExit(Collider other) {
        if(other.transform.tag == "Squeezer")
        {
            Debug.Log("NON SQUEEZE");
        }
    }
}
