using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    [SerializeField] private CollectablesUI collectablesUI;

    [SerializeField] private GameObject saveObject;
    public int unlockedLevels;
    public int[] collectibles;
    
    private void Awake() 
    {
        Load();    

        if(SceneManager.GetActiveScene().name != "LevelSelection")
        {
            collectibles = new int[SceneManager.sceneCountInBuildSettings];
        }
        
    }
    public void Save()
    {
        collectibles[SceneManager.GetActiveScene().buildIndex - 4] = collectablesUI.alreadyCollected.Count;

        SaveProgress saveProgress = new SaveProgress
        {
            unlockedLevels = unlockedLevels,
            collectibles = collectibles
        };

        string json = JsonUtility.ToJson(saveProgress);
        SaveSystem.SaveLevel(json);

    }

    public void Load()
    {
        string saveString = SaveSystem.LoadSaveData();
        if(saveString != null)
        {
            SaveProgress saveProgress = JsonUtility.FromJson<SaveProgress>(saveString);

            unlockedLevels = saveProgress.unlockedLevels;
            collectibles = saveProgress.collectibles;
        }
    }

    private class SaveProgress
    {
        public int unlockedLevels;
        public int[] collectibles;
    }
}
