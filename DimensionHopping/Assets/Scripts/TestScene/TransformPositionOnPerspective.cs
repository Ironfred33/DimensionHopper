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


    void TransformPosition()
    {

        if (transform.position == _transformFirstPoint)
        {
            transform.position = _transformSecondPoint;
        }
        else if (transform.position == _transformSecondPoint)
        {
            transform.position = _transformFirstPoint;
        }

    }



    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {

            TransformPosition();


        }

    }
}
