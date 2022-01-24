using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialKeys : MonoBehaviour
{
    // Start is called before the first frame update

    private bool _gamePaused;

    public KeyCode pauseGame;

    public KeyCode restart;
    void Start()
    {
        
    }

    

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(pauseGame) && !_gamePaused)
        {
            PauseGame();
        }

        else if(Input.GetKeyDown(pauseGame) && _gamePaused)
        {
            ResumeGame();
        }
        
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
