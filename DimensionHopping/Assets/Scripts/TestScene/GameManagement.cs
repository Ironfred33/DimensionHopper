using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Greift Skripts auf und weist sie den entsprechenden Game Objekten zu
public class GameManagement : MonoBehaviour
{

    public Vector3 SpawnCoords;
    private GameObject _externalVariables;
    private GameObject _playerPrefab;
    private GameObject _player;
    private EVPlayer _externalVariablesPlayerScript;
    private PlayerHealth _playerHealthScript;
    private PlayerController _playerControllerScript;
    private CameraTransition _cameraTransitionScript;
    private CameraController _cameraControlScript;
    private DronePosition _dronePositionScript;
    private Transform _trackingTarget;
    private GameObject _trackingPoint;
    private GameObject _canvas;
    private GameObject _drone;
    private SceneLoader _sceneLoaderScript;
    private LevelCompleted _levelCompleted;

    private Goal _goalScript;

    private List<GameObject> _heartImages = new List<GameObject>();

    void Awake()
    {

        Cursor.visible = false;

        LoadPrefab();

        SpawnPlayer();

        GetAllComponents();

        AssignComponentsToPlayer();

        AssignComponentsToCamera();

        AssignRemainingComponents();

    }

    // Spawnt Spieler an einer vordefinierten Position
    public void SpawnPlayer()
    {

        Instantiate(_playerPrefab, SpawnCoords, Quaternion.Euler(0, 90, 0));

        //GetAllComponents();

        //AssignComponentsToPlayer();

    }

    // 

    // Lädt Spieler Prefab
    void LoadPrefab()
    {
        _playerPrefab = Resources.Load("Prefabs/character_v2") as GameObject;

    }


    

    // Weist Spieler-Gameobjekt Komponenten zu
    void GetAllComponents()
    {

        // Player

        _player = GameObject.FindGameObjectWithTag("Player");


        // Player Scripts

        _playerHealthScript = _player.GetComponent<PlayerHealth>();

        _playerControllerScript = _player.GetComponent<PlayerController>();


        // External Variables und Script

        _externalVariables = GameObject.FindGameObjectWithTag("ExternalVariables");

        _externalVariablesPlayerScript = _externalVariables.GetComponent<EVPlayer>();


        // Camera Scripts

        _cameraControlScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        _cameraTransitionScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTransition>();


        // Canvas

        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        _levelCompleted = GameObject.FindGameObjectWithTag("Canvas").GetComponent<LevelCompleted>();


        // Drohne

        _drone = GameObject.FindGameObjectWithTag("Drone");

        _dronePositionScript = _drone.GetComponent<DronePosition>();

        // Ziel

        _goalScript = GameObject.FindGameObjectWithTag("Finish").GetComponent<Goal>();

        // Sceneloader

        _sceneLoaderScript = this.GetComponent<SceneLoader>();


    }

    // Greift externe Skripts auf
    void AssignComponentsToPlayer()
    {
        // Player Health Script

        _playerHealthScript.externalPlayer = _externalVariablesPlayerScript;


        // Herz-Images des Canvas hinzufügen
        // Falls es jemals mehr Herzen geben sollte, unten optimieren. Ist noch mit maximal 3 Children gehardcoded

        for (int i = 0; i < 3; i++)
        {
            _playerHealthScript.hearts[i] = _canvas.transform.Find("MainGame").GetChild(i).GetComponent<Image>();
        }


        _playerControllerScript.extVars = _externalVariablesPlayerScript;

        _playerControllerScript.cameraControl = _cameraControlScript;

        _playerControllerScript.cameraTransition = _cameraTransitionScript;

    }

    void AssignComponentsToCamera()
    {

        _cameraControlScript.externalVariables = _externalVariables;

        _cameraControlScript.player = _player;

        _cameraTransitionScript.externalVariables = _externalVariables;

        _cameraTransitionScript.playerControl = _playerControllerScript;

        _cameraTransitionScript.player = _player;

        _trackingTarget = _player.transform;

        _cameraControlScript.trackingTarget = _trackingTarget;

        _trackingPoint = GameObject.FindGameObjectWithTag("PlayerTrackingPoint");

        _cameraControlScript.fpPosition = _trackingPoint;

    }


    void AssignRemainingComponents()
    {
        _dronePositionScript.cameraEV = _externalVariables.GetComponent<EVCamera>();
        _dronePositionScript.camControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        _levelCompleted.sceneLoad = _sceneLoaderScript;

    }











}
