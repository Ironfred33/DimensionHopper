using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Updated das UI beim Aufheben von Collectibles
public class CollectablesUI : MonoBehaviour
{

    public Image[] disks;

    public Sprite collectedDisk;

    public Sprite uncollectedDisk;

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
                disks[collectNumber-1].sprite = collectedDisk;
            }
    }

}
