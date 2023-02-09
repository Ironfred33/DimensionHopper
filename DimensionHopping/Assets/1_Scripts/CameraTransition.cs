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
    [HideInInspector] public PlayerController playerControl;
    private Quaternion _rotation2DP;
    public GameObject[] arrayPGOxPositive;
    public GameObject[] arrayPGOxNegative;
    public GameObject[] arrayPGOzPositive;
    public GameObject[] arrayPGOzNegative;
    public GameObject compass;
    public GameObject crossHair;
    [HideInInspector] public GameObject player;
    public AnimCurve curve2DToFPP;
    public AnimCurve curveFPPTo2D;
    public AudioSource droneSound;
    private float _transitionTime;
    private bool _levelGenerator;
    [HideInInspector] public bool switchingFrom2DtoFPP = false;
    [HideInInspector] public bool switchingFromFPPto2D = false;

    [SerializeField] private LayerMask _cullingMaskFp;
    [SerializeField] private LayerMask _cullingMask2D;



    // PGOskripte in bereits existierendem Level
    [HideInInspector] public List<TransformPositionOnPerspective> transformScriptsPGOxPositive = new List<TransformPositionOnPerspective>();
    [HideInInspector] public List<TransformPositionOnPerspective> transformScriptsPGOxNegative = new List<TransformPositionOnPerspective>();
    [HideInInspector] public List<TransformPositionOnPerspective> transformScriptsPGOzPositive = new List<TransformPositionOnPerspective>();
    [HideInInspector] public List<TransformPositionOnPerspective> transformScriptsPGOzNegative = new List<TransformPositionOnPerspective>();


    // PGOskripte für den Level Generator

    [HideInInspector] public List<PGO> transformScriptsPGOxPositiveGenerator = new List<PGO>();
    [HideInInspector] public List<PGO> transformScriptsPGOxNegativeGenerator = new List<PGO>();
    [HideInInspector] public List<PGO> transformScriptsPGOzPositiveGenerator = new List<PGO>();
    [HideInInspector] public List<PGO> transformScriptsPGOzNegativeGenerator = new List<PGO>();


    //public TransformPositionOnPerspective[] transformPos;
    private float _elapsed;
    private float _dt;

    [HideInInspector] public bool transitionInProgress;

    private void Start()
    {

        this.GetComponent<Camera>().cullingMask = _cullingMask2D;
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
                    player.transform.localScale = new Vector3(player.transform.localScale.x, player.transform.localScale.y, Mathf.Abs(player.transform.localScale.z));
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
            if (droneSound != null ) droneSound.Play();


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

            this.GetComponent<Camera>().cullingMask = _cullingMaskFp;
        }

        // Wechselt von FPP zu 2D

        else if (switchingFromFPPto2D)
        {

            DisableObject(compass);
            DisableObject(crossHair);
            TogglePlayerControl();
            if (droneSound != null ) droneSound.Play();
            this.GetComponent<Camera>().cullingMask = _cullingMask2D;

            while (_elapsed <= extVars.duration)
            {
                _dt = Time.deltaTime;
                _elapsed = _elapsed + _dt;

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

        if (transformScriptsPGOxPositive != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.XPositive)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOxPositive)
            {
                StartCoroutine(script.TransformPosition());

            }
        }

        if (transformScriptsPGOxNegative != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.XNegative)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOxNegative)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzPositive != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.ZPositive)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOzPositive)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzNegative != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.ZNegative)
        {
            foreach (TransformPositionOnPerspective script in transformScriptsPGOzNegative)
            {
                StartCoroutine(script.TransformPosition());
            }
        }



    }

    void TransformPGOPositionsGenerator()
    {
        if (transformScriptsPGOxPositiveGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.XPositive)
        {
            foreach (PGO script in transformScriptsPGOxPositiveGenerator)
            {
                StartCoroutine(script.TransformPosition());

            }
        }

        if (transformScriptsPGOxNegativeGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.XNegative)
        {
            foreach (PGO script in transformScriptsPGOxNegativeGenerator)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzPositiveGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.ZPositive)
        {
            foreach (PGO script in transformScriptsPGOzPositiveGenerator)
            {
                StartCoroutine(script.TransformPosition());
            }
        }

        if (transformScriptsPGOzNegativeGenerator != null && cameraControl.playerOrientation == CameraController.PlayerOrientation.ZNegative)
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
