using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Verschiebt PGOs 
public class TransformPositionOnPerspective : MonoBehaviour
{

    private EVCameraTransition _EVcamTransitionScript;
    private EVPlatformFloating _floating;
    private PlatformFloating _floatingScript;

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
    [HideInInspector] public Vector3 transformFirstPoint;
    [HideInInspector] public Vector3 transformSecondPoint;

    [HideInInspector] public Vector3 transformFirstPointLowered;
    [HideInInspector] public Vector3 transformSecondPointLowered;

    public float worldAxisTargetPoint;
    private float _elapsed;
    private float _dt;
    public bool originalPosition;


    void Awake()
    {
        _floating = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVPlatformFloating>();
        originalPosition = true;
        AssignPoints();
        LoadGlows();
        TagThisAndGetGlow();
        GetDuration();
        CalculateNewPositions();

    }

    void CalculateNewPositions()
    {

        if (this.GetComponent<PlatformFloating>() == true) _floatingScript = this.GetComponent<PlatformFloating>();

        transformFirstPointLowered = new Vector3(transformFirstPoint.x, transformFirstPoint.y - _floating.loweringAmount, transformFirstPoint.z);

        transformSecondPointLowered = new Vector3(transformSecondPoint.x, transformSecondPoint.y - _floating.loweringAmount, transformSecondPoint.z);


    }


    // Weist Positionen in beiden Perspektiven zu
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

    // Lädt Materialien für Glow Effekt
    void LoadGlows()
    {
        _blueGlow = Resources.Load("Materials/Glows/FloatEffect_Blue") as Material;
        _redGlow = Resources.Load("Materials/Glows/FloatEffect_Red") as Material;
        _yellowGlow = Resources.Load("Materials/Glows/FloatEffect_Yellow") as Material;
        _greenGlow = Resources.Load("Materials/Glows/FloatEffect_Green") as Material;
    }

    // Vergibt Tags an PGOs und weist Materialien entsprechend zu
    void TagThisAndGetGlow()
    {

        switch (worldAxis)
        {
            case (WorldAxis.PGOxPositive):
                gameObject.tag = "PGOxPositive";
                transform.Find("EnergyField").GetComponent<Renderer>().material = _blueGlow;
                break;

            case (WorldAxis.PGOxNegative):
                gameObject.tag = "PGOxNegative";
                transform.Find("EnergyField").GetComponent<Renderer>().material = _redGlow;
                break;

            case (WorldAxis.PGOzPositive):
                gameObject.tag = "PGOzPositive";
                transform.Find("EnergyField").GetComponent<Renderer>().material = _greenGlow;
                break;

            case (WorldAxis.PGOzNegative):
                gameObject.tag = "PGOzNegative";
                transform.Find("EnergyField").GetComponent<Renderer>().material = _yellowGlow;
                break;

        }

    }


    // Greift Dauer des Perspektivwechsels auf
    void GetDuration()
    {
        _EVcamTransitionScript = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVCameraTransition>();

    }

    // Verschiebt PGOs 
    public IEnumerator TransformPosition()
    {

        originalPosition = SwitchBool(originalPosition);

        _elapsed = 0f;


        if (transform.position == transformFirstPoint)
        {

            while (_elapsed <= _EVcamTransitionScript.duration)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;
                transform.position = Vector3.Lerp(transformFirstPoint, transformSecondPoint, _elapsed / _EVcamTransitionScript.duration);


                yield return null;
            }

        }
        else if (transform.position == transformSecondPoint)
        {
            while (_elapsed <= _EVcamTransitionScript.duration)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;

                transform.position = Vector3.Lerp(transformSecondPoint, transformFirstPoint, _elapsed / _EVcamTransitionScript.duration);

                yield return null;
            }
        }

        else if (transform.position == transformFirstPointLowered)
        {

            while (_elapsed <= _EVcamTransitionScript.duration)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;
                transform.position = Vector3.Lerp(transformFirstPointLowered, transformSecondPointLowered, _elapsed / _EVcamTransitionScript.duration);


                yield return null;
            }

        }
        else if (transform.position == transformSecondPointLowered)
        {

            while (_elapsed <= _EVcamTransitionScript.duration)
            {
                _dt = Time.deltaTime;
                _elapsed += _dt;

                transform.position = Vector3.Lerp(transformSecondPointLowered, transformFirstPointLowered, _elapsed / _EVcamTransitionScript.duration);

                yield return null;
            }

        }

    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.collider.CompareTag("MovingPlatform"))
        {
            Debug.Log("Parenting");
            transform.SetParent(collisionInfo.collider.transform);
        }

    }

    void OnCollisionExit(Collision other)
    {
        transform.SetParent(null);
    }

    bool SwitchBool(bool b)
    {
        if (b == true) b = false;
        else if (b == false) b = true;

        return b;
    }

}


