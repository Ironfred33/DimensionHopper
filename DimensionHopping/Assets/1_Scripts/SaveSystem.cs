using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveLevel(string saveString)
    {
        File.WriteAllText(Application.dataPath + "/save.txt", saveString);
        
    }

    public static string LoadSaveData()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            return saveString;
        }

        else
        {
            return null;
        }

    }
}
