using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Updated die Rotation des Kompass
public class Compass : MonoBehaviour
{


    private Transform _playerTransform;
    private Vector3 _direction;


    void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Update()
    {
        _direction.z = _playerTransform.eulerAngles.y;
        transform.localEulerAngles = _direction;

    }
}
