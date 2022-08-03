using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Lädt nächste Szene, sobald das Ziel erreicht wird
public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private SaveData _saveData;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canvas.GetComponent<UIManager>().state = UIState.LevelCompleted;
            if(_saveData.unlockedLevels < SceneManager.GetActiveScene().buildIndex - 4)
            {
                _saveData.unlockedLevels += 1;
            }
            _saveData.Save();
            
        }
    }

    
}
