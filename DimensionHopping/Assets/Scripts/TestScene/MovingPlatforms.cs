using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Transform firstPosition;
    public Transform secondPosition;

    public float movementTime;
    private float _elapsed;
    public bool _movingToTarget;

    void Start()
    {
        _movingToTarget = true;
    }

    void Update()
    {
        _elapsed += Time.deltaTime;

        Debug.Log(_elapsed);
        if(_movingToTarget)
        {
            transform.position = Vector3.Lerp(firstPosition.position, secondPosition.position, _elapsed/movementTime);

            if(_elapsed >= movementTime)
            {
                _elapsed = 0f;
                _movingToTarget = false;
            }
        }

        else
        {
            transform.position = Vector3.Lerp(secondPosition.position, firstPosition.position, _elapsed/movementTime);

            if(_elapsed >= movementTime)
            {
                _elapsed = 0f;
                _movingToTarget = true;
            }
        }
    }



}
