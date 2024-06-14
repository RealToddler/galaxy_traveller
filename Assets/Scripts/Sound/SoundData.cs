using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Sound/New sound")]
public class SoundData : ScriptableObject
{
    public new string name;
    public AudioClip sound;
}

