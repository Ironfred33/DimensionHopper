using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LevelGeneratorGameManagement : MonoBehaviour
{

    private LevelGeneratorSceneManagement _sceneManagementScript;

    private GenerateLevel _levelGenerationScript;

    [SerializeField] private GameObject _buttons;

    [SerializeField] private bool _sceneReloaded;

    public KeyCode restartButton;

    public KeyCode pauseButton;

    public KeyCode playerRespawn;

    public Vector3 spawnCoordinates;

    [SerializeField] private CameraController _camControlScript;

    [SerializeField] private CameraTransition _camTransitionScript;

    [SerializeField] private PlayerController _playerControlScript;
    private GameObject _player;

    [SerializeField] private GameObject _playerPrefab;

    [SerializeField] private EVPlayer _extVarsPlayer;

    private PlayerHealth _playerHealthScript;

    

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
        _extVarsPlayer = GameObject.FindGameObjectWithTag("ExternalVariables").GetComponent<EVPlayer>();

        _player = Resources.Load("Prefabs/character_v2") as GameObject;
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

        Debug.Log("ASSIGNCOMPONENTS TRIGGERED");
        _camControlScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        _camTransitionScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraTransition>();
        _playerControlScript = _playerPrefab.GetComponent<PlayerController>();

        _camControlScript.trackingTarget = _playerPrefab.transform;
        //_camControlScript.fpPosition = _playerPrefab.transform.Find("FPPPoint").gameObject;
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
