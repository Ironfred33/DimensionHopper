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

    }

    public WorldAxis worldAxis;
    public float worldAxisTargetPoint;

    //public animCurve transitionCurve;
    private Vector3 _transformFirstPoint;
    public Vector3 _transformSecondPoint;

    private EVCameraTransition _EVcamTransitionScript;

    private float _elapsed;

    private float _dt;




    void Awake()
    {

        AssignPoints();
        TagThisGameObject();
        GetDuration();



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
        else if (worldAxis == WorldAxis.PGOzPositive || worldAxis == WorldAxis.PGOzNegative)
        {
            _transformSecondPoint = new Vector3(worldAxisTargetPoint, _transformFirstPoint.y, transform.position.z);
        }
    }

    private void Update()
    {
        Debug.Log("Duration im PGO Script: " + _EVcamTransitionScript.duration);
    }


    void TagThisGameObject()
    {

        switch (worldAxis)
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


    void GetDuration()
    {
        _EVcamTransitionScript = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVCameraTransition>();

    }

    // DURATION FUNKTIONIERT MOMENTAN NOCH NICHT KORREKT
    // BEI ZB. 5 SEKUNDEN PASSIERT DIE TRANSITION TROTZDEM IN NUR CA. 1 SEC

    public IEnumerator TransformPosition()
    {
        _elapsed = 0f;


        if (transform.position == _transformFirstPoint)
        {

            while (_elapsed <= _EVcamTransitionScript.duration)
            {
                _dt = Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, _transformSecondPoint, _elapsed / _EVcamTransitionScript.duration) ;
                _elapsed += _dt;
                yield return null;
            }

        }
        else if (transform.position == _transformSecondPoint)
        {
            while (_elapsed <= _EVcamTransitionScript.duration)
            {
                _dt = Time.deltaTime;
                
                transform.position = Vector3.Lerp(transform.position, _transformFirstPoint, _elapsed / _EVcamTransitionScript.duration * _EVcamTransitionScript.curveIntensity);
                _elapsed += _dt;
                yield return null;
            }
        }




    }
}


