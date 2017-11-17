using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeam : SwissArmyKnife.SingletonPersistent<PlayerTeam> {


    public List<PawnType> playerPawns = new List<PawnType>(5);

    void Start()
    {

    }

    public PawnType GetPawn(int index)
    {
        return playerPawns[index] != null ? playerPawns[index] : null;
    }

    public void AddPawn(PawnType newPawn)
    {
        if (newPawn != null)
            playerPawns.Add(newPawn);
    }

    public void ClearList()
    {
        playerPawns.Clear();
    }
}
