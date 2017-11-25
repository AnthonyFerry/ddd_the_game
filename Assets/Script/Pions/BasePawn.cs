﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePawn : MonoBehaviour {

    [SerializeField]
    protected PawnType _type;
    public bool isPlayer = true;
    public AtkSys attackType;

    [Header("Life values")]
    public float health = 0;
    public float maxHealth = 0;
    public int armor = 0;

    [Header("Movement values")]
    public int moveRange = 0;
    public bool walkThroughWalls = false;
    public bool hasAlreadyMoved = false; //to know if this pawn already moved this turn.

    [Header("Attack values")]
    public int attackRange = 0;
    public int minDamages = 0;
    public int maxDamages = 0;
    public bool isWallPass = false;

    [SerializeField]
    protected Vector2 _location; //Position on the battlefield, to know on which case of my board is this Pawn.
    [SerializeField]
    protected Vector3 _worldPosition;

    protected PawnMovement _movements;
    protected LevelTerrain _terrain;
    UI_Pawn_Icon _icon;

    /// <summary>
    /// Refers to the position on the gameboard
    /// </summary>
    public Vector2 PawnLocation { get { return _location; } }

    /// <summary>
    /// Refers to the real position, understood by Unity
    /// </summary>
    public Vector3 WorldPosition { get { return _worldPosition; } }

    /// <summary>
    /// To get the specific type of a BasePawn
    /// </summary>
    public string PawnType { get { return _type.Type; } }
    public PawnType type { get { return _type; } }

    public virtual void Init(PawnData datum) {
        _type = datum.type;
        maxHealth = health = _type.life;
        armor = _type.armor;
        moveRange = _type.moveRange;
        attackRange = _type.attackRange;
        minDamages = _type.minDamages;
        maxDamages = _type.maxDamages;
        isPlayer = datum.isPlayer;
        attackType = _type.attackType;

        _worldPosition = datum.location;
        _location = datum.boardPosition;

        _icon = UI_Management.Instance.createIcon(this, isPlayer);
        _terrain = FindObjectOfType<LevelTerrain>();
        _movements = gameObject.AddComponent<PawnMovement>();
        _movements.Init(this, _terrain);

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
            AudioManager.Play("Movement", true);
        }
        else
        {
            Debug.Log("destination given is null");
        }
    }

    /// <summary>
    /// If return true, the attacked pawn is still alive.
    /// </summary>
    public virtual bool atkFunction(BasePawn target) {
        AudioManager.Play(type.SoundName);
        bool result = target.takeDamages(this.dealDamages(target.PawnType));
        if (GameManager.Instance.getRemainingOpponentsAmount(this.isPlayer) > 0/* && result*/)
        {
            if (result || GameManager.Instance.getRemainingOpponentsAmount(this.isPlayer) > 1)
            {
                GameManager.Instance.TurnTransition();
            }
            else
            {
                Debug.Log("On ne passe pas ! <2>");
            }
        }
        else
        {
            Debug.Log("On ne passe pas ! <1>");
        }
        return result;
    }

    /// <summary>
    /// Called when a Pawn take damages. If return true, the Pawn is still alive. Otherwise, it is destroyed.
    /// </summary>
    public bool takeDamages(int dmg) {
        Debug.Log(this.name + " health before taking damages is " + health);
        Debug.Log(this.name + " protection is " + (armor * 2));
        health = health - (dmg/* - armor*/);
        Debug.Log(this.name + " health after calcul is " + health);
        _icon.updateDamages(health);

        if (health <= 0) {
            Debug.Log("Oh no ! "+this.name+" has been destroyed !");
            return false;
        }
        return true;
    }

    public int dealDamages(string target)
    {
        float multi = 1.0f;
        if (target == _type.strength[0] || target == _type.strength[1]) {
            multi = 1.25f;
            Debug.Log("target is weak !");
        } else if (target == _type.weakness[0] || target == _type.weakness[1]) {
            multi = 0.75f;
            Debug.Log("target is too strong...");
        }
        int dmg = Mathf.FloorToInt(Random.Range(minDamages, maxDamages) * multi);
        Debug.Log(this.name + " deals some damages : " + dmg);
        return dmg;
    }
}
