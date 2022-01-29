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

    /*void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayJumpSound();
        }
    }
    */

    public void PlayGameOverSound()
    {
        _source.clip = effects[0];
        _source.Play();
    }

    public void PlayJumpSound()
    {
        _source.clip = effects[1];
        _source.Play();
    }

    public void PlayDamageSound()
    {
        _source.clip = effects[2];
        _source.Play();
    }

    public void PlayPerspectiveSwitchSound()
    {
        _source.clip = effects[3];
        _source.Play();
    }
}
