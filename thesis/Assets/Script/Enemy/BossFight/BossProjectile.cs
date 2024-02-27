using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float damage;
    public float speed;

    private void Start()
    {
        EnemyBullet[] allBullet = transform.GetComponentsInChildren<EnemyBullet>();
        foreach (EnemyBullet bullet in allBullet)
        {
            bullet.damage = damage;
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }
}
