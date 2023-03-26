using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

// Regelt die Optionen in den Einstellungen
public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMix;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _effectSample;
    [SerializeField] private AudioClip _musicSample;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    private Resolution[] resolutions;
    
    private bool _fullscreen;
    
    void Start()
    {
        _fullscreen = true;
        Screen.fullScreen = _fullscreen;
        
        resolutions = Screen.resolutions;

        _resolutionDropdown.ClearOptions();

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

        _resolutionDropdown.AddOptions(resOptions);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }


    // Anpassung der Musik-Lautstärke
    public void SetMusicVolume(float musicVolume)
    {
        _audioMix.SetFloat("MusicVolume", musicVolume);
    }

    // Anpassung der Effekt-Lautstärke
    public void SetEffectVolume(float effectsVolume)
    {
        _audioMix.SetFloat("SFXVolume", effectsVolume);
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
        _source.outputAudioMixerGroup = _audioMix.FindMatchingGroups("Music")[0];
        _source.clip = _musicSample;
        _source.Play();
    }

    // Test der Effekt-Lautstärke
    public void TestEffectVolume()
    {
        _source.outputAudioMixerGroup = _audioMix.FindMatchingGroups("SFX")[0];
        _source.clip = _effectSample;
        _source.Play();
    }

}
