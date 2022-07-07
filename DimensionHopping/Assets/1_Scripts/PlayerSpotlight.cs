using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert das Spotlight des Spieler-Charakters
public class PlayerSpotlight : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private Vector3 _offset;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.position + _offset;
    }
}
