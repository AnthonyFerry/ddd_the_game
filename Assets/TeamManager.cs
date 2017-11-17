using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwissArmyKnife;

public class TeamManager : Singleton<TeamManager>{

    public List<PawnType> availablePawns = new List<PawnType>();
    public List<PawnSelecter> pawns = new List<PawnSelecter>();
    public GameObject panel;

    void Start()
    {
        DisplayCurrentTeam();
    }

    void OnEnable()
    {
        DisplayCurrentTeam();
    }

    public void ValidateTeam()
    {
        PlayerTeam.Instance.ClearList();

        foreach (PawnSelecter pawn in pawns)
        {
            PlayerTeam.Instance.AddPawn(availablePawns[pawn.selectedPawn]);
        }
    }

    void DisplayCurrentTeam()
    {
        var team = PlayerTeam.Instance.playerPawns;

        for (int i = 0; i < 5; i++)
        {
            pawns[i].SelectPawn(availablePawns.IndexOf(team[i]));
        }
    }

    public void Close(bool saveTeam = false)
    {
        if (saveTeam)
        {
            ValidateTeam();
        }

        panel.SetActive(false);
    }
}
