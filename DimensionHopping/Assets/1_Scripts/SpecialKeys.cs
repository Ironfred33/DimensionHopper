using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tastenkürzel für Befehle
public class SpecialKeys : MonoBehaviour
{
    public KeyCode pauseGameKey;
    public KeyCode restartKey;
    public KeyCode loadNextSceneKey;
    public KeyCode loadLastSceneKey;
    private SceneLoader _sceneLoaderScript;
    [SerializeField] private UIManager _uiManager;

    private bool _gamePaused;

    void Start()
    {
        _sceneLoaderScript = GetComponent<SceneLoader>();
    }


    void Update()
    {

        if (Input.GetKeyDown(pauseGameKey) && !_gamePaused)
        {
            PauseGame();
        }

        else if (Input.GetKeyDown(pauseGameKey) && _gamePaused)
        {
            ResumeGame();
        }

        if (Input.GetKeyDown(restartKey))
        {
            ReloadScene();
        }

        if (Input.GetKeyDown(loadNextSceneKey))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(loadLastSceneKey))
        {
            LoadLastScene();
        }

    }

    // Lädt derzeit laufende Szene neu
    void ReloadScene()
    {
        _sceneLoaderScript.ReLoadCurrentScene();
    }


    // Pausiert das Spiel
    void PauseGame()
    {
        Time.timeScale = 0;
        _gamePaused = true;
        _uiManager.state = UIState.Pause;
    }

    // Setzt das Spiel fort
    void ResumeGame()
    {
        Time.timeScale = 1;
        _gamePaused = false;
        _uiManager.state = UIState.MainGame;
    }

    // Lädt die nächste Szene
    void LoadNextScene()
    {
        _sceneLoaderScript.LoadNextScene();
    }

    // Lädt die letzte Szene
    void LoadLastScene()
    {
        _sceneLoaderScript.LoadLastScene();
    }

}
