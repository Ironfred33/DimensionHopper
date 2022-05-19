using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Regelt den Perspektivwechsel
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

    private bool _levelGenerator;

    public bool switchingFrom2DtoFPP = false;
    public bool switchingFromFPPto2D = false;



    // PGOskripte in bereits existierendem Level
    public List<TransformPositionOnPerspective> transformScriptsPGOxPositive = new List<TransformPositionOnPerspective>();
    public List<TransformPositionOnPerspective> transformScriptsPGOxNegative = new List<TransformPositionOnPerspective>();
    public List<TransformPositionOnPerspective> transformScriptsPGOzPositive = new List<TransformPositionOnPerspective>();
    public List<TransformPositionOnPerspective> transformScriptsPGOzNegative = new List<TransformPositionOnPerspective>();


    // PGOskripte für den Level Generator

    public List<PGO> transformScriptsPGOxPositiveGenerator = new List<PGO>();
    public List<PGO> transformScriptsPGOxNegativeGenerator = new List<PGO>();
    public List<PGO> transformScriptsPGOzPositiveGenerator = new List<PGO>();
    public List<PGO> transformScriptsPGOzNegativeGenerator = new List<PGO>();


    //public TransformPositionOnPerspective[] transformPos;
    private float _elapsed;
    private float _dt;

    public bool transitionInProgress;

    private void Start()
    {

        extVars = externalVariables.GetComponent<EVCameraTransition>();

        CheckForLevelGeneratorInScene();
        

        // wenn kein levelgenerator in Szene
        if (!_levelGenerator)
        {
            GetAllPGOs();
            GetAllPGOScripts();

        }

       
    }



    // wird über den levelgenerator zugegriffen, sobald plattformen generiert wurden, um sie zu pgos zu machen
    public void PGOSetup()
    {

        Debug.Log("PGO SETUP IN CAM TRANS TRIGGERED");
        GetAllPGOs();
        GetAllPGOScriptsGenerator();
    }

    // überprüft, ob ein levelgenerator in der szene ist
    void CheckForLevelGeneratorInScene()
    {
        if(GameObject.FindGameObjectWithTag("LevelGenerator") != null) _levelGenerator = true;
        else _levelGenerator = false;
    }


    private void Update()
    {
        _rotation2DP = Quaternion.Euler(cameraControl.current2DEulerAngles);


        if (Input.GetKeyDown(KeyCode.C) && !transitionInProgress)
        {
            transitionInProgress = true;
            // end running/jumping animation
            playerControl.GetComponent<Animator>().SetBool("isRunning", false);
            playerControl.GetComponent<Animator>().SetBool("isJumping", false);
            playerControl.state = PlayerState.Idle;

            if (!cameraControl.is2DView)
            {

                cameraControl.Set2DCameraAngle();
                cameraControl.TrackingIn2d();
                switchingFromFPPto2D = true;


            }

            else if (cameraControl.is2DView)
            {

                switchingFrom2DtoFPP = true;


                if (playerControl.playerIsFlipped)
                {
                    player.transform.localScale = new Vector3(1, 1, 1);
                    playerControl.playerIsFlipped = false;
                }

            }
            StartCoroutine(CamTransition());

            if(!_levelGenerator) TransformPGOPositions();
            else if(_levelGenerator) TransformPGOPositionsGenerator();

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

                // Interpoliert Position und Rotation

                cam.transform.position = Vector3.Lerp(cameraControl.current2DPosition, cameraControl.fpPosition.transform.position, _elapsed / extVars.duration) + (new Vector3(curve2DToFPP.curve.Evaluate(_elapsed), 0f, 0f) * extVars.curveIntensity);

                cam.transform.rotation = Quaternion.Lerp(_rotation2DP, cameraControl.fpPosition.transform.rotation, _elapsed / extVars.duration);

                switchingFrom2DtoFPP = false;
                cameraControl.is2DView = false;
                yield return null;
            }
            Debug.Log("Transition Time: " + _transitionTime);

            cameraControl.is2DView = false;
            transitionInProgress = false;
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

                cam.transform.position = Vector3.Lerp(cameraControl.fpPosition.transform.position, cameraControl.current2DPosition, _elapsed / extVars.duration) + (new Vector3(curveFPPTo2D.curve.Evaluate(_elapsed), 0f, 0f) * extVars.curveIntensity);
                cam.transform.rotation = Quaternion.Lerp(cameraControl.fpPosition.transform.rotation, _rotation2DP, _elapsed / extVars.duration);

                switchingFromFPPto2D = false;
                cameraControl.is2DView = true;
                yield return null;
            }

            cameraControl.is2DView = true;
            transitionInProgress = false;
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

    // Greift alle PGOs auf
    void GetAllPGOs()
    {
        arrayPGOxPositive = GameObject.FindGameObjectsWithTag("PGOxPositive");
        arrayPGOxNegative = GameObject.FindGameObjectsWithTag("PGOxNegative");
        arrayPGOzPositive = GameObject.FindGameObjectsWithTag("PGOzPositive");
        arrayPGOzNegative = GameObject.FindGameObjectsWithTag("PGOzNegative");

    }

    // Greif alle PGOs auf, wenn das Level zufällig generiert wird
    void GetAllPGOScriptsGenerator()
    {
        if (arrayPGOxPositive != null)
        {
            foreach (GameObject obj in arrayPGOxPositive)
            {
                transformScriptsPGOxPositiveGenerator.Add(obj.GetComponent<PGO>());
            }

        }

        if (arrayPGOxNegative != null)
        {
            foreach (GameObject obj in arrayPGOxNegative)
            {
                transformScriptsPGOxNegativeGenerator.Add(obj.GetComponent<PGO>());
            }
        }

        if (arrayPGOzPositive != null)
        {
            foreach (GameObject obj in arrayPGOzPositive)
            {
                transformScriptsPGOzPositiveGenerator.Add(obj.GetComponent<PGO>());
            }
        }

        if (arrayPGOzNegative != null)
        {
            foreach (GameObject obj in arrayPGOzNegative)
            {
                transformScriptsPGOzNegativeGenerator.Add(obj.GetComponent<PGO>());
            }
        }

    }

    // Weist PGO Script den entsprechenden Objekten zu
    void GetAllPGOScripts()
    {


        if (arrayPGOxPositive != null)
        {
            foreach (GameObject obj in arrayPGOxPositive)
            {
                transformScriptsPGOxPositive.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }

        }

        if (arrayPGOxNegative != null)
        {
            foreach (GameObject obj in arrayPGOxNegative)
            {
                transformScriptsPGOxNegative.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }
        }

        if (arrayPGOzPositive != null)
        {
            foreach (GameObject obj in arrayPGOzPositive)
            {
                transformScriptsPGOzPositive.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }
        }

        if (arrayPGOzNegative != null)
        {
            foreach (GameObject obj in arrayPGOzNegative)
            {
                transformScriptsPGOzNegative.Add(obj.GetComponent<TransformPositionOnPerspective>());
            }
        }

    }

    // Verschiebt PGOs sofern die Kamera entsprechend gedreht wird
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

    void TransformPGOPositionsGenerator()
    {
        if (transformScriptsPGOxPositiveGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.xPositive)
        {
            foreach (PGO script in transformScriptsPGOxPositiveGenerator)
            {
                StartCoroutine(script.TransformPosition());

            }
        }

        if (transformScriptsPGOxNegativeGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.xNegative)
        {
            foreach (PGO script in transformScriptsPGOxNegativeGenerator)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzPositiveGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.zPositive)
        {
            foreach (PGO script in transformScriptsPGOzPositiveGenerator)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzNegativeGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.zNegative)
        {
            foreach (PGO script in transformScriptsPGOzNegativeGenerator)
            {
                StartCoroutine(script.TransformPosition());
            }
        }


    }

    // Setzt Gameobjekt inaktiv
    void DisableObject(GameObject obj)
    {

        obj.SetActive(false);

    }

    // Setzt Gameobjekt aktiv
    void EnableObject(GameObject obj)
    {

        obj.SetActive(true);
    }


}
