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
                StartCoroutine(EnemyTakeDamage(collision, idamageable));

            }
        }

        if (collision.CompareTag("Boss"))
        {
            if (collision.TryGetComponent<BossController>(out BossController idamageable))
            {
                if (idamageable.curBehavior == BossBehavior.Weakness)
                {
                    StartCoroutine(EnemyTakeDamage(collision, idamageable));

                }
                else
                {
                    GameObject hitPar = GameManager.Instance.hitParticle;
                    GameManager.Instance.SpawnParticle(hitPar, transform.position);
                    Destroy(gameObject);
                }
            }
        }

        if (collision.CompareTag("Weakspot"))
        {
            if (collision.TryGetComponent<WeakSpot>(out WeakSpot weakSpot))
            {
                GameObject hitPar = GameManager.Instance.hitParticle;
                GameManager.Instance.SpawnParticle(hitPar, collision.transform.position, true);
                GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);

                weakSpot.RemoveWeakSpotHP();

                Destroy(gameObject);
            }
        }

        if (collision.CompareTag("Ground"))
        {
            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, transform.position);
            Destroy(gameObject);
        }

    }

    IEnumerator EnemyTakeDamage(Collider2D collision, IDamageable damageable)
    {
        GameObject hitPar = GameManager.Instance.hitParticle;
        GameManager.Instance.SpawnParticle(hitPar, collision.transform.position, true);
        //GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);
        yield return StartCoroutine(damageable.TakeDamage());
        Destroy(gameObject);
    }
}
