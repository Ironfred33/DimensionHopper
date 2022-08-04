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

    [SerializeField] private Image _previewImage;
    [SerializeField] private float _unlockedAlpha;
    [SerializeField] private float _lockedAlpha;


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

        SwitchPreviewAlpha();

    }

    public void StartLevel()
    {
        if(_levelOrder<= _saveData.unlockedLevels)
        {
            SceneManager.LoadScene(_levelOrder + 2);
        }
    }

    private void SwitchPreviewAlpha()
    {
        var previewColors = _previewImage.color;
        
        if(_levelOrder<= _saveData.unlockedLevels)
        {
            previewColors.a = _unlockedAlpha;
        }
        else
        {
            previewColors.a = _lockedAlpha;
        }

        _previewImage.color = previewColors;
    }
}
