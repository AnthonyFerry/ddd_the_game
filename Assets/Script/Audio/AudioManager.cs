using SwissArmyKnife;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// This class allows to manage audio resources as a static object pooler
/// </summary>
public class AudioManager : SingletonPersistent<AudioManager>{

    [Header("How many slots ?")]
    public int pooledAmount = 10;

    [Header("Settings Asset")]
    public SoundSettings soundSettings;

    [Header("Mixers")]
    public AudioMixer mainMixer;

    List<SoundSlot> _slots = new List<SoundSlot>();

    void Start()
    {
        if (soundSettings == null)
        {
            Debug.LogError("Sound Settings are missing.");
            return;
        }
        else
            CreateSlots();

    }

    #region Internal
    void CreateSlots()
    {
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject slot = new GameObject();
            slot.transform.SetParent(this.transform);

            _slots.Add(slot.AddComponent<SoundSlot>());
        }
    }

    SoundSlot GetEmptySlot()
    {
        foreach (var slot in _slots)
            if (slot.isEmpty)
                return slot;

        return null;
    }

    Sound GetSoundByName(string soundName)
    {
        foreach (var sound in soundSettings.sounds)
            if (sound.name == soundName.Trim())
                return sound;

        return null;
    }

    void InternalPlay(string soundName, bool playRandom = false, string newTag = "")
    {
        var slot = GetEmptySlot();
        var sound = GetSoundByName(soundName);
        
        if (!sound.multiplePlay)
        {
            foreach (var s in _slots)
                if (s.sound == sound && s.isPlaying)
                    return;
        }

        slot.Play(sound, playRandom, newTag);
    }

    void InternalStopByTag(string tag)
    {
        foreach (var slot in _slots)
            if (slot.tag == tag)
                slot.Stop();
    }

    bool InternalIsPlaying(string tag)
    {
        foreach (var slot in _slots)
        {
            if (slot.tag == tag)
                return true;
        }
        return false;
    }

    #endregion

    /// <summary>
    /// Play a sound by its name.
    /// </summary>
    /// <param name="soundName">Refers to the name field of the sound asset</param>
    /// <param name="playRandom">If true, play randomly a sound in the clips of the sound asset. If false, play the default clip</param>
    /// <param name="newTag">Tag to identify the sound</param>
    static public void Play(string soundName, bool playRandom = false, string newTag = "")
    {
        Instance.InternalPlay(soundName, playRandom, newTag);
    }

    /// <summary>
    /// Stop all the sounds by their tag.
    /// </summary>
    /// <param name="tag"></param>
    static public void StopByTag(string tag)
    {
        Instance.InternalStopByTag(tag);
    }

    static public bool IsPlaying(string tag)
    {
        return Instance.InternalIsPlaying(tag);
    }


}
