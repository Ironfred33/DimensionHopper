using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject _mainUI;
    [SerializeField] private SceneLoader _sceneLoad;


    public void RetryLevel()
    {
        this.gameObject.SetActive(false);
       _mainUI.SetActive(true);
    }

    public void BackToMenu()
    {
        _sceneLoad.LoadStartScene();
    }
}
