using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveLevel(string saveString)
    {
        File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/save.txt", saveString);
        
    }

    public static string LoadSaveData()
    {
        if(File.Exists(System.IO.Directory.GetCurrentDirectory() + "/save.txt"))
        {
            string saveString = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/save.txt");
            return saveString;
        }

        else
        {
            return null;
        }

    }
}
