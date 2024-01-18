using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyType/CloseCombat")]
public class CloseCombatEnemy : EnemySO
{
    public CloseCombatEnemy()
    {
        type = EnemyType.CloseCombat;
    }
}
