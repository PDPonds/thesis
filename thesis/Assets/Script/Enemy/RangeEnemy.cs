using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyType/RangeEnemy")]
public class RangeEnemy : EnemySO
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float attackRange;

    public RangeEnemy()
    {
        type = EnemyType.Range;
    }
}
