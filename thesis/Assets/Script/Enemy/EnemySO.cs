using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject
{
    public int maxHp;
    public EnemyType type;
    public float damage;
    public float attackSpeed;
    public int minExpDropCount;
    public int maxExpDropCount;
    public int dropCoin;
}

public enum EnemyType { Range, CloseCombat }
