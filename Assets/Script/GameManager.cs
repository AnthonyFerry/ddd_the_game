using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

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

    public List<BasePawn> Pawns {
        get { return _pawns; }
    }

	// Use this for initialization
	void Start () {
        _terrain._manager = this;
        _terrain.BuildTerrain(3);
        //createPawn(_pawnTypes[0]);
        createPawn(createPawnData(_pawnTypes[0], new Vector3(0, 0, 0), new Vector2(0, 0), true));
        createPawn(createPawnData(_pawnTypes[1], _terrain.GetCellByPosition(5, 1).gameObject.transform.position, new Vector2(5, 1), true));
        createPawn(createPawnData(_pawnTypes[2], _terrain.GetCellByPosition(5, 3).gameObject.transform.position, new Vector2(5, 3), false));
        _interface.Init(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void createPawn(PawnData datum) {
        GameObject character = Instantiate(datum.type.Reference, _terrain.gameObject.transform);
        character.gameObject.transform.position = datum.location;
    
        BasePawn newBasePawn = character.AddComponent(Type.GetType(datum.type.ScriptName)) as BasePawn;
        _pawns.Add(newBasePawn);

        character.name = datum.type.Type + "_" + _pawns.Count;
        _terrain.GetCellByPosition(datum.boardPosition).SetState(CellState.occupied);
        newBasePawn.Init(datum);

    }

    PawnData createPawnData(PawnType type, Vector3 loc, Vector2 board_pos, bool player) {
        PawnData newData;
        newData.type = type;
        newData.location = loc;
        newData.boardPosition = board_pos;
        newData.isPlayer = player;
        return newData;
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
}

public struct PawnData {
    public Vector3 location;
    public PawnType type;
    public Vector2 boardPosition;
    public bool isPlayer;
}
