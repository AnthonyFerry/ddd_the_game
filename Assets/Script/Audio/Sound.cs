using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Sound))]
public class SoundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SoundSettings settings = AudioManager.Instance.soundSettings;

        DrawDefaultInspector();

        Sound _sound = (Sound)target;

        if (settings.sounds.Contains(_sound))
        {
            if (GUILayout.Button("Remove sound from the list"))
            {
                settings.sounds.Remove(_sound);
                EditorUtility.SetDirty(settings);
            }
        }
        else
        {
            if (GUILayout.Button("Add sound to the list"))
            {
                settings.sounds.Add(_sound);
                EditorUtility.SetDirty(settings);
            }
        }
        
    }
}

#endif

[CreateAssetMenu(fileName = "Sound", menuName = "Audio Manager/Sound", order = 1)]
public class Sound : ScriptableObject {

    [Header("Name of the asset")]
    public new string name;

    [Header("The default sound to play")]
    public AudioClip defaultClip;

    [Header("All the sounds related to the same effect")]
    public List<AudioClip> clips;
    
    [Header("Does it loop ?")]
    public bool loop;

    [Header("Can this sound be played multiple times simultaneously ?")]
    public bool multiplePlay;

    [Header("Properties")]
    [Range(0f, 1.5f)]
    public float volume = 1f;

    [Range(-1f, 1f)]
    public float pan = 0f;

    [Range(-3f, 3f)]
    public float pitch = 1f;
}