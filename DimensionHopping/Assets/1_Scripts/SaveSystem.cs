using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveLevel(string saveString)
    {
        File.WriteAllText(Application.persistentDataPath + "/save.txt", saveString);
        
    }

    public static string LoadSaveData()
    {
        if(File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.persistentDataPath + "/save.txt");
            return saveString;
        }

        else
        {
            return null;
        }

    }
}
