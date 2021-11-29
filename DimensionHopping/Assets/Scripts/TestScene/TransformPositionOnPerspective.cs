using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPositionOnPerspective : MonoBehaviour
{

    public enum WorldAxis
    {

        xAxis,
        zAxis,

    }

    public WorldAxis worldAxis;
    public float worldAxisTargetPoint;
    private Vector3 _transformFirstPoint;
    private Vector3 _transformSecondPoint;

    private float _elapsed;

    public float transitionDuration;


    void Start()
    {
        _transformFirstPoint = this.transform.position;

        if (worldAxis == WorldAxis.zAxis)
        {
            _transformSecondPoint = new Vector3(_transformFirstPoint.x, _transformFirstPoint.y, worldAxisTargetPoint);
        }
        else if (worldAxis == WorldAxis.xAxis)
        {
            _transformSecondPoint = new Vector3(worldAxisTargetPoint, _transformFirstPoint.y, transform.position.z);
        }

    }


    // void TransformPosition()
    // {

    //     if (transform.position == _transformFirstPoint)
    //     {
    //         transform.position = _transformSecondPoint;
    //     }
    //     else if (transform.position == _transformSecondPoint)
    //     {
    //         transform.position = _transformFirstPoint;
    //     }


    // }

    void TagThisGameObject()
    {



    }

    private IEnumerator TransformPosition(Vector3 firstPos, Vector3 secPos)
    {
        _elapsed = 0f;

        if (transform.position == _transformFirstPoint)
        {

            while (_elapsed <= transitionDuration)
            {
                _elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, _transformSecondPoint, _elapsed / transitionDuration);
                 yield return null;
            }

        }
        else if (transform.position == _transformSecondPoint)
        {
            while (_elapsed <= transitionDuration)
            {
                _elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, _transformFirstPoint, _elapsed / transitionDuration);
                 yield return null;
            }
        }



    }





    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {

            StartCoroutine(TransformPosition(_transformFirstPoint, _transformSecondPoint));


        }

    }
}
