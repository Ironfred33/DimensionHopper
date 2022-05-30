using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kontrolliert bewegliche Plattformen
public class MovingPlatforms : MonoBehaviour
{
    public Transform firstPosition;
    public Transform secondPosition;

    public float movementTime;
    private float _elapsed;
    private bool _movingToTarget;
    private Vector3 _offset;

    public float waitTime;
    public float elapsedWaitTime;

    void Start()
    {
        _movingToTarget = true;
    }

    void FixedUpdate()
    {
        _elapsed += Time.deltaTime;

        Debug.Log(_elapsed);
        if(_movingToTarget)
        {
            transform.position = Vector3.Lerp(firstPosition.position, secondPosition.position, _elapsed/movementTime);

            if(_elapsed >= movementTime)
            {
                elapsedWaitTime += Time.deltaTime;
                if(elapsedWaitTime >= waitTime)
                {
                    elapsedWaitTime = 0f;
                    _elapsed = 0f;
                    _movingToTarget = false;
                }           
                
            }
        }

        else
        {
            transform.position = Vector3.Lerp(secondPosition.position, firstPosition.position, _elapsed/movementTime);

            if(_elapsed >= movementTime)
            {
                elapsedWaitTime += Time.deltaTime;
                if(elapsedWaitTime >= waitTime)
                {
                    elapsedWaitTime = 0f;
                    _elapsed = 0f;
                    _movingToTarget = true;
                }   
                
            }
        }
    }

}