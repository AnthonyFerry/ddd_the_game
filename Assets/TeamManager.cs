﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwissArmyKnife;

public class TeamManager : SingletonPersistent<TeamManager>{

    public GameObject panel = null;

    // Liste de tous les pions disponibles dans le jeu.
    public List<PawnType> availablePawns = new List<PawnType>();
    
    public List<PawnType> playerTeam = new List<PawnType>();

    [SerializeField]
    PawnSelector[] _pawnSelectors;

    void Start()
    {
        SaveManager.Instance.Load();

        if (panel == null) return;

        _pawnSelectors = panel.GetComponentsInChildren<PawnSelector>();
        DisplayCurrentTeam();
    }

    void OnEnable()
    {
        DisplayCurrentTeam();
    }

    public void ValidateTeam()
    {
        playerTeam.Clear();

        foreach(var selector in _pawnSelectors)
        {
            playerTeam.Add(selector.type);
        }
    }

    public void DisplayCurrentTeam()
    {
        for(int i = 0; i < 5; i++)
        {
            _pawnSelectors[i].SelectPawn(availablePawns.IndexOf(playerTeam[i]));
        }
    }

    public void Close(bool saveTeam = false)
    {
    }
}
