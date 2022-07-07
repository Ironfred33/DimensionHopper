using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lädt nächste Szene, sobald das Ziel erreicht wird
public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canvas.GetComponent<UIManager>().state = UIState.LevelCompleted;

        }
    }
}
