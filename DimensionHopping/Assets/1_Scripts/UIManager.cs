using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        mainGameScreen = this.transform.Find("MainGame").gameObject;
        gameOverScreen = this.transform.Find("GameOver").gameObject;
        levelCompletedScreen = this.transform.Find("LevelCompleted").gameObject;
        levelGeneratorScreen = this.transform.Find("LevelGenerator").gameObject;
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
        sceneLoad.ReLoadCurrentScene();
        state = UIState.MainGame;
        Debug.Log("Reload Scene");
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

}
