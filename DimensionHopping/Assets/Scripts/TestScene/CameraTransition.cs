using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public GameObject cam;
    public GameObject externalVariables;
    EVCameraTransition extVars;
    public CameraController cameraControl;
    public PlayerController playerControl;
    private Quaternion _rotation2DP;

    public GameObject[] arrayPGOxPositive;
    public GameObject[] arrayPGOxNegative;
    public GameObject[] arrayPGOzPositive;
    public GameObject[] arrayPGOzNegative;

    private TransformPositionOnPerspective _transformPGOScript;

    /*
    public GameObject block;
    public GameObject crane;
    public RedButton rb;
    */
    public GameObject player;

    public animCurve curve2DToFPP;
    public animCurve curveFPPTo2D;

    public bool switchingFrom2DtoFPP = false;
    public bool switchingFromFPPto2D = false;


    public List<TransformPositionOnPerspective> transformScripts = new List<TransformPositionOnPerspective>();
    //public TransformPositionOnPerspective[] transformPos;
    private float _elapsed;
    private float _dt;

    private void Start()
    {
        extVars = externalVariables.GetComponent<EVCameraTransition>();
        GetAllPGOs();
        GetAllPGOScripts();
    }


    private void Update()
    {
        _rotation2DP = Quaternion.Euler(cameraControl.current2DEulerAngles);


        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!cameraControl._is2DView)
            {

                cameraControl.Set2DCameraAngle();
                cameraControl.TrackingIn2d();
                switchingFromFPPto2D = true;



            }

            else if (cameraControl._is2DView)
            {

                switchingFrom2DtoFPP = true;



            }
            StartCoroutine(CamTransition());


            /*
            if(!cameraControl._is2DView)
            {
                crane.transform.position = crane.GetComponent<Crane>().positionFPP;
            }

            else if(cameraControl._is2DView)
            {
                block.transform.position = block.GetComponent<Block>().position2D;
                crane.transform.position = crane.GetComponent<Crane>().position2D;
            }
            */
        }


    }

    void GetAllPGOs()
    {
        arrayPGOxPositive = GameObject.FindGameObjectsWithTag("PGOxPositive");
        arrayPGOxNegative = GameObject.FindGameObjectsWithTag("PGOxNegative");
        arrayPGOzPositive = GameObject.FindGameObjectsWithTag("PGOzPositive");
        arrayPGOzNegative = GameObject.FindGameObjectsWithTag("PGOzNegative");

    }

    void GetAllPGOScripts()
    {


        if (arrayPGOxPositive != null)
        {   
            foreach (GameObject obj in arrayPGOxPositive)
            { 
                transformScripts.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }

        }

    }

    void TransformPGOPositions()
    {

        foreach (TransformPositionOnPerspective script in transformScripts)
            { 
                StartCoroutine(script.TransformPosition(extVars.duration));
    
            }


    }

    // Regelt die Camera Transition zwischen 2D und FPP

    private IEnumerator CamTransition()
    {
        _elapsed = 0f;

        // Wechselt von 2D zur FPP

        if (switchingFrom2DtoFPP)
        {
            TogglePlayerControl();

            while (_elapsed <= extVars.duration)
            {
                _dt = Time.deltaTime;
                _elapsed = _elapsed + _dt;
                //Debug.Log("Running");

                // Interpoliert Position und Rotation

                TransformPGOPositions();

                cam.transform.position = Vector3.Lerp(cameraControl.current2DPosition, cameraControl.FPPposition.transform.position, _elapsed / extVars.duration) + (new Vector3(curve2DToFPP.curve.Evaluate(_elapsed), 0f, 0f) * extVars.curveIntensity);

                cam.transform.rotation = Quaternion.Lerp(_rotation2DP, cameraControl.FPPposition.transform.rotation, _elapsed / extVars.duration);

                switchingFrom2DtoFPP = false;
                cameraControl._is2DView = false;
                yield return null;
            }


            cameraControl._is2DView = false;
            TogglePlayerControl();
        }

        // Wechselt von FPP zu 2D

        else if (switchingFromFPPto2D)
        {
            TogglePlayerControl();

            while (_elapsed <= extVars.duration)
            {
                _dt = Time.deltaTime;
                _elapsed = _elapsed + _dt;
                //Debug.Log("Running");

                TransformPGOPositions();

                // Interpoliert Position und Rotation

                cam.transform.position = Vector3.Lerp(cameraControl.FPPposition.transform.position, cameraControl.current2DPosition, _elapsed / extVars.duration) + (new Vector3(curveFPPTo2D.curve.Evaluate(_elapsed), 0f, 0f) * extVars.curveIntensity);
                cam.transform.rotation = Quaternion.Lerp(cameraControl.FPPposition.transform.rotation, _rotation2DP, _elapsed / extVars.duration);

                switchingFromFPPto2D = false;
                cameraControl._is2DView = true;
                yield return null;
            }

            cameraControl._is2DView = true;
            TogglePlayerControl();
        }


    }

    // Sorgt dafür, dass man während der Animation weder sich selbst, noch die Kamera drehen oder bewegen kann

    void TogglePlayerControl()
    {
        if (switchingFrom2DtoFPP || switchingFromFPPto2D)
        {
            cameraControl.enabled = false;
            playerControl.enabled = false;
        }

        else if (!switchingFrom2DtoFPP && !switchingFromFPPto2D)
        {
            cameraControl.enabled = true;
            playerControl.enabled = true;
        }
    }


}
