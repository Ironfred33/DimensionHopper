using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tastenkürzel für Befehle
public class SpecialKeys : MonoBehaviour
{
    // Start is called before the first frame update

    private bool _gamePaused;

    public KeyCode pauseGameKey;

    public KeyCode restartKey;

    public KeyCode loadNextSceneKey;

    public KeyCode loadLastSceneKey;

    private SceneLoader _sceneLoaderScript;

    void Start()
    {
        _sceneLoaderScript = GetComponent<SceneLoader>();
    }

    

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(pauseGameKey) && !_gamePaused)
        {
            PauseGame();
        }

        else if(Input.GetKeyDown(pauseGameKey) && _gamePaused)
        {
            ResumeGame();
        }

        if(Input.GetKeyDown(restartKey))
        {
            ReloadScene();
        }

        if(Input.GetKeyDown(loadNextSceneKey))
        {
            LoadNextScene();
        }

        if(Input.GetKeyDown(loadLastSceneKey))
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
    }

    // Setzt das Spiel fort
    void ResumeGame()
    {
        Time.timeScale = 1;
        _gamePaused = false;
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
