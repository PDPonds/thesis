using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed;
    public float damage;

    private void Start()
    {
        EnemyBullet[] allBullets = transform.GetComponentsInChildren<EnemyBullet>();
        foreach (EnemyBullet bullet in allBullets)
        {
            bullet.damage = damage;
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * speed);
    }


}
