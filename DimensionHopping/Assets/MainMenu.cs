﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert das Hauptmenü
public class MainMenu : MonoBehaviour
{
    public SceneLoader sceneLoad;
    public void StartGame()
    {
        Cursor.visible = true;
        sceneLoad.LoadNextScene();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
