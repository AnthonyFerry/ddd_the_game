using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwissArmyKnife;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    override public void OnInspectorGUI()
    {

        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        if (GUILayout.Button("Save Progression"))
        {
            SaveManager.SaveProgression();
        }

        if (GUILayout.Button("Save Team"))
        {
            SaveManager.SaveTeam();
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        if (GUILayout.Button("Load Progression"))
        {
            SaveManager.LoadProgression();
        }

        if (GUILayout.Button("Load Team"))
        {
            SaveManager.Load();
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Clear Datas"))
        {
            PlayerPrefs.DeleteAll();
        }

        DrawDefaultInspector();
    }
}

#endif

/// <summary>
/// Cette classe sert à sauvegarder la progression du joueur. Sont sauvegardés les MenuDatas ainsi que PlayerTeam.
/// </summary>
public class SaveManager : MonoBehaviour {

    const string FIRST_PAWN = "first";
    const string SECOND_PAWN = "second";
    const string THIRD_PAWN = "third";
    const string FOURTH_PAWN = "fourth";
    const string FIFTH_PAWN = "fifth";

    static public void SaveTeam()
    {
        var tm = TeamManager.Instance;

        PlayerPrefs.SetInt(FIRST_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[0]));
        PlayerPrefs.SetInt(SECOND_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[1]));
        PlayerPrefs.SetInt(THIRD_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[2]));
        PlayerPrefs.SetInt(FOURTH_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[3]));
        PlayerPrefs.SetInt(FIFTH_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[4]));

        PlayerPrefs.Save();
    }

    static public void SaveProgression()
    {
        var worlds = MenuDatas.Instance.worlds;

        int i = 0;
        int j = 0;
        int levelCount = 0;
        for (i = 0; i < worlds.Count; i++)
        {
            levelCount = 0;

            PlayerPrefs.SetInt("World" + i, worlds[i].isLocked ? 0 : 1);

            for (j = 0; j < worlds[i].levels.Count; j++)
            {
                if (!worlds[i].levels[j].isLocked) levelCount++;
            }

            PlayerPrefs.SetInt("World" + i + "levels", levelCount);
        }

        PlayerPrefs.Save();
    }

    static public void LoadProgression()
    {
        var worlds = MenuDatas.Instance.worlds;

        int i = 0;
        int j = 0;
        for (i = 0; i < worlds.Count; i++)
        {
            worlds[i].isLocked = PlayerPrefs.GetInt("World" + i) == 0 ? true : false;

            int levelCount = PlayerPrefs.GetInt("World" + i + "levels");

            for (j = 0; j < worlds[i].levels.Count; j++)
            {
                worlds[i].levels[j].isLocked = j < levelCount ? false : true;
            }
        }
    }

    static public void Load()
    {
        var tm = TeamManager.Instance;
        Debug.Log(PlayerPrefs.GetInt(FIRST_PAWN) + "" + PlayerPrefs.GetInt(SECOND_PAWN) + "" + PlayerPrefs.GetInt(THIRD_PAWN) + "" + PlayerPrefs.GetInt(FOURTH_PAWN) + "" + PlayerPrefs.GetInt(FIFTH_PAWN));
        tm.playerTeam.Add(tm.availablePawns[PlayerPrefs.GetInt(FIRST_PAWN)]);
        tm.playerTeam.Add(tm.availablePawns[PlayerPrefs.GetInt(SECOND_PAWN)]);
        tm.playerTeam.Add(tm.availablePawns[PlayerPrefs.GetInt(THIRD_PAWN)]);
        tm.playerTeam.Add(tm.availablePawns[PlayerPrefs.GetInt(FOURTH_PAWN)]);
        tm.playerTeam.Add(tm.availablePawns[PlayerPrefs.GetInt(FIFTH_PAWN)]);
    }
}
