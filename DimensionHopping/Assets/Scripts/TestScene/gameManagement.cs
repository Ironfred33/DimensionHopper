using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManagement : MonoBehaviour
{
    

    public GameObject externalVariables;
    public Vector3 SpawnCoords;
    private GameObject _playerPrefab;
    private EVPlayer _externalVariablesPlayerScript;
    private EVCamera _externalVariablesCameraScript;
    private PlayerHealth _playerHealthScript;
    private PlayerController _playerControllerScript;
    private CameraTransition _cameraTransitionScript;
    private CameraController _cameraControlScript;
    private Transform _trackingTarget;
    private GameObject _trackingPoint;
    
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
        _playerHealthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        _playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _externalVariablesPlayerScript = externalVariables.GetComponent<EVPlayer>();
        _cameraControlScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();      
        _cameraTransitionScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTransition>();


    }

    void AssignComponentsToPlayer()
    {
        _playerHealthScript.externalPlayer = _externalVariablesPlayerScript;
        _playerControllerScript.extVars = _externalVariablesPlayerScript;
        _playerControllerScript.cameraControl = _cameraControlScript;

    }

    void AssignComponentsToCamera()
    {

        _cameraControlScript.externalVariables = externalVariables;
        _cameraControlScript.player = GameObject.FindGameObjectWithTag("Player");
        _cameraTransitionScript.externalVariables = externalVariables;
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
