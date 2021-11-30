using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPositionOnPerspective : MonoBehaviour
{

    public enum WorldAxis
    {

        PGOxPositive,
        PGOxNegative,
        PGOzPositive,
        PGOzNegative,

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

        AssignPoints();
        TagThisGameObject();

    }

    void AssignPoints()
    {
        // First Point

        _transformFirstPoint = this.transform.position;

        // Second Point
        
        if (worldAxis == WorldAxis.PGOxPositive || worldAxis == WorldAxis.PGOxNegative)
        {
            _transformSecondPoint = new Vector3(_transformFirstPoint.x, _transformFirstPoint.y, worldAxisTargetPoint);
        }
        else if (worldAxis == WorldAxis.PGOzPositive|| worldAxis == WorldAxis.PGOzNegative)
        {
            _transformSecondPoint = new Vector3(worldAxisTargetPoint, _transformFirstPoint.y, transform.position.z);
        }
    }


    void TagThisGameObject()
    {

        switch(worldAxis)
        {
            case (WorldAxis.PGOxPositive):
            gameObject.tag = "PGOxPositive";
            break;

            case (WorldAxis.PGOxNegative):
            gameObject.tag = "PGOxNegative";
            break;

            case (WorldAxis.PGOzPositive):
            gameObject.tag = "PGOzPositive";
            break;

            case (WorldAxis.PGOzNegative):
            gameObject.tag = "PGOzNegative";
            break;

        }

    }

    public IEnumerator TransformPosition(Vector3 firstPos, Vector3 secPos, float duration)
    {
        _elapsed = 0f;

        if (transform.position == _transformFirstPoint)
        {

            while (_elapsed <= duration)
            {
                _elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, _transformSecondPoint, _elapsed / duration);
                 yield return null;
            }

        }
        else if (transform.position == _transformSecondPoint)
        {
            while (_elapsed <= duration)
            {
                _elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, _transformFirstPoint, _elapsed / duration);
                 yield return null;
            }
        }



    }





    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {

            StartCoroutine(TransformPosition(_transformFirstPoint, _transformSecondPoint, transitionDuration));


        }

    }
}
