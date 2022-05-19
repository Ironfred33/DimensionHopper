using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lädt nächste Szene, sobald das Ziel erreicht wird
public class Goal : MonoBehaviour
{
    public GameObject mainUI;

    public GameObject levelCompleted;

    public GameObject stateBar;

    public GameObject crossHair;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainUI.SetActive(false);
            levelCompleted.SetActive(true);
            stateBar.SetActive(false);
            crossHair.SetActive(false);

            Cursor.visible = true;
        }
    }
}
