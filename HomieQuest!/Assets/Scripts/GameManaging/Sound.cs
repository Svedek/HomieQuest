using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private string name;

    [Range(0f, 1f)]
    [SerializeField] float volume;
    [Range(.1f, 3f)]
    [SerializeField] private float pitch;
    [SerializeField] private bool loop;

    private AudioSource source;

    public string GetName() { return name; }

    public void Setup(AudioSource src)
    {
        source = src;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }

    public void Play()
    {
        source.Play();
    }
}
