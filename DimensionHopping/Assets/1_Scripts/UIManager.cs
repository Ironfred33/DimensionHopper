using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIState
{
        MainGame,
        GameOver,
        LevelCompleted,
        LevelGenerator,
        Pause
}

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _mainGameScreen;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _levelCompletedScreen;
    [SerializeField] private GameObject _levelGeneratorScreen;
    [SerializeField] private GameObject _pauseScreen;
    public UIState state;
    [SerializeField] private GameObject _manager;
    [SerializeField] private SceneLoader sceneLoad;
    [SerializeField] private GeneratorOptions _generatorOptions;
    private GenerateLevel _levelGenerationScript;
    private LevelGeneratorGameManagement _levelGenerationManagement;

    void Start()
    {
        _mainGameScreen = this.transform.Find("MainGame").gameObject;
        _gameOverScreen = this.transform.Find("GameOver").gameObject;
        _levelCompletedScreen = this.transform.Find("LevelCompleted").gameObject;
        _levelGeneratorScreen = this.transform.Find("LevelGenerator").gameObject;
        _pauseScreen = this.transform.Find("Pause").gameObject;
        sceneLoad = _manager.GetComponent<SceneLoader>();

        if(SceneManager.GetActiveScene().name == "LevelGenerator")
        {
            state = UIState.LevelGenerator;
            _levelGenerationScript =  GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<GenerateLevel>();
            _levelGenerationManagement =  GameObject.FindGameObjectWithTag("LevelGenerationManager").GetComponent<LevelGeneratorGameManagement>();
        }

        else
        {
            state = UIState.MainGame;
        }
    }

    void Update()
    {
       // Debug.Log("current state: " + state);
        EvaluateState();
    }

    public void EvaluateState()
    {
        switch (state)
        {
            case UIState.MainGame:
                _gameOverScreen.SetActive(false);
                _levelCompletedScreen.SetActive(false);
                _levelGeneratorScreen.SetActive(false);
                _mainGameScreen.SetActive(true);
                _pauseScreen.SetActive(false);
                Cursor.visible = false;
                break;
            
            case UIState.GameOver:
                _mainGameScreen.SetActive(false);
                _gameOverScreen.SetActive(true);
                Debug.Log("Activated GO screen");
                Cursor.visible = true;
                break;
            
            case UIState.LevelCompleted:
                _mainGameScreen.SetActive(false);
                _levelCompletedScreen.SetActive(true);
                Cursor.visible = true;
                break;

            case UIState.LevelGenerator:
                _mainGameScreen.SetActive(false);
                _levelGeneratorScreen.SetActive(true);
                Cursor.visible = true;
                break;
            case UIState.Pause:
                _mainGameScreen.SetActive(true);
                _levelGeneratorScreen.SetActive(false);
                _pauseScreen.SetActive(true);
                Cursor.visible = true;
                break;
        }

    }

    public void RepeatLevel()
    {
        if(SceneManager.GetActiveScene().name == "LevelGenerator")
        {
            state = UIState.MainGame;
            Debug.Log("Reload Scene");
        }
        else
        {
            state = UIState.MainGame;
            Debug.Log("Reload Scene");
            sceneLoad.ReLoadCurrentScene();
        }
    }

    public void BackToMenu()
    {
        sceneLoad.LoadStartScene();
        state = UIState.MainGame;
        Debug.Log("BackToMenu");
    }

    public void NextLevel()
    {
        sceneLoad.LoadNextScene();
        state = UIState.MainGame;
        Debug.Log("Reload Scene");
    }
    public void ResetState()
    {
        Debug.Log("Changed state");
        this.state = UIState.MainGame;
        
    }

    public void ButtonGenerate()
    {

        if (!_levelGenerationScript.levelGenerated) 
        
        {
            _generatorOptions.OverWriteGenerationOptions();
            _levelGenerationScript.Setup();
            _levelGenerationScript.Generate();

        }


        StartGame();


    }

    public void StartGame()
    {

        
        if(_levelGenerationScript.levelGenerated) 
        {
            state = UIState.MainGame;
            _levelGenerationManagement.SpawnPlayer();
        }
        else if(!_levelGenerationScript.levelGenerated) Debug.Log("You need to generate a Level first!");

       
    }

    public void Reset()
    {
        sceneLoad.ReLoadCurrentScene();
        _levelGenerationScript.ResetAllRelevantVariables();
        state = UIState.LevelGenerator;
    }

}
