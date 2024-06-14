using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
public class SoundLibrary : MonoBehaviour
{
    [SerializeField] public List<SoundData> listAudio;

    public static SoundLibrary Instance;
    private AudioSource _audioSource;

    private void Start()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundName)
    {
        var first = listAudio.FirstOrDefault(soundData => soundData.name == soundName);

        if (first != null)
        {
            _audioSource.PlayOneShot(first.sound);
        }
    }
}
