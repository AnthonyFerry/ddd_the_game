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

    public Vector2 PawnLocation { get { return _location; } }

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

    }

    // Update is called once per frame
    protected void Update () {
		
	}

    public void moveFunction(Vector2 newPos, Vector2 nBoardPos) {
        this.transform.position = new Vector3(newPos.x, 0.5f, newPos.y);
        _location = nBoardPos;
    }
}
