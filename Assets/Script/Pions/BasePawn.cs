using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePawn : MonoBehaviour {

    [SerializeField]
    protected PawnType Type;
    public bool isPlayer = true;

    [Header("Life values")]
    public int health = 0;
    public int armor = 0;

    [Header("Movement values")]
    public int moveRange = 0;
    public bool walkThroughWalls = false;

    [Header("Attack values")]
    public int attackRange = 0;
    public int minDamages = 0;
    public int maxDamages = 0;
    public bool isWallPass = false;

    [SerializeField]
    protected Vector2 _location; //Position on the battlefield, to know on which case of my board is this Pawn.
    [SerializeField]
    protected Vector3 _worldPosition;

    PawnMovement _movements;

    /// <summary>
    /// Refers to the position on the gameboard
    /// </summary>
    public Vector2 PawnLocation { get { return _location; } }

    /// <summary>
    /// Refers to the real position, understood by Unity
    /// </summary>
    public Vector3 WorldPosition { get { return _worldPosition; } }

    public virtual void Init(PawnData datum) {
        Type = datum.type;
        health = Type.life;
        armor = Type.armor;
        moveRange = Type.moveRange;
        attackRange = Type.attackRange;
        minDamages = Type.minDamages;
        maxDamages = Type.maxDamages;

        _worldPosition = datum.location;
        _location = datum.boardPosition;

        _movements = gameObject.AddComponent<PawnMovement>();
        _movements.Init(this, FindObjectOfType<LevelTerrain>());

    }

    public void updateLocation(int Xmod, int Ymod)
    {
        _location.x += Xmod;
        _location.y += Ymod;
    }

    // Update is called once per frame
    protected void Update () {
        _worldPosition = this.gameObject.transform.position;
        _movements.InternalUpdate();
        
    }

    public void moveFunction(Cell destination) {
        
        if (destination != null)
        {
            Debug.Log(destination.gameObject.name);
            _movements.NewDestination(destination);
        }
        else
        {
            Debug.Log("destination given is null");
        }
    }
}
