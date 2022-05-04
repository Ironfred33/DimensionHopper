using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Verhindert, dass Spieler von beweglichen Plattformen fällt
public class PlatformInteraction : MonoBehaviour
{
    private GameObject _mover;
    private Vector3 _offset;

    void LateUpdate()
    {
        if(_mover != null)
        {
            _mover.transform.position = transform.position + _offset;
        }
        
    }

    void OnTriggerStay(Collider other)
    {
        _mover = other.gameObject;
        // _mover.transform.SetParent(this.gameObject.transform);
        
        _offset = _mover.transform.position - transform.position;
    }

    void OnTriggerExit(Collider other)
    {
        //_mover.transform.parent = null;
        
         _mover = null;

    }
}
