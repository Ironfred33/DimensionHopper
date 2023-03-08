using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGO : MonoBehaviour
{
    private CameraTransition _camTransitionScript;

    private Material _blueGlow;
    private Material _redGlow;
    private Material _yellowGlow;
    private Material _greenGlow;
    private GameObject _platformChild;
    public WorldAxis worldAxis;
    public Vector3 transformFirstPoint;
    public Vector3 transformSecondPoint;

    [HideInInspector] public float worldAxisTargetPoint;
    private float _elapsed;
    private float _dt;
    [HideInInspector] public float transitionDuration = 1f;

    public enum WorldAxis
    {

        PGOxPositive,
        PGOxNegative,
        PGOzPositive,
        PGOzNegative,

    }

    


    public void PlatformSetup()
    {

        AssignPoints();
        LoadGlows();
        TagThisAndGetGlow();

        if (worldAxis != WorldAxis.PGOxPositive) SwapTransformPoints();

    }



    void AssignPoints()
    {
        // First Point

        transformFirstPoint = this.transform.position;

        // Second Point

        if (worldAxis == WorldAxis.PGOxPositive || worldAxis == WorldAxis.PGOxNegative)
        {
            transformSecondPoint = new Vector3(transformFirstPoint.x, transformFirstPoint.y, worldAxisTargetPoint);
        }
        else if (worldAxis == WorldAxis.PGOzPositive || worldAxis == WorldAxis.PGOzNegative)
        {
            transformSecondPoint = new Vector3(worldAxisTargetPoint, transformFirstPoint.y, transform.position.z);
        }
    }

    void SwapTransformPoints()
    {
        if (this.transform.position == transformFirstPoint) this.transform.position = transformSecondPoint;
        else if (this.transform.position == transformSecondPoint) this.transform.position = transformFirstPoint;
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

        _platformChild = this.transform.Find("Cube").gameObject;

        switch (worldAxis)
        {
            case (WorldAxis.PGOxPositive):
                gameObject.tag = "PGOxPositive";
                _platformChild.GetComponent<Renderer>().material = _blueGlow;

                break;

            case (WorldAxis.PGOxNegative):
                gameObject.tag = "PGOxNegative";
                _platformChild.GetComponent<Renderer>().material = _redGlow;
                break;

            case (WorldAxis.PGOzPositive):
                gameObject.tag = "PGOzPositive";
                _platformChild.GetComponent<Renderer>().material = _greenGlow;
                break;

            case (WorldAxis.PGOzNegative):
                gameObject.tag = "PGOzNegative";
                _platformChild.GetComponent<Renderer>().material = _yellowGlow;
                break;

        }

    }



    void GetDuration()
    {
        _camTransitionScript = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<CameraTransition>();

    }

    public IEnumerator TransformPosition()
    {
        _elapsed = 0f;


        if (transform.position == transformFirstPoint)
        {

            while (_elapsed <= transitionDuration)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;
                transform.position = Vector3.Lerp(transformFirstPoint, transformSecondPoint, _elapsed / transitionDuration);


                yield return null;
            }

        }
        else if (transform.position == transformSecondPoint)
        {
            while (_elapsed <= transitionDuration)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;

                transform.position = Vector3.Lerp(transformSecondPoint, transformFirstPoint, _elapsed / transitionDuration);

                yield return null;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(this.transform);
            Debug.Log("Player is on Platform");
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = null;
            Debug.Log("Player Left Platform, OH NO!");
        }
    }


}
