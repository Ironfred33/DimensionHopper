using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kontrolliert bewegliche Plattformen
public class MovingPlatforms : MonoBehaviour
{
    public Vector3 firstPosition;
    public Vector3 secondPosition;
    private Vector3 _offset;

    public float movementTime;
    private float _elapsed;
    private bool _movingToTarget;
    [SerializeField] private float _waitTime;
    [SerializeField] private float _elapsedWaitTime;

    void Start()
    {
        _movingToTarget = true;
    }

    void FixedUpdate()
    {
        _elapsed += Time.deltaTime;

       
        if(_movingToTarget)
        {
            transform.position = Vector3.Lerp(firstPosition, secondPosition, _elapsed/movementTime);

            if(_elapsed >= movementTime)
            {
                _elapsedWaitTime += Time.deltaTime;
                if(_elapsedWaitTime >= _waitTime)
                {
                    _elapsedWaitTime = 0f;
                    _elapsed = 0f;
                    _movingToTarget = false;
                }           
                
            }
        }

        else
        {
            transform.position = Vector3.Lerp(secondPosition, firstPosition, _elapsed/movementTime);

            if(_elapsed >= movementTime)
            {
                _elapsedWaitTime += Time.deltaTime;
                if(_elapsedWaitTime >= _waitTime)
                {
                    _elapsedWaitTime = 0f;
                    _elapsed = 0f;
                    _movingToTarget = true;
                }   
                
            }
        }
    }

}
