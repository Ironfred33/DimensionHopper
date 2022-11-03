using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    [SerializeField] private float _pushForce;
    [SerializeField] private Vector3 _startCoordinates;
    private Rigidbody _rb;
    [SerializeField] private bool isOnGround;
    public Transform groundCheck;
    public float groundRadius;
    public LayerMask groundMask;

    void Start()
    {
        _startCoordinates = transform.position;
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    void FixedUpdate()
    {
        isOnGround = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
    }

    void OnCollisionStay(Collision other)
    {
        if(Input.GetKey(KeyCode.E) && other.gameObject.CompareTag("Player"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Vector3 pushDirection = transform.position - other.transform.position;
            if(Mathf.Abs(pushDirection.x) > Mathf.Abs(pushDirection.z))
            {
                if(pushDirection.x >= 0)
                {
                    pushDirection = new Vector3(1,0,0);
                }
                else if(pushDirection.x < 0)
                {
                    pushDirection = new Vector3(-1,0,0);
                }
            }
            else if(Mathf.Abs(pushDirection.z) >= Mathf.Abs(pushDirection.x))
            {
                if(pushDirection.z >= 0)
                {
                    pushDirection = new Vector3(0,0,1);
                }
                else if(pushDirection.z < 0)
                {
                    pushDirection = new Vector3(0,0,-1);
                }
            }

            _rb.MovePosition(transform.position + pushDirection * _pushForce * Time.deltaTime);
            Debug.Log("Pushing");
        }

        if (isOnGround && other.collider.CompareTag("PGOzNegative") || other.collider.CompareTag("PGOzPositive") || other.collider.CompareTag("PGOxPositive") || other.collider.CompareTag("PGOxNegative") || other.collider.CompareTag("MovingPlatform") || other.collider.CompareTag("Pushable"))
        {
            Debug.Log("Parenting");
            this.transform.SetParent(other.collider.transform);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            _rb.velocity = new Vector3(0,0,0);
        }

        this.transform.SetParent(null);
        
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
