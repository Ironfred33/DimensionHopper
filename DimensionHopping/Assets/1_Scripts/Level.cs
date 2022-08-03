using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Level : MonoBehaviour
{
    [SerializeField] private int _levelOrder;
    [SerializeField] private SaveData _saveData;
    [SerializeField] private Image[] _collectedDisks;
    [SerializeField] private Sprite _fullDisk;
    [SerializeField] private Sprite _emptyDisk;

    void Start()
    {
        
        
        for(int i = 0; i<_collectedDisks.Length; i++)
        {
            if(i < _saveData.collectibles[_levelOrder])
            {
                _collectedDisks[i].sprite = _fullDisk;
            }

            else
            {
                _collectedDisks[i].sprite = _emptyDisk;
            }

        }
    }

    public void StartLevel()
    {
        if(_levelOrder<= _saveData.unlockedLevels)
        {
            SceneManager.LoadScene(_levelOrder + 2);
        }
    }
}
