using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Verhindert, dass Spieler von beweglichen Plattformen fällt
public class PlatformInteraction : MonoBehaviour
{
    private GameObject _mover;
    private Vector3 _offset;

    void OnCollisionEnter(Collision other)
    {
        _mover = other.gameObject;
        _mover.transform.SetParent(this.gameObject.transform);
        
    }

    void OnCollisionExit(Collision other)
    {
        _mover.transform.parent = null;

    }
}
