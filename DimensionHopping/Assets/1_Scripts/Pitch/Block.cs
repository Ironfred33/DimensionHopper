using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public RedButton button;
    private Rigidbody blockRigidbody;
    public Vector3 position2D;
    public Vector3 positionFPP;

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.tag == "Deadly")
        {
            Debug.Log(other);
            Destroy(other.gameObject);
            button.enemiesDefeated = true;
            blockRigidbody.isKinematic = true;
        }
    }
    
}
