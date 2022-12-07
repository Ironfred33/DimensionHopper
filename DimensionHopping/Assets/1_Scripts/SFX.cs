using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Steuert das Abspielen von Soundeffekten 
public class SFX : MonoBehaviour
{
    private AudioSource _source;
    [SerializeField] private AudioClip[] _effects;
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    /*void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayJumpSound();
        }
    }
    */

    // Spielt Game Over Sound ab
    public void PlayGameOverSound()
    {
        _source.clip = _effects[0];
        _source.Play();
    }

    // Spielt Sound beim Springen
    public void PlayJumpSound()
    {
        _source.clip = _effects[1];
        _source.Play();
    }

    public void PlayLandingSound()
    {
        _source.clip = _effects[2];
        _source.Play();
    }

    // Spielt Sound, wenn Spieler getroffen wird
    public void PlayDamageSound()
    {
        _source.clip = _effects[3];
        _source.Play();
    }

    // Spielt Sound, wenn die Perspektive gewechselt wird
    public void PlayPerspectiveSwitchSound()
    {
        _source.clip = _effects[4];
        _source.Play();
    }
}
