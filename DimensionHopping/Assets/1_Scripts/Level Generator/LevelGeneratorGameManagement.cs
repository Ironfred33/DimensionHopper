using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LevelGeneratorGameManagement : MonoBehaviour
{

    private LevelGeneratorSceneManagement _sceneManagementScript;
    private GenerateLevel _levelGenerationScript;
    private CameraController _camControlScript;
    private CameraTransition _camTransitionScript;
    private PlayerController _playerControlScript;
    private EVPlayer _extVarsPlayer;
    private PlayerHealth _playerHealthScript;

    private GameObject _buttons;
    private GameObject _player;
    private GameObject _playerPrefab;
    public KeyCode restartButton;
    public KeyCode pauseButton;
    public KeyCode playerRespawn;
    public Vector3 spawnCoordinates;

    private bool _paused;

    void Awake()
    {
        InitialSetup();

    }


    void Update()
    {
        GetInput();
    }

    void InitialSetup()
    {

        _sceneManagementScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<LevelGeneratorSceneManagement>();
        _levelGenerationScript = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<GenerateLevel>();
        _extVarsPlayer = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVPlayer>();

        _player = Resources.Load("Prefabs/NewCharacter") as GameObject;
        spawnCoordinates = new Vector3(1, 0, 0);

    }

    void GetInput()
    {
        if(Input.GetKeyDown(restartButton))
        {
            Reset();
        }

        if(Input.GetKeyDown(pauseButton) && !_paused)
        {
            Pause();
            _paused = true;
        }

        if(Input.GetKeyDown(pauseButton) && _paused)
        {
            UnPause();
            _paused = false;
        }

        if(Input.GetKeyDown(playerRespawn))
        {
            RespawnPlayer();
        }
        
    }



    public void ButtonGenerate()
    {

        if (!_levelGenerationScript.levelGenerated) _levelGenerationScript.Generate();


    }

    public void ButtonStartGame()
    {

        
        if(_levelGenerationScript.levelGenerated) 
        {
            _buttons.SetActive(false);
            SpawnPlayer();
        }
        else if(!_levelGenerationScript.levelGenerated) Debug.Log("You need to generate a Level first!");

        
       
    }

    public void Reset()
    {
        _sceneManagementScript.ReloadScene();
        _levelGenerationScript.ResetAllRelevantVariables();
        _buttons.SetActive(true);
    }

    public void SpawnPlayer()
    {

        _playerPrefab = Instantiate(_player, spawnCoordinates, Quaternion.identity);

        _playerPrefab.transform.Rotate(new Vector3(0, 90, 0), Space.Self);

        AssignComponents();

    }

    void AssignComponents()
    {

        _camControlScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        _camTransitionScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTransition>();
        _playerControlScript = _playerPrefab.GetComponent<PlayerController>();

        _camControlScript.trackingTarget = _playerPrefab.transform;
        _camControlScript.player = _playerPrefab;

        _camTransitionScript.player = _playerPrefab;
        _camTransitionScript.playerControl = _playerControlScript;

        _playerControlScript.cameraControl = _camControlScript;
        _playerControlScript.cameraTransition = _camTransitionScript;

        _playerControlScript.extVars = _extVarsPlayer;
        _camControlScript.fpPosition = GameObject.FindGameObjectWithTag("PlayerTrackingPoint");

        _playerHealthScript = _playerPrefab.GetComponent<PlayerHealth>();
        _playerHealthScript.externalPlayer = _extVarsPlayer;

        for (int i = 0; i < 3; i++)
        {
            _playerHealthScript.hearts[i] = GameObject.FindGameObjectWithTag("Canvas").transform.Find("MainGame").GetChild(i).GetComponent<Image>();
        }

        
    }

    void  Pause()
    {
        Time.timeScale = 0;
    }

    void UnPause()
    {
        Time.timeScale = 1;
    }

    private void RespawnPlayer()
    {
        _playerPrefab.transform.position = spawnCoordinates;
    }

}
