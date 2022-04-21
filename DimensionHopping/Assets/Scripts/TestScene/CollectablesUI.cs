using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectablesUI : MonoBehaviour
{

    public Image[] disks;

    public Sprite collectedDisk;

    public Sprite uncollectedDisk;

    public List<int> _alreadyCollected;

    void Start() 
    {
        _alreadyCollected = new List<int>();
    }
    public void FoundCollectable(int collectNumber)
    {
        _alreadyCollected.Add(collectNumber);
        if(_alreadyCollected.Contains(collectNumber))
            {
                disks[collectNumber-1].sprite = collectedDisk;
            }
    }

}
