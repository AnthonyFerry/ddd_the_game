using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntel : SwissArmyKnife.Singleton<EnemyIntel> {

    public bool _enemyLocker = false; // if true, AI is like deactivated
    GameManager _manager;
    LevelTerrain _terrain;
    List<Cell> _cells = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(LevelTerrain t)
    {
        _manager = GameManager.Instance;
        _terrain = t;
    }

    public void BeginTurn()
    {
        BasePawn target;

        if ((target = isAnyPawnInDanger(false)) != null) //if there is any of the AI pawn in danger
        {
            _terrain.searchForNeighbourgs(_terrain.GetCellByPosition(target.PawnLocation), target.moveRange, target.PawnType == "Winged");
            _cells = _terrain.Neighborhood;
            _terrain.Neighborhood.Clear();
            if (_cells != null) {
                target.moveFunction(_cells[Mathf.FloorToInt(Random.Range(0, _cells.Count - 1))]);
            } else {
                if(_terrain.searchForTargets(_terrain.GetCellByPosition(target.PawnLocation), target.attackRange, target.attackType)){

                }
            }
        }
        else if((target = isAnyPawnInDanger(true)) != null) //if there is any of the Player pawn in danger
        {

        }
    }

    BasePawn isAnyPawnInDanger(bool _isPlayer)
    {
        foreach (BasePawn p in _manager.Pawns)
        {
            if (p.isPlayer == _isPlayer && p.health <= ((p.maxHealth / 100) * 20))
            {
                return p;
            }
        }

        return null;
    }

    BasePawn findAdjacentTarget(Cell origin, bool _isPlayer)
    {
        Vector2 OriginLoc = origin.BoardPosition;
        BasePawn Nearest = null;

        foreach (BasePawn p in _manager.Pawns)
        {
            if (p.isPlayer == _isPlayer)
            {
                if (p.PawnLocation.x <= OriginLoc.x + 1 && p.PawnLocation.x >= OriginLoc.x - 1)
                {
                    if (p.PawnLocation.y <= OriginLoc.y + 1 && p.PawnLocation.y >= OriginLoc.y - 1)
                    {
                        Nearest = p;
                    }
                }
            }
        }
        return Nearest;
    }

    
}
