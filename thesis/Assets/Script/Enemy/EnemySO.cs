using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject
{
    public int maxHp;
    public EnemyType type;
    public float attackSpeed;
}

public enum EnemyType { Range, CloseCombat }
