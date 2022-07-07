using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoad;

    public void NextLevel()
    {
        _sceneLoad.LoadNextScene();
    }

    public void RepeatLevel()
    {
        _sceneLoad.ReLoadCurrentScene();
    }
}
