using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
public class SoundLibrary : MonoBehaviour
{
    [SerializeField] public List<AudioClip> listAudio;

    public static SoundLibrary Instance;
    private AudioSource _audioSource;

    private void Start()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.5f;
    }

    public void PlaySound(string soundName)
    {
        var first = listAudio.FirstOrDefault(soundData => soundData.name == soundName);

        if (first != null)
        {
            _audioSource.pitch = 1;
            _audioSource.PlayOneShot(first);
        }
    }

    public bool Emilien()
    {
        return _audioSource.isPlaying;
    }

    public void Run(string soundName)
    {
        _audioSource.pitch = 2;
        var first = listAudio.FirstOrDefault(soundData => soundData.name == soundName);
        _audioSource.PlayOneShot(first);
    }

    public void Stop()
    {
        _audioSource.Stop();
    }
    
}
