using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SwissArmyKnife;

public class TeamManager : SingletonPersistent<TeamManager> {

    public GameObject panel = null;

    // Liste de tous les types de pions disponibles dans le jeu.
    public List<PawnType> availablePawns = new List<PawnType>();

    // Liste des pions détenus par le joueur
    public List<PawnType> playerTeam = new List<PawnType>();

    [SerializeField]
    PawnSelector[] _pawnSelectors = new PawnSelector[5];

    void Start()
    {
        SaveManager.Load();

        if (panel == null) return;
    }

    public void ValidateTeam()
    {
        playerTeam.Clear();

        foreach(var selector in _pawnSelectors)
        {
            playerTeam.Add(selector.type);
        }

        SaveManager.SaveTeam();
    }

    public void DisplayCurrentTeam()
    {
        GetPawnSelectors();

        for(int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            _pawnSelectors[i].SelectPawn(availablePawns.IndexOf(playerTeam[i]));
        }
    }

    public void Close(bool saveTeam = false)
    {
    }

    void GetPawnSelectors()
    {
        if (_pawnSelectors[0] == null)
            _pawnSelectors = panel.GetComponentsInChildren<PawnSelector>();
    }
}
