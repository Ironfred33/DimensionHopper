using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Level : MonoBehaviour
{
    [SerializeField] private int _levelOrder;
    [SerializeField] private LevelSelectionData _levelSelectionData;

    void Update()
    {
        
    }

    public void StartLevel()
    {
        if(_levelOrder<= _levelSelectionData.unlockedLevels)
        {
            SceneManager.LoadScene(_levelOrder + 2);
        }
    }
}
