using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundState { empty, playing, paused }

public class SoundSlot : MonoBehaviour {

    public Sound sound { set { _sound = value; } get { return _sound; } }
    public bool isPlaying { get { return (state != SoundState.empty); } }
    public bool isPaused { get { return state == SoundState.paused; } }
    public bool isEmpty { get { return state == SoundState.empty; } }
    public new string tag = "";

    public SoundState state = SoundState.empty;
    AudioSource _audioSource;
    Sound _sound;

    void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        gameObject.name = "empty slot";
    }

    void Update()
    {
        if (_audioSource.isPlaying)
        {
            if (state != SoundState.playing)
                state = SoundState.playing;
        }
        else if (state != SoundState.empty)
            FreeSlot();
    }

    public void Play(Sound newSound, bool playRandom = false, string newTag = "")
    {
        sound = newSound;

        if (sound == null) return;

        gameObject.name = sound.name;
        _audioSource.volume = sound.volume;
        _audioSource.panStereo = sound.pan;
        _audioSource.pitch = sound.pitch;
        _audioSource.loop = sound.loop;

        if (playRandom)
            _audioSource.clip = sound.clips[Random.Range(0, sound.clips.Count)];
        else
            _audioSource.clip = sound.defaultClip;

        tag = newTag;
        _audioSource.Play();
    }

    public void Pause()
    {
        if (!isPaused)
        {
            state = SoundState.paused;
            _audioSource.Pause();
        }
    }

    public void UnPause()
    {
        if (isPaused)
        {
            state = SoundState.playing;
            _audioSource.UnPause();
        }
    }

    public void Stop()
    {
        FreeSlot();
    }

    public void TooglePause()
    {
        if (isPlaying)
        {
            Pause();
        }
        else
        {
            UnPause();
        }
    }

    void FreeSlot()
    {
        state = SoundState.empty;
        sound = null;
        _audioSource.clip = null;
        gameObject.name = "empty slot";
        tag = "";
    }


}
