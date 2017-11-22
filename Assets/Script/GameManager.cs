using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SwissArmyKnife.Singleton<GameManager> {

    [SerializeField]
    [Header("The battlefield")]
    LevelTerrain _terrain;

    [SerializeField]
    [Header("The interface")]
    UI_Management _interface;

    [SerializeField]
    [Header("A list of the pawn types we can use in our game")]
    List<PawnType> _pawnTypes = null;    //From 0 to 4 : Tank, Assassin, Archer, Wizard, Winged

    [SerializeField]
    [Header("A list of the pawns currently played")]
    List<BasePawn> _pawns = null;


    [Header("Useful variables for the party")]
    [SerializeField] EnemyIntel _motherFucker;
    [SerializeField] bool _isPlayerTurn = true;
    [SerializeField] int _nbPlayerPawn = 0;
    [SerializeField] int _nbEnemyPawn = 0;
    [SerializeField] GameState _locker = GameState.unlocked; // Ragnar Lock-Broke !
    public BasePawn currentlyPlayedPawn = null;

    public List<BasePawn> Pawns {
        get { return _pawns; }
    }

    public GameState Locker {
        get { return _locker; }
        set { _locker = value; }
    }
    
    public bool IsPlayerTurn {
        get { return _isPlayerTurn; }
    }

	// Use this for initialization
	void Start () {
        _motherFucker = EnemyIntel.Instance;
        _motherFucker.Init(_terrain);
        _isPlayerTurn = true;
        _terrain._manager = this;
        //createPawn(createPawnData(_pawnTypes[0], new Vector3(0, 0, 0), new Vector2(0, 0), true));
        //createPawn(createPawnData(_pawnTypes[1], _terrain.GetCellByPosition(5, 1).gameObject.transform.position, new Vector2(5, 1), true));
        //createPawn(createPawnData(_pawnTypes[2], _terrain.GetCellByPosition(5, 3).gameObject.transform.position, new Vector2(5, 3), false));
        //createPawn(createPawnData(_pawnTypes[0], _terrain.GetCellByPosition(6, 3).gameObject.transform.position, new Vector2(6, 3), false));
        //createPawn(createPawnData(_pawnTypes[4], _terrain.GetCellByPosition(3, 6).gameObject.transform.position, new Vector2(3, 6), true));
        _interface.Init(this);
    }

    // Update is called once per frame
    void Update() {
        if (_locker == GameState.endgame)
            return;

        if (_nbPlayerPawn == 0 && _nbEnemyPawn != 0) {
            //defeat function
            Debug.Log("Defeat");
            _locker = GameState.endgame;
            _interface.callEndGame(false);
        } else if (_nbEnemyPawn == 0 && _nbPlayerPawn != 0) {
            //victory function
            Debug.Log("Victory");
            _locker = GameState.endgame;
            MenuDatas.Instance.UnlockNextLevel();
            _interface.callEndGame(true);
        }

        if (_locker == GameState.unlocked) {
            if (!_isPlayerTurn)
            {
                //AI
                TurnTransition();
            }
        }
		
	}

    /// <summary>
    /// The actual function which create a Pawn on the battlefield, using informations provided by a PawnData.
    /// </summary>
    /// <param name="datum">The Pawn data that contain... well, all important data</param>
    public void createPawn(PawnData datum) {
        GameObject character = Instantiate(datum.type.Reference, _terrain.gameObject.transform);
        character.gameObject.transform.position = datum.location;
    
        BasePawn newBasePawn = character.AddComponent(Type.GetType(datum.type.ScriptName)) as BasePawn;
        _pawns.Add(newBasePawn);

        if (datum.isPlayer) {_nbPlayerPawn += 1; } else { _nbEnemyPawn += 1; }

        character.name = datum.type.Type + "_" + _pawns.Count;
        _terrain.GetCellByPosition(datum.boardPosition).SetState(CellState.occupied);
        newBasePawn.Init(datum);

    }

    /// <summary>
    /// Function which create a PawnData, mostly useful for pawn creation.
    /// </summary>
    /// <param name="type">The PawnType of the Pawn</param>
    /// <param name="loc">Its location in the world</param>
    /// <param name="board_pos">Its location on the gameboard</param>
    /// <param name="player">Does it belong to the player ? True for player, False for AI</param>
    public PawnData createPawnData(PawnType type, Vector3 loc, Vector2 board_pos, bool player) {
        PawnData newData;
        newData.type = type;
        newData.location = loc;
        newData.boardPosition = board_pos;
        newData.isPlayer = player;
        return newData;
    }

    public void destroyPawn(BasePawn p) {
        if (p.isPlayer) { _nbPlayerPawn -= 1; } else { _nbEnemyPawn -= 1; }
        _terrain.GetCellByPosition(p.PawnLocation).SetState(CellState.free);
        Destroy(p.gameObject);
        _pawns.Remove(p);
    }

    public bool isPawnExisting(int x, int y) { return isPawnExisting(new Vector2(x, y)); }

    public bool isPawnExisting(Vector2 pos) {

        foreach (BasePawn p in _pawns) {
            if (p.PawnLocation == pos) {
                return true;
            }
        }
        return false;
    }

    public BasePawn getPawnByLocation(int x, int y) { return getPawnByLocation(new Vector2(x, y)); }

    public BasePawn getPawnByLocation(Vector2 pos) {
        foreach (BasePawn p in _pawns)
        {
            if (p.PawnLocation == pos)
            {
                return p;
            }
        }
        return null;
    }

    public PawnType GetPawnData(int index)
    {
        return _pawnTypes[index] == null ? null : _pawnTypes[index];
    }

    public int getRemainingOpponentsAmount(bool isPlayer)
    {
        return isPlayer ? _nbEnemyPawn : _nbPlayerPawn;
    }

    //called in BasePawn.AtkFunction()
    public void TurnTransition()
    {
        _locker = GameState.locked;
        if (currentlyPlayedPawn != null) currentlyPlayedPawn.hasAlreadyMoved = false;
        currentlyPlayedPawn = null;
        Debug.Log("Locked !");
        _isPlayerTurn = !_isPlayerTurn;
        _interface.changingTurn(_isPlayerTurn);
    }
}

public struct PawnData {
    public Vector3 location;
    public PawnType type;
    public Vector2 boardPosition;
    public bool isPlayer;
}

public enum GameState
{
    unlocked,
    locked,
    endgame
}