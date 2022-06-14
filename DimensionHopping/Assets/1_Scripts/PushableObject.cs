using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    [SerializeField] private float _pushForce;
    [SerializeField] private Vector3 _startCoordinates;
    private Rigidbody _rb;

    void Start()
    {
        _startCoordinates = transform.position;
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnCollisionStay(Collision other)
    {
        if(Input.GetKey(KeyCode.E) && other.gameObject.CompareTag("Player"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Vector3 pushDirection = transform.position - other.transform.position;
            pushDirection.y = 0;
            pushDirection.Normalize();

            _rb.MovePosition(transform.position + pushDirection * _pushForce * Time.deltaTime);
            Debug.Log("Pushing");
        }
    }

    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            _rb.velocity = new Vector3(0,0,0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("OutOfBounds"))
        {
            this.gameObject.SetActive(false);
            transform.position = _startCoordinates;
            this.gameObject.SetActive(true);

        }
    }
}
