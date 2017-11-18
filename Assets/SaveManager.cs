using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwissArmyKnife;


/// <summary>
/// Cette classe sert à sauvegarder la progression du joueur. Sont sauvegardés les MenuDatas ainsi que PlayerTeam.
/// </summary>
public class SaveManager : SingletonPersistent<SaveManager> {

    const string FIRST_PAWN = "first";
    const string SECOND_PAWN = "second";
    const string THIRD_PAWN = "third";
    const string FOURTH_PAWN = "fourth";
    const string FIFTH_PAWN = "fifth";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Save()
    {
        var tm = TeamManager.Instance;

        PlayerPrefs.SetInt(FIRST_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[0]));
        PlayerPrefs.SetInt(SECOND_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[1]));
        PlayerPrefs.SetInt(THIRD_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[2]));
        PlayerPrefs.SetInt(FOURTH_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[3]));
        PlayerPrefs.SetInt(FIFTH_PAWN, tm.availablePawns.IndexOf(tm.playerTeam[4]));

        PlayerPrefs.Save();
    }

    public void Load()
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
