using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialKeys : MonoBehaviour
{
    // Start is called before the first frame update

    private bool _gamePaused;

    public KeyCode pauseGameKey;

    public KeyCode restartKey;

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
        
    }


    void ReloadScene()
    {   
        _sceneLoaderScript.ReLoadCurrentScene();
    }



    void PauseGame()
    {
        Time.timeScale = 0;
        _gamePaused = true;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        _gamePaused = false;
    }

}
