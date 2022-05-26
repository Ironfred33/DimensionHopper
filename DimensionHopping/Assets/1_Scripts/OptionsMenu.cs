using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

// Regelt die Optionen in den Einstellungen
public class OptionsMenu : MonoBehaviour
{
    public Slider effectSlider;
    public Slider musicSlider;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public AudioMixer audioMix;
    public AudioSource source;
    public AudioClip effectSample;
    public AudioClip musicSample;

    private bool _fullscreen;
    void Start()
    {
        _fullscreen = true;
        Screen.fullScreen = _fullscreen;
        
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;

        List<string> resOptions = new List<string>();

        for(int i = 0; i < resolutions.Length; i++)
        {
            string resOption = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(resOption);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            } 
        }

        resolutionDropdown.AddOptions(resOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    // Anpassung der Musik-Lautstärke
    public void SetMusicVolume(float musicVolume)
    {
        audioMix.SetFloat("MusicVolume", musicVolume);
    }

    // Anpassung der Effekt-Lautstärke
    public void SetEffectVolume(float effectsVolume)
    {
        audioMix.SetFloat("SFXVolume", effectsVolume);
    }

    // Toggle Vollbild-Modus
    public void SetFullscreen()
    {
        if(_fullscreen)
        {
            _fullscreen = false;
        }
        else
        {
            _fullscreen = true;
        }
        Screen.fullScreen = _fullscreen;
    }

    // Anpassung der Auflösung
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Test der Musik-Lautstärke
    public void TestMusicVolume()
    {
        source.outputAudioMixerGroup = audioMix.FindMatchingGroups("Music")[0];
        source.clip = musicSample;
        source.Play();
    }

    // Test der Effekt-Lautstärke
    public void TestEffectVolume()
    {
        source.outputAudioMixerGroup = audioMix.FindMatchingGroups("SFX")[0];
        source.clip = effectSample;
        source.Play();
    }

}
