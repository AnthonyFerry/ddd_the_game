using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSettings", menuName = "Audio Manager/Sound Settings", order = 2)]
public class SoundSettings : ScriptableObject {

    public List<Layer> layers;
    public List<Sound> sounds;
}

[Serializable]
public class Layer
{
    public string name;

    [Range(0f, 1f)]
    public float volume = 1f;


}