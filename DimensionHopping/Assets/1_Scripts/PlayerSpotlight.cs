using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert das Spotlight des Spieler-Charakters
public class PlayerSpotlight : MonoBehaviour
{
    private Transform _target;
    public Vector3 Offset;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.position + Offset;
    }
}
