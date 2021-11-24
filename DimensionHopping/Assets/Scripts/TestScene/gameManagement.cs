using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManagement : MonoBehaviour
{
    
    public Vector3 SpawnCoords;
    private GameObject _externalVariables;
    private GameObject _playerPrefab;
    private EVPlayer _externalVariablesPlayerScript;
    private EVCamera _externalVariablesCameraScript;
    private PlayerHealth _playerHealthScript;
    private PlayerController _playerControllerScript;
    private CameraTransition _cameraTransitionScript;
    private CameraController _cameraControlScript;
    private Transform _trackingTarget;
    private GameObject _trackingPoint;
    private GameObject _canvas;
    private List <GameObject> _heartImages = new List <GameObject>();
    void Awake()
    {

        LoadPrefab();

        SpawnPlayer();

    }

    public void SpawnPlayer()
    {

        Instantiate(_playerPrefab, SpawnCoords, Quaternion.Euler(0, 90, 0));

        GetAllComponents();
        
        AssignComponentsToPlayer();

        AssignComponentsToCamera();

        FinalCameraSetup();

    }


    

    void LoadPrefab()
    {
        _playerPrefab = Resources.Load("Prefabs/character_v2") as GameObject;

    }

    void GetAllComponents()
    {
        // Player Scripts

        _playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        _playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        // External Variables und Script

        _externalVariables = GameObject.FindGameObjectWithTag("ExternalVariables");

        _externalVariablesPlayerScript = _externalVariables.GetComponent<EVPlayer>();


        // Camera Scripts

        _cameraControlScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();    

        _cameraTransitionScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTransition>();


        // Canvas Gameobject

        _canvas = GameObject.FindGameObjectWithTag("Canvas");


    }

    void AssignComponentsToPlayer()
    {
        // Player Health Script

        _playerHealthScript.externalPlayer = _externalVariablesPlayerScript;

        
        // Herz-Images des Canvas hinzufügen

        for (int i = 0; i < 3; i++)
        {
        _playerHealthScript.hearts[i] = _canvas.transform.GetChild(i).GetComponent<Image>();
        }


        _playerControllerScript.extVars = _externalVariablesPlayerScript;

        _playerControllerScript.cameraControl = _cameraControlScript;

    }

    void AssignComponentsToCamera()
    {

        _cameraControlScript.externalVariables = _externalVariables;

        _cameraControlScript.player = GameObject.FindGameObjectWithTag("Player");

        _cameraTransitionScript.externalVariables = _externalVariables;

        _cameraTransitionScript.playerControl = _playerControllerScript;

        _cameraTransitionScript.player = GameObject.FindGameObjectWithTag("Player");

    }

    void FinalCameraSetup()
    {
        _trackingTarget = GameObject.FindGameObjectWithTag("Player").transform;

        _cameraControlScript.trackingTarget = _trackingTarget;

        _trackingPoint = GameObject.FindGameObjectWithTag("PlayerTrackingPoint");
        
        _cameraControlScript.FPPposition = _trackingPoint;

    }

    








    
}
