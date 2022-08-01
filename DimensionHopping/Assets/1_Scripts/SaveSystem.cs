using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveLevel(LevelSelectionData levelSelectionData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.saveData";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            SaveData data = new SaveData(levelSelectionData);
            formatter.Serialize(stream, data);
        
        }

        
    }

    public static SaveData LoadSaveData()
    {
        string path = Application.persistentDataPath + "/player.saveData";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using(FileStream stream = new FileStream(path, FileMode.Open))
            {
                SaveData data = formatter.Deserialize(stream) as SaveData;
                return data;
            }

        }

        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }
}
