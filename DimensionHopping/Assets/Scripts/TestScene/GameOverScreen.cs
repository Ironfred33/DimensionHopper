using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public GameObject mainUI;

    public GameObject gameOverScreen;

    public SceneLoader sceneLoad;


    public void RetryLevel()
    {
        this.gameObject.SetActive(false);
        mainUI.SetActive(true);
    }

    public void BackToMenu()
    {
        sceneLoad.LoadStartScene();
    }
}
