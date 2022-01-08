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
    private Material _blueGlow;
    private Material _redGlow;
    private Material _yellowGlow;
    private Material _greenGlow;


    public WorldAxis worldAxis;
    public float worldAxisTargetPoint;

    //public animCurve transitionCurve;
    private Vector3 _transformFirstPoint;
    public Vector3 _transformSecondPoint;

    private EVCameraTransition _EVcamTransitionScript;

    private float _elapsed;

    private float _dt;

    public AnimCurve curvePGO;

    private float _transitionTime;


    void Awake()
    {

        AssignPoints();
        LoadGlows();
        TagThisAndGetGlow();
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

     void LoadGlows()
    {
        _blueGlow = Resources.Load("Materials/Glows/Glow_Blue") as Material;
        _redGlow = Resources.Load("Materials/Glows/Glow_Red") as Material;
        _yellowGlow = Resources.Load("Materials/Glows/Glow_Yellow") as Material;
        _greenGlow = Resources.Load("Materials/Glows/Glow_Green") as Material;
    }


    void TagThisAndGetGlow()
    {

        switch (worldAxis)
        {
            case (WorldAxis.PGOxPositive):
                gameObject.tag = "PGOxPositive";
                GetComponent<Renderer>().material = _blueGlow;
                
                break;

            case (WorldAxis.PGOxNegative):
                gameObject.tag = "PGOxNegative";
                GetComponent<Renderer>().material = _redGlow;
                break;

            case (WorldAxis.PGOzPositive):
                gameObject.tag = "PGOzPositive";
                GetComponent<Renderer>().material = _greenGlow;
                break;

            case (WorldAxis.PGOzNegative):
                gameObject.tag = "PGOzNegative";
                GetComponent<Renderer>().material = _yellowGlow;
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
                _elapsed += _dt;
                transform.position = Vector3.Lerp(_transformFirstPoint, _transformSecondPoint, _elapsed /  _EVcamTransitionScript.duration);

               
                yield return null;
            }

        }
        else if (transform.position == _transformSecondPoint)
        {
            while (_elapsed <= _EVcamTransitionScript.duration)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;

                transform.position = Vector3.Lerp(_transformSecondPoint, _transformFirstPoint, _elapsed / _EVcamTransitionScript.duration);
        
                yield return null;
            }
        }




    }
}


