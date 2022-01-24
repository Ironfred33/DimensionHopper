using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInteraction : MonoBehaviour
{
    private GameObject _mover;
    private Vector3 _offset;

    // Update is called once per frame
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
        _offset = _mover.transform.position - transform.position;
    }

    void OnTriggerExit(Collider other)
    {
        _mover = null;
    }
}
