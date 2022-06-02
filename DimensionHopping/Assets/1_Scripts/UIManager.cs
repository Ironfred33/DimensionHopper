using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIState
{
        MainGame,
        GameOver,
        LevelCompleted,
        LevelGenerator
}

public class UIManager : MonoBehaviour
{
    
    public GameObject mainGameScreen;
    public GameObject gameOverScreen;
    public GameObject levelCompletedScreen;
    public GameObject levelGeneratorScreen;
    public UIState state;
    public SceneLoader sceneLoad;
    private GenerateLevel _levelGenerationScript;
    private LevelGeneratorGameManagement _levelGenerationManagement;

    void Start()
    {
        mainGameScreen = this.transform.Find("MainGame").gameObject;
        gameOverScreen = this.transform.Find("GameOver").gameObject;
        levelCompletedScreen = this.transform.Find("LevelCompleted").gameObject;
        levelGeneratorScreen = this.transform.Find("LevelGenerator").gameObject;

        _levelGenerationScript =  GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<GenerateLevel>();
        _levelGenerationManagement =  GameObject.FindGameObjectWithTag("LevelGenerationManager").GetComponent<LevelGeneratorGameManagement>();

        if(SceneManager.GetActiveScene().name == "LevelGeneratorFred")
        {
            state = UIState.LevelGenerator;
        }

        else
        {
            state = UIState.MainGame;
        }
    }

    void Update()
    {
        Debug.Log("current state: " + state);
        EvaluateState();
    }

    public void EvaluateState()
    {
        switch (state)
        {
            case UIState.MainGame:
                gameOverScreen.SetActive(false);
                levelCompletedScreen.SetActive(false);
                levelGeneratorScreen.SetActive(false);
                mainGameScreen.SetActive(true);
                Cursor.visible = false;
                break;
            
            case UIState.GameOver:
                mainGameScreen.SetActive(false);
                gameOverScreen.SetActive(true);
                Debug.Log("Activated GO screen");
                Cursor.visible = true;
                break;
            
            case UIState.LevelCompleted:
                mainGameScreen.SetActive(false);
                levelCompletedScreen.SetActive(true);
                Cursor.visible = true;
                break;

            case UIState.LevelGenerator:
                mainGameScreen.SetActive(false);
                levelGeneratorScreen.SetActive(true);
                Cursor.visible = true;
                break;
        }

    }

    public void RepeatLevel()
    {
        if(SceneManager.GetActiveScene().name == "LevelGeneratorFred")
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

    public void ButtonGenerate()
    {

        if (!_levelGenerationScript.levelGenerated) _levelGenerationScript.Generate();


    }

    public void ButtonStartGame()
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
