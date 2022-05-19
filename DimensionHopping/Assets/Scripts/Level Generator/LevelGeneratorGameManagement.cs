using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorGameManagement : MonoBehaviour
{

    private LevelGeneratorSceneManagement _sceneManagementScript;

    private GenerateLevel _levelGenerationScript;

    [SerializeField] private GameObject _canvas;

    [SerializeField] private bool _sceneReloaded;

    public KeyCode restartButton;

    public KeyCode pauseButton;

    public KeyCode playerRespawn;

    public Vector3 spawnCoordinates;

    private CameraController _camControlScript;

    private CameraTransition _camTransitionScript;

    private PlayerController _playerControlScript;
    private GameObject _player;

    private GameObject _playerPrefab;

    private bool _paused;

    //private bool _playerSpawned;



    void Awake()
    {
        InitialSetup();

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void InitialSetup()
    {

        _sceneManagementScript = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<LevelGeneratorSceneManagement>();
        _levelGenerationScript = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<GenerateLevel>();

        _player = Resources.Load("Player") as GameObject;
        spawnCoordinates = new Vector3(1, 2, 0);

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

        //StartCoroutine(_levelGenerationScript.CountGenerationTime());

        if (!_levelGenerationScript.levelGenerated) _levelGenerationScript.Generate();


    }

    public void ButtonStartGame()
    {

        

        if(_levelGenerationScript.levelGenerated) 
        {
            _canvas.SetActive(false);
            SpawnPlayer();
        }
        else if(!_levelGenerationScript.levelGenerated) Debug.Log("You need to generate a Level first!");

        
       
    }

    public void Reset()
    {
        _sceneManagementScript.ReloadScene();
        _levelGenerationScript.ResetAllRelevantVariables();
        _canvas.SetActive(true);
        //_playerSpawned = false;
    }

    void SpawnPlayer()
    {
        //_playerSpawned = true;

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
        _camControlScript.fpPosition = _playerPrefab.transform.Find("FPPPoint").gameObject;
        _camControlScript.player = _playerPrefab;

        _camTransitionScript.player = _playerPrefab;
        _camTransitionScript.playerControl = _playerControlScript;

        _playerControlScript.cameraControl = _camControlScript;
        _playerControlScript.cameraTransition = _camTransitionScript;

        
    }



    

    IEnumerator WaitTime(float time)
    {

        yield return new WaitForSeconds(time);
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
