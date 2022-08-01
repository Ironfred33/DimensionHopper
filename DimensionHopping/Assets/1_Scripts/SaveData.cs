using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : MonoBehaviour
{
    public int unlockedLevels;
    public int[] collectibles;
    
    public SaveData(LevelSelectionData levelSelectionData)
    {
        unlockedLevels = levelSelectionData.unlockedLevels;
        collectibles = levelSelectionData.collectibles;

    } 
}
