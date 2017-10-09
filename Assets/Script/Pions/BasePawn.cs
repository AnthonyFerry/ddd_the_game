using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePawn : MonoBehaviour {

    [SerializeField]
    protected PawnType Type;

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
    protected Vector3 _location; //Position on the battlefield, to know on which case of my board is this Pawn.
    [SerializeField]
    protected Vector3 _worldPosition;

    public virtual void Init(PawnData datum) {
        Type = datum.type;
        health = Type.life;
        armor = Type.armor;
        moveRange = Type.moveRange;
        attackRange = Type.attackRange;
        minDamages = Type.minDamages;
        maxDamages = Type.maxDamages;

        _worldPosition = datum.location;
    }

    // Update is called once per frame
    protected void Update () {
		
	}
}
