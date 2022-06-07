using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPlatform : MonoBehaviour
{
    [SerializeField] private float _boostForce;

    void OnCollisionEnter(Collision other)
    {
        Rigidbody _rb = other.gameObject.GetComponent<Rigidbody>(); 
        if(other.gameObject.CompareTag("Player"))
        {
            _rb.AddForce(transform.up*_boostForce, ForceMode.Impulse);
        }
    }

}
