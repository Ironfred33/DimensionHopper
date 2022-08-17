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
        if (_playerTransform != null) _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Update()
    {
        if(_playerTransform == null)
        {
            if(GameObject.FindGameObjectWithTag("Player") != null) _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
        }

        else
        {
            _direction.z = _playerTransform.eulerAngles.y;
            transform.localEulerAngles = _direction;
        }
        
    }
}
