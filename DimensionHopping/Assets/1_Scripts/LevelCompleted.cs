using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleted : MonoBehaviour
{
    public SceneLoader sceneLoad;

    public void NextLevel()
    {
        sceneLoad.LoadNextScene();
    }

    public void RepeatLevel()
    {
        sceneLoad.ReLoadCurrentScene();
    }
}
