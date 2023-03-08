using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Updated das UI beim Aufheben von Collectibles
public class CollectablesUI : MonoBehaviour
{

    [SerializeField] private Image[] _disks;
    [SerializeField] private Image[] _completedDisks;
    [SerializeField] private Sprite _collectedDisk;
 
    public List<int> alreadyCollected;

    void Start() 
    {
        alreadyCollected = new List<int>();
    }

    // Fügt neu gefundenes Collectible der Liste der bereits gefundenen Collectibles hinzu und aktualisiert das UI entsprechend. 
    public void FoundCollectable(int collectNumber)
    {
        alreadyCollected.Add(collectNumber);
        if(alreadyCollected.Contains(collectNumber))
            {
                _disks[collectNumber-1].sprite = _collectedDisk;
                _completedDisks[collectNumber-1].sprite = _collectedDisk;
            }
    }

}
