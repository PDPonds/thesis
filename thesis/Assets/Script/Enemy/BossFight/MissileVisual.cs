using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileVisual : MonoBehaviour
{

    float speed;

    public void SetupVisual(BossController boss)
    {
        speed = boss.projectileSpeed;
    }

    void Update()
    {
        Vector2 dir = new Vector2(-1, .5f);
        transform.Translate(dir * speed * 2 * Time.deltaTime);
        Destroy(gameObject, 5f);
    }
}
