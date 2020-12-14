using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform trackedObject;
    public float updateSpeed = 3;
    public Vector3 trackingOffset;
    private Vector3 offset;
    void Start()
    {
        offset = (Vector3)trackingOffset;
        offset.z = transform.position.z - trackedObject.position.z;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, trackedObject.position + offset, updateSpeed * Time.deltaTime);
        
    }
}
