using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PawnType", menuName = "DDD/PawnType", order = 5)]
public class PawnType : ScriptableObject
{
    public string ScriptName;

    [Header("What's this thing anyway ?")]
    public string Type;

    [Header("Can I walk on it ?")]
    public bool IsCollider;

    [Header("What is it capable of ?")]
    public int moveRange;
    public int attackRange;
    public int minDamages;
    public int maxDamages;

    [Header("How much resistant is it ?")]
    public int armor;
    public int life;

    [Header("What are its weaknesses and strengths ?")]
    public string[] weakness = new string[2];
    public string[] strength = new string[2];

    [Header("What does it looks like ?")]
    public GameObject Reference;

}