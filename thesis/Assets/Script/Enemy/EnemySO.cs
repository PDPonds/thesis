using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject
{
    public int maxHp;
    public float damage;
    public float attackSpeed;
    public int minExpDropCount;
    public int maxExpDropCount;
    public int dropCoin;
    public EnemyType type;

}

public enum EnemyType { Range, CloseCombat }
