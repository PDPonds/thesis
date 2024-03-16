using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<IDamageable>(out IDamageable idamageable))
            {
                EnemyController enemy = collision.GetComponent<EnemyController>();
                Animator anim = enemy.GetComponent<Animator>();
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
                {
                    StartCoroutine(EnemyTakeDamage(collision, idamageable));
                }
            }
        }

        if (collision.CompareTag("Boss"))
        {
            if (collision.TryGetComponent<BossController>(out BossController idamageable))
            {
                StartCoroutine(EnemyTakeDamage(collision, idamageable));
            }
        }

        //if (collision.CompareTag("Weakspot"))
        //{
        //    if (collision.TryGetComponent<WeakSpot>(out WeakSpot weakSpot))
        //    {
        //        StartCoroutine(EnemyTakeDamage(collision, weakSpot.bossController));
        //        weakSpot.RemoveWeakSpotHP();
        //        GameManager.Instance.SpawnParticle(GameManager.Instance.weakspotParticle, transform.position, true);
        //        Destroy(gameObject);
        //    }
        //}

        if (collision.CompareTag("Ground"))
        {
            SoundManager.Instance.PlayOnShot("LaserHit");
            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, transform.position);
            Destroy(gameObject);
        }

    }

    IEnumerator EnemyTakeDamage(Collider2D collision, IDamageable damageable)
    {
        SoundManager.Instance.PlayOnShot("LaserHit");
        //GameObject hitPar = GameManager.Instance.hitParticle;
        //GameManager.Instance.SpawnParticle(hitPar, collision.transform.position, true);
        //GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);
        yield return StartCoroutine(damageable.TakeDamage());
        Destroy(gameObject);
    }
}
