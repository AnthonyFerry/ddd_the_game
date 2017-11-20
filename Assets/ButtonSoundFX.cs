using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundFX : MonoBehaviour {

    [SerializeField]
    string _soundName;

    [SerializeField]
    string _tag;

    [SerializeField]
    bool _randomPlay;


	public void PlaySound()
    {
        AudioManager.Play(_soundName, _randomPlay, _tag);
    }
}
