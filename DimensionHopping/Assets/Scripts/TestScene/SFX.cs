using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    private AudioSource _source;
    public AudioClip[] effects;
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    void PlayGameOverSound()
    {
        _source.clip = effects[0];
        _source.Play();
    }

    void PlayJumpSound()
    {
        _source.clip = effects[1];
        _source.Play();
    }

    void PlayDamageSound()
    {
        _source.clip = effects[2];
        _source.Play();
    }

    void PlayPerspectiveSwitchSound()
    {
        _source.clip = effects[3];
        _source.Play();
    }
}
