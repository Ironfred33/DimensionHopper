using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lädt nächste Szene, sobald das Ziel erreicht wird
public class Goal : MonoBehaviour
{
    public GameObject canvas;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.GetComponent<UIManager>().state = UIState.LevelCompleted;

        }
    }
}
