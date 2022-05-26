using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Wird genutzt, um Szenen zu laden
public class SceneLoader : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Lädt die nächste Szene
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // Lädt die letzte Szene
    public void LoadLastScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }
    
    // Lädt erste Szene (Hauptmenü)
    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevelGeneratorScene()
    {
        SceneManager.LoadScene("LevelGenerator");
    }

    // Lädt den Endscreen
    public void LoadEndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    // Lädt die derzeit laufende Szene neu
    public void ReLoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }


}
