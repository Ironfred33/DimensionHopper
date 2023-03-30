using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert das Hauptmenü
public class MainMenu : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoad;

    private void Start() 
    {
        _sceneLoad = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneLoader>();
        Cursor.visible = true;
    }
    public void StartGame()
    {
        
        Cursor.visible = true;
        _sceneLoad.LoadNextScene();
    }

    public void LevelGenerator()
    {
        _sceneLoad.LoadLevelGeneratorScene();
    }


    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
