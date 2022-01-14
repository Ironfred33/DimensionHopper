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

    private GameObject[] _arrayPGOxPositive;
    private GameObject[] _arrayPGOxNegative;
    private GameObject[] _arrayPGOzPositive;
    private GameObject[] _arrayPGOzNegative;

    public GameObject compass;

    public GameObject crossHair;

    private TransformPositionOnPerspective _transformPGOScript;

    /*
    public GameObject block;
    public GameObject crane;
    public RedButton rb;
    */

    public GameObject player;

    public AnimCurve curve2DToFPP;
    public AnimCurve curveFPPTo2D;

    private float _transitionTime;

    public bool switchingFrom2DtoFPP = false;
    public bool switchingFromFPPto2D = false;


    public List<TransformPositionOnPerspective> transformScriptsPGOxPositive = new List<TransformPositionOnPerspective>();
    public List<TransformPositionOnPerspective> transformScriptsPGOxNegative = new List<TransformPositionOnPerspective>();
    public List<TransformPositionOnPerspective> transformScriptsPGOzPositive = new List<TransformPositionOnPerspective>();
    public List<TransformPositionOnPerspective> transformScriptsPGOzNegative = new List<TransformPositionOnPerspective>();


    //public TransformPositionOnPerspective[] transformPos;
    private float _elapsed;
    private float _dt;

    public bool _transitionInProgress;

    private void Start()
    {
        extVars = externalVariables.GetComponent<EVCameraTransition>();
        GetAllPGOs();
        GetAllPGOScripts();

        // if(cameraControl._is2DView) DisableObject(compass);
        // else if(!cameraControl._is2DView) EnableObject(compass);
    }


    private void Update()
    {
        _rotation2DP = Quaternion.Euler(cameraControl.current2DEulerAngles);


        if (Input.GetKeyDown(KeyCode.C) && !_transitionInProgress)
        {
            _transitionInProgress = true;            
			// end running/jumping animation
            playerControl.GetComponent<Animator>().SetBool("isRunning", false);
            playerControl.GetComponent<Animator>().SetBool("isJumping", false);
            playerControl.state = PlayerState.Idle;

            if (!cameraControl._is2DView)
            {

                cameraControl.Set2DCameraAngle();
                cameraControl.TrackingIn2d();
                switchingFromFPPto2D = true;
         

            }

            else if (cameraControl._is2DView)
            {

                switchingFrom2DtoFPP = true;
              

                if(playerControl.playerIsFlipped)
                {
                    player.transform.localScale = new Vector3(1, 1, 1);
                    playerControl.playerIsFlipped = false;
                }

            }
            StartCoroutine(CamTransition());
            TransformPGOPositions();


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


    // Regelt die Camera Transition zwischen 2D und FPP

    private IEnumerator CamTransition()
    {
        _elapsed = 0f;

        // Wechselt von 2D zur FPP

        if (switchingFrom2DtoFPP)
        {
            TogglePlayerControl();
            //TransformPGOPositions();
            

            while (_elapsed <= extVars.duration)
            {
                _dt = Time.deltaTime;
                _elapsed = _elapsed + _dt;
                _transitionTime = _elapsed + _dt;
                //Debug.Log("Running");

                // Interpoliert Position und Rotation

                

                cam.transform.position = Vector3.Lerp(cameraControl.current2DPosition, cameraControl.FPPposition.transform.position, _elapsed / extVars.duration) + (new Vector3(curve2DToFPP.curve.Evaluate(_elapsed), 0f, 0f) * extVars.curveIntensity);
                
                cam.transform.rotation = Quaternion.Lerp(_rotation2DP, cameraControl.FPPposition.transform.rotation, _elapsed / extVars.duration);

                // if (!playerControl.playerIsFlipped)
                // {
                //     cam.transform.rotation = Quaternion.Lerp(_rotation2DP,cameraControl.FPPposition.transform.rotation,  _elapsed / extVars.duration);
                // }
                // else if (playerControl.playerIsFlipped)
                // {
                //     cameraControl.FPPposition.transform.rotation = Quaternion.Lerp(_rotation2DP, new Quaternion(0, cameraControl.FPPposition.transform.rotation.y + 180, 0, 0), _elapsed / extVars.duration);
                // }

                

                switchingFrom2DtoFPP = false;
                cameraControl._is2DView = false;
                yield return null;
            }
            Debug.Log("Transition Time: " + _transitionTime);

            cameraControl._is2DView = false;
            _transitionInProgress = false;
            EnableObject(compass);
            EnableObject(crossHair);
            TogglePlayerControl();
        }

        // Wechselt von FPP zu 2D

        else if (switchingFromFPPto2D)
        {

            DisableObject(compass);
            DisableObject(crossHair);
            TogglePlayerControl();
            //TransformPGOPositions();

            while (_elapsed <= extVars.duration)
            {
                _dt = Time.deltaTime;
                _elapsed = _elapsed + _dt;
                //Debug.Log("Running");

                

                // Interpoliert Position und Rotation

                cam.transform.position = Vector3.Lerp(cameraControl.FPPposition.transform.position, cameraControl.current2DPosition, _elapsed / extVars.duration) + (new Vector3(curveFPPTo2D.curve.Evaluate(_elapsed), 0f, 0f) * extVars.curveIntensity);
                cam.transform.rotation = Quaternion.Lerp(cameraControl.FPPposition.transform.rotation, _rotation2DP, _elapsed / extVars.duration);

                switchingFromFPPto2D = false;
                cameraControl._is2DView = true;
                yield return null;
            }

            cameraControl._is2DView = true;
            _transitionInProgress = false;
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

    void GetAllPGOs()
    {
        _arrayPGOxPositive = GameObject.FindGameObjectsWithTag("PGOxPositive");
        _arrayPGOxNegative = GameObject.FindGameObjectsWithTag("PGOxNegative");
        _arrayPGOzPositive = GameObject.FindGameObjectsWithTag("PGOzPositive");
        _arrayPGOzNegative = GameObject.FindGameObjectsWithTag("PGOzNegative");

    }

    void GetAllPGOScripts()
    {


        if (_arrayPGOxPositive != null)
        {
            foreach (GameObject obj in _arrayPGOxPositive)
            {
                transformScriptsPGOxPositive.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }

        }

        if (_arrayPGOxNegative != null)
        {
            foreach (GameObject obj in _arrayPGOxNegative)
            {
                transformScriptsPGOxNegative.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }
        }

        if (_arrayPGOzPositive != null)
        {
            foreach (GameObject obj in _arrayPGOzPositive)
            {
                transformScriptsPGOzPositive.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }
        }

        if (_arrayPGOzNegative != null)
        {
            foreach (GameObject obj in _arrayPGOzNegative)
            {
                transformScriptsPGOzNegative.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }
        }

    }


    void TransformPGOPositions()
    {

        if (transformScriptsPGOxPositive != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.xPositive)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOxPositive)
            {
                StartCoroutine(script.TransformPosition());

            }
        }

        if (transformScriptsPGOxNegative != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.xNegative)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOxNegative)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzPositive != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.zPositive)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOzPositive)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzNegative != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.zNegative)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOzNegative)
            {
                StartCoroutine(script.TransformPosition());
            }
        }



    }

    void DisableObject(GameObject obj)
    {

        obj.SetActive(false);

    }

    void EnableObject(GameObject obj)
    {

        obj.SetActive(true);
    }


}
