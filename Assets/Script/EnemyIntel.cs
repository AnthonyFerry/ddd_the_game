using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntel : SwissArmyKnife.Singleton<EnemyIntel> {

    public bool _enemyLocker = true; // if true, AI is like deactivated
    public ActualStep step = ActualStep.StandBy;
    public float _timerAI = 0.0f;
    public float AiThinkingTime = 1.5f;
    public bool _isThinking = false; // to prevent other actions happening during AI process
    GameManager _manager;
    LevelTerrain _terrain;
    [SerializeField] List<Cell> _cells = null;
    [SerializeField] BasePawn target = null;
    [SerializeField] BasePawn attaker = null;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_enemyLocker) { 
            _timerAI += Time.deltaTime;

            if (_timerAI > AiThinkingTime)
            {
                _timerAI = 0;
                _enemyLocker = true;

                switch (step)
                {
                    case ActualStep.StandBy:
                        break;
                    case ActualStep.beforeMoving:
                        break;
                    case ActualStep.Moving:
                        target = Herio();
                        if (target != null)
                        {
                            //openTimer(ActualStep.afterMoving);
                        }
                        break;
                    case ActualStep.afterMoving:
                        step = ActualStep.StandBy;
                        attaker = Slash();
                        break;
                    default:
                        break;
                }

            }
        }
    }

    public void Init(LevelTerrain t)
    {
        _manager = GameManager.Instance;
        _terrain = t;
    }

    public void BeginTurn()
    {
        _isThinking = true;
        step = ActualStep.beforeMoving;
        attaker = Slash();
        if (attaker == null)
        {
            openTimer(ActualStep.Moving);
        }
    }

    public void openTimer(ActualStep stepepouze)
    {
        step = stepepouze;
        _enemyLocker = false;
    }

    public void BeginTurn_OLD() //Deprecated - dead code
    {
        BasePawn target = null;
        BasePawn attaker = null;

        _isThinking = true;
        /*
        if ((target = isAnyPawnInDanger(false)) != null) //if there is any of the AI pawn in danger
        {
            _terrain.searchForNeighbourgs(_terrain.GetCellByPosition(target.PawnLocation), target.moveRange, target.PawnType == "Winged");
            _cells = _terrain.Neighborhood;
            _terrain.Neighborhood.Clear();
            if (_cells != null) { //if there is some reachable position to escape, then escape !
                target.moveFunction(_cells[Mathf.FloorToInt(Random.Range(0, _cells.Count - 1))]);
            } else { //Otherwise, fight back !
                if(_terrain.searchForTargets(_terrain.GetCellByPosition(target.PawnLocation), 1, target.attackType, target.isPlayer)){
                    BasePawn victim = findAdjacentTarget(_terrain.GetCellByPosition(target.PawnLocation), true);
                    if (target.atkFunction(victim))
                        _manager.destroyPawn(victim);
                }
            }
            return;
        }
        else if((target = isAnyPawnInDanger(true)) != null) //if there is any of the Player pawn in danger
        {

            foreach (BasePawn p in _manager.Pawns)
            {
                if (attaker != null) break;
                if (p.isPlayer == false)
                {
                    if (_terrain.searchForTargets(_terrain.GetCellByPosition(p.PawnLocation), p.attackRange, p.attackType, p.isPlayer) && _manager.distanceBetweenPawns(p, target) <= p.attackRange)
                    {
                        attaker = p;
                    }
                }
            }

            attaker.atkFunction(target);
            return;
        }  
        */
        /*
        //si aucun pion n'est en danger, on regarde si on peut éventuellement attaquer un pion
        foreach (BasePawn p in _manager.Pawns)
        {
            if (p.isPlayer == false)
            {
                Debug.Log("AI searching an available attaker ; testing : " + p.name);
                _terrain.HighlightTargetables(_terrain.GetCellByPosition(p.PawnLocation), p.attackRange, p.attackType);
                if (_terrain.Targetables.Count > 0) {
                    Debug.Log("AI found some targets to attack !");
                    attaker = p;
                    List<BasePawn> _targets = getReachableTargets(attaker);
                    if (_targets != null) {
                        BasePawn victim = _targets[Mathf.FloorToInt(Random.Range(0, _targets.Count - 1))];
                        if (!attaker.atkFunction(victim))
                            _manager.destroyPawn(victim);
                    } else {
                        Debug.Log("For some reasons, AI didn't managed to catch an available target...");
                    }
                    _terrain.ClearTargetables();
                    break;
                }
                _terrain.ClearTargetables();
            }
        }*/
        Debug.Log("Launching Slash 1");
        attaker = Slash();

        Debug.Log("AI -> attaker is null ? : " + (attaker == null));

        if (attaker == null)
        {
            /*
            foreach (BasePawn p in _manager.Pawns)
            {
                if (p.isPlayer == false)
                {
                    Debug.Log("AI select " + p.name);
                    target = p;
                    //_terrain.searchForNeighbourgs(_terrain.GetCellByPosition(target.PawnLocation), target.moveRange, target.PawnType == "Winged");
                    _terrain.HighlightNeighbourgs(_terrain.GetCellByPosition(target.PawnLocation), target.moveRange, target.PawnType == "Winged");
                    _cells = _terrain.Neighborhood;

                    if (_cells != null)
                    {
                        Cell depart = _cells[Mathf.FloorToInt(Random.Range(0, _cells.Count - 1))];
                        target.moveFunction(depart);
                        depart.SetState(CellState.free);
                        _terrain.ClearNeighbourgs();
                        _terrain.refreshOccupationMap();
                        break;
                    }
                    _terrain.ClearNeighbourgs();
                    _terrain.refreshOccupationMap();
                    target = null;
                }
            }
            */
            Debug.Log("Launching Herio 1");
            target = Herio();
        }

        if (target != null)
        {
            Debug.Log("Launching Slash 2");
            //target = Slash();
        }

        //_isThinking = false;
    }

    //to attack
    BasePawn Slash()
    {
        BasePawn atk = null;

        Debug.Log("Entering Slash");

        foreach (BasePawn p in _manager.Pawns)
        {
            if (p.isPlayer == false)
            {
                Debug.Log("AI searching an available attaker ; testing : " + p.name);
                _terrain.HighlightTargetables(_terrain.GetCellByPosition(p.PawnLocation), p.attackRange, p.attackType, p.isPlayer);
                if (_terrain.Targetables.Count > 0)
                {
                    Debug.Log("AI found some targets to attack !");
                    atk = p;
                    List<BasePawn> _targets = getReachableTargets(atk);
                    Debug.Log("Nb cibles trouvées  : "+_targets.Count);
                    if (/*_targets != null && */_targets.Count > 0)
                    {
                        Debug.Log("Attaaaaaaaaaaack !");
                        BasePawn victim = _targets[Mathf.FloorToInt(Random.Range(0, _targets.Count - 1))];
                        if (!atk.atkFunction(victim))
                            _manager.destroyPawn(victim);
                    }
                    else {
                        Debug.Log("For some reasons, AI didn't managed to catch an available target...");
                        atk = null;
                    }
                    _terrain.ClearTargetables();
                    break;
                }
                _terrain.ClearTargetables();
            }
        }
        _terrain.refreshOccupationMap();
        Debug.Log("Exiting Slash");
        return atk;
    }

    //to move
    BasePawn Herio()
    {
        BasePawn targeto = null;

        foreach (BasePawn p in _manager.Pawns)
        {
            if (p.isPlayer == false)
            {
                Debug.Log("AI select " + p.name);
                targeto = p;
                //_terrain.searchForNeighbourgs(_terrain.GetCellByPosition(target.PawnLocation), target.moveRange, target.PawnType == "Winged");
                _terrain.HighlightNeighbourgs(_terrain.GetCellByPosition(targeto.PawnLocation), targeto.moveRange, targeto.PawnType == "Winged");
                _cells = _terrain.Neighborhood;

                if (_cells != null)
                {
                    Cell depart = _cells[Mathf.FloorToInt(Random.Range(0, _cells.Count - 1))];
                    targeto.moveFunction(depart);
                    depart.SetState(CellState.free);
                    _terrain.ClearNeighbourgs();
                    _terrain.refreshOccupationMap();
                    break;
                }
                _terrain.ClearNeighbourgs();
                _terrain.refreshOccupationMap();
                //target = null;
            }
        }
        return targeto;
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

    /// <summary>
    /// A function to find some Pawn in the close neighborhood (1 cell away)
    /// </summary>
    /// <param name="origin">The original position</param>
    /// <param name="_isPlayer">The allegiance of the pawn</param>
    /// <returns></returns>
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

    List<BasePawn> getReachableTargets(BasePawn attaker)
    {
        Debug.Log("Entering getReachableTargets");
        List<BasePawn> _pawns = new List<BasePawn>();
        float atkRange = attaker.attackRange + 0.25f;
        Debug.Log("atkRange : " + atkRange);
        bool isDatPlayer = attaker.isPlayer;

        foreach(BasePawn p in _manager.Pawns)
        {
            if (p.isPlayer != isDatPlayer)
            {
                Debug.Log("Checking " + p.name);
                float d = _manager.distanceBetweenPawns(attaker, p);
                Debug.Log("distance : " + d);
                if (d <= atkRange)
                {
                    Debug.Log(p.name+" is available");
                    _pawns.Add(p);
                }
                else
                {
                    Debug.Log(p.name+" is too far away.");
                }
            }
        }

        return _pawns;
    }
    
}

public enum ActualStep
{
    StandBy,
    beforeMoving,
    Moving,
    afterMoving
}
